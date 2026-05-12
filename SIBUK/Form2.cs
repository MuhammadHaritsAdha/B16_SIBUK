using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormTransaksi : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader reader;
        string connString = "Data Source=RITS;Initial Catalog=TokoBukuDB;Integrated Security=True";
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

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBukuSimple", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    cbBuku.DataSource = dt;
                    cbBuku.DisplayMember = "judul";
                    cbBuku.ValueMember = "bukuId";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

            // 2. HITUNG AKUMULASI: Cek berapa jumlah buku ini yang sudah ada di grid
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

            // 3. CEK STOk (Bandingkan Stok Gudang vs Total Akumulasi)
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string qCek = "SELECT stok FROM vw_StokBuku WHERE bukuId = @b";
                using (SqlCommand cmd = new SqlCommand(qCek, conn))
                {
                    cmd.Parameters.AddWithValue("@b", bukuId);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int stokGudang = Convert.ToInt32(result);

                        if (totalAkanDibeli > stokGudang)
                        {
                            MessageBox.Show($"Stok tidak mencukupi!\n" +
                                            $"Di Gudang: {stokGudang}\n" +
                                            $"Sudah di Keranjang: {jumlahDiKeranjang}\n" +
                                            $"Maksimal yang bisa ditambah: {stokGudang - jumlahDiKeranjang}");
                            return;
                        }
                    }
                }
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
                if (row.IsNewRow) continue; // penting!
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
                // Konversi DataGridView ke format XML string untuk dikirim ke SP
                string xmlItems = "<root>";
                foreach (DataGridViewRow row in dgvTransaksi.Rows)
                {
                    if (row.IsNewRow) continue;
                    xmlItems += $"<item bukuId='{row.Cells["bukuId"].Value}' " +
                                $"jumlah='{row.Cells["jumlah"].Value}' " +
                                $"subtotal='{row.Cells["subtotal"].Value}' />";
                }
                xmlItems += "</root>";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SimpanTransaksi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@total", Convert.ToInt32(txtTotal.Text));
                        cmd.Parameters.AddWithValue("@status", "lunas");
                        cmd.Parameters.AddWithValue("@items", xmlItems);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Transaksi Berhasil Disimpan!");
                        dgvTransaksi.Rows.Clear();
                        txtTotal.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message);
            }
        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            FormKelolaBuku fKelola = new FormKelolaBuku();
            fKelola.Show();
            this.Hide(); // Sembunyikan transaksi agar tidak bertumpuk
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FormLaporan fLaporan = new FormLaporan();
            fLaporan.Show();
            this.Hide();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // 1. Validasi baris yang dipilih
            if (dgvTransaksi.CurrentRow == null || dgvTransaksi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih item di tabel terlebih dahulu!");
                return;
            }

            // 2. Inisialisasi jumlahBaru dari input user
            if (!int.TryParse(txtJumlah.Text, out int jumlahBaru) || jumlahBaru <= 0)
            {
                MessageBox.Show("Masukkan jumlah baru yang valid (angka positif)!");
                return;
            }

            int bukuId = Convert.ToInt32(dgvTransaksi.CurrentRow.Cells["bukuId"].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // 3. Cek stok ke VIEW
                string qCek = "SELECT stok FROM vw_StokBuku WHERE bukuId = @b";
                using (SqlCommand cmd = new SqlCommand(qCek, conn))
                {
                    cmd.Parameters.AddWithValue("@b", bukuId);
                    int stokTersedia = Convert.ToInt32(cmd.ExecuteScalar());

                    if (jumlahBaru > stokTersedia)
                    {
                        MessageBox.Show($"Stok tidak cukup! Stok tersedia di gudang: {stokTersedia}");
                        return;
                    }
                }
            }

            // 4. Update baris di DataGridView
            DataGridViewRow row = dgvTransaksi.CurrentRow;
            int harga = Convert.ToInt32(row.Cells["harga"].Value);

            row.Cells["jumlah"].Value = jumlahBaru;
            row.Cells["subtotal"].Value = harga * jumlahBaru;

            HitungTotal();
            MessageBox.Show("Data di keranjang berhasil diperbarui!");
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

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            // Cari FormLogin yang tadi di-hide
            Form login = Application.OpenForms["FormLogin"];
            if (login != null)
            {
                login.Show(); // Munculkan kembali
                              // Kosongkan text agar user baru harus ngetik lagi
                login.Controls["txtUsername"].Text = "";
                login.Controls["txtPassword"].Text = "";
            }
            this.Close(); // Tutup form transaksi
        }

    }
}