using System;
using System.Data;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormTransaksi : Form
    {
        DAL dal = new DAL();

        string roleUser;
        int userId;

        public FormTransaksi(string role, int id)
        {
            InitializeComponent();
            roleUser = role;
            userId = id;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            if (roleUser != "admin") btnLaporan.Visible = false;

            if (dgvTransaksi.Columns.Count == 0)
            {
                dgvTransaksi.Columns.Add("bukuId", "ID Buku");
                dgvTransaksi.Columns.Add("judul", "Judul Buku");
                dgvTransaksi.Columns.Add("harga", "Harga");
                dgvTransaksi.Columns.Add("jumlah", "Qty");
                dgvTransaksi.Columns.Add("subtotal", "Subtotal");
            }
            txtTotal.ReadOnly = true;
            txtTotal.Text = "0";

            try
            {
                // 3-TIER: Mengambil daftar buku dari fungsi DAL
                DataTable dt = dal.GetBukuSimple();
                cbBuku.DataSource = dt;
                cbBuku.DisplayMember = "judul";
                cbBuku.ValueMember = "bukuId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar buku: " + ex.Message, "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbBuku_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBuku.SelectedItem is DataRowView row)
            {
                txtHarga.Text = row["hargaSatuan"].ToString();
                txtHarga.ReadOnly = true;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // 1. Validasi Input Dasar
            if (!int.TryParse(txtJumlah.Text, out int jumlahInput) || jumlahInput < 1)
            {
                MessageBox.Show("Jumlah minimal 1 buku dan harus berupa angka!");
                return;
            }

            int bukuId = Convert.ToInt32(cbBuku.SelectedValue);
            int jumlahDiKeranjang = 0;

            // 2. HITUNG AKUMULASI
            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToInt32(row.Cells["bukuId"].Value) == bukuId)
                {
                    jumlahDiKeranjang = Convert.ToInt32(row.Cells["jumlah"].Value);
                    break;
                }
            }

            int totalAkanDibeli = jumlahInput + jumlahDiKeranjang;

            try
            {
                // 3-TIER: Mengambil stok dari database lewat fungsi DAL
                int stokGudang = dal.GetStokBuku(bukuId);

                // *Catatan Sidang*: Beri komentar (/* */) pada blok IF di bawah ini jika ingin menguji kekuatan TRIGGER database-mu secara jebol
                if (totalAkanDibeli > stokGudang)
                {
                    MessageBox.Show($"Stok tidak mencukupi!\n" +
                                    $"Di Gudang: {stokGudang}\n" +
                                    $"Sudah di Keranjang: {jumlahDiKeranjang}\n" +
                                    $"Maksimal yang bisa ditambah: {stokGudang - jumlahDiKeranjang}");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memeriksa stok database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Update atau Tambah ke Grid
            string judul = cbBuku.Text;
            int harga = Convert.ToInt32(txtHarga.Text);
            bool found = false;

            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToInt32(row.Cells["bukuId"].Value) == bukuId)
                {
                    row.Cells["jumlah"].Value = totalAkanDibeli;
                    row.Cells["subtotal"].Value = totalAkanDibeli * harga;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dgvTransaksi.Rows.Add(bukuId, judul, harga, jumlahInput, (harga * jumlahInput));
            }

            HitungTotal();
            txtJumlah.Clear();
        }

        private void HitungTotal()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;
                total += Convert.ToInt32(row.Cells["subtotal"].Value);
            }
            txtTotal.Text = total.ToString();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (dgvTransaksi.Rows.Count == 0 || (dgvTransaksi.Rows.Count == 1 && dgvTransaksi.Rows[0].IsNewRow))
            {
                MessageBox.Show("Keranjang masih kosong!");
                return;
            }

            try
            {
                // Membuat format XML string dari DataGridView
                string xmlItems = "<root>";
                foreach (DataGridViewRow row in dgvTransaksi.Rows)
                {
                    if (row.IsNewRow) continue;
                    xmlItems += $"<item bukuId='{row.Cells["bukuId"].Value}' " +
                                $"jumlah='{row.Cells["jumlah"].Value}' " +
                                $"subtotal='{row.Cells["subtotal"].Value}' />";
                }
                xmlItems += "</root>";

                // 3-TIER: Panggil fungsi simpan terpusat di DAL
                dal.SimpanTransaksi(userId, Convert.ToInt32(txtTotal.Text), "lunas", xmlItems);

                MessageBox.Show("Transaksi Berhasil Disimpan dengan Model 3-Tier!");
                dgvTransaksi.Rows.Clear();
                txtTotal.Text = "0";
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                // Menangkap pesan pembatalan dari TRIGGER INSTEAD OF INSERT di database
                MessageBox.Show("Ditolak oleh Database (Trigger):\n" + sqlEx.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvTransaksi.CurrentRow == null || dgvTransaksi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih item di tabel terlebih dahulu!");
                return;
            }

            if (!int.TryParse(txtJumlah.Text, out int jumlahBaru) || jumlahBaru <= 0)
            {
                MessageBox.Show("Masukkan jumlah baru yang valid (angka positif)!");
                return;
            }

            int bukuId = Convert.ToInt32(dgvTransaksi.CurrentRow.Cells["bukuId"].Value);

            try
            {
                // 3-TIER: Cek stok update lewat fungsi DAL
                int stokTersedia = dal.GetStokBuku(bukuId);

                if (jumlahBaru > stokTersedia)
                {
                    MessageBox.Show($"Stok tidak cukup! Stok tersedia di gudang: {stokTersedia}");
                    return;
                }

                // Update baris di DataGridView jika stok aman
                DataGridViewRow row = dgvTransaksi.CurrentRow;
                int harga = Convert.ToInt32(row.Cells["harga"].Value);

                row.Cells["jumlah"].Value = jumlahBaru;
                row.Cells["subtotal"].Value = harga * jumlahBaru;

                HitungTotal();
                MessageBox.Show("Data di keranjang berhasil diperbarui!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memperbarui item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTransaksi.CurrentRow == null || dgvTransaksi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih item yang ingin dihapus terlebih dahulu!");
                return;
            }

            string judul = dgvTransaksi.CurrentRow.Cells["judul"].Value.ToString();
            DialogResult konfirmasi = MessageBox.Show(
                $"Hapus \"{judul}\" dari keranjang?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (konfirmasi == DialogResult.Yes)
            {
                dgvTransaksi.Rows.Remove(dgvTransaksi.CurrentRow);
                HitungTotal();
            }
        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            FormKelolaBuku fKelola = new FormKelolaBuku();
            fKelola.Show();
            this.Hide();
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FormLaporan fLaporan = new FormLaporan();
            fLaporan.Show();
            this.Hide();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Form login = Application.OpenForms["FormLogin"];
            if (login != null)
            {
                login.Show();
                login.Controls["txtUsername"].Text = "";
                login.Controls["txtPassword"].Text = "";
            }
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void dgvTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}