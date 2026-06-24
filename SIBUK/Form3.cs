using ExcelDataReader;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormKelolaBuku : Form
    {
        // 3-TIER: Menggunakan satu objek DAL (dbLogic) untuk semua komunikasi database
        DAL dbLogic = new DAL();
        int selectedId = -1;

        public FormKelolaBuku()
        {
            InitializeComponent();
        }

        private bool Validasi()
        {
            // Reset semua border dulu
            SetBorder(txtJudul, false);
            SetBorder(txtPengarang, false);
            SetBorder(txtPenerbit, false);
            SetBorder(txtHarga, false);
            SetBorder(txtStok, false);

            bool valid = true;
            string pesan = "";

            // 1. Judul — wajib, max 50 karakter
            if (string.IsNullOrWhiteSpace(txtJudul.Text))
            {
                pesan += "- Judul tidak boleh kosong.\n";
                SetBorder(txtJudul, true);
                valid = false;
            }
            else if (txtJudul.Text.Trim().Length < 2)
            {
                pesan += "- Judul terlalu pendek (Minimal 2 karakter).\n";
                SetBorder(txtJudul, true);
                valid = false;
            }
            else if (txtJudul.Text.Trim().Length > 50)
            {
                pesan += "- Judul terlalu panjang (Maksimal 50 karakter).\n";
                SetBorder(txtJudul, true);
                valid = false;
            }

            // 2. Pengarang — wajib, hanya huruf & spasi, max 30 karakter
            if (string.IsNullOrWhiteSpace(txtPengarang.Text))
            {
                pesan += "- Pengarang tidak boleh kosong.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPengarang.Text.Trim(), @"^[a-zA-Z\s.,']+$"))
            {
                pesan += "- Pengarang hanya boleh berisi huruf dan spasi.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }
            else if (txtPengarang.Text.Trim().Length > 30)
            {
                pesan += "- Pengarang maksimal 30 karakter.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }

            // 3. Penerbit — wajib, max 50 karakter
            if (txtPenerbit.Text.Trim().Length < 2)
            {
                pesan += "- Nama penerbit terlalu pendek.\n";
                SetBorder(txtPenerbit, true);
                valid = false;
            }
            else if (txtPenerbit.Text.Trim().Length > 50)
            {
                pesan += "- Penerbit maksimal 50 karakter.\n";
                SetBorder(txtPenerbit, true);
                valid = false;
            }

            // 4. Harga — wajib, angka positif
            if (string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                pesan += "- Harga tidak boleh kosong.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (!int.TryParse(txtHarga.Text, out int harga))
            {
                pesan += "- Harga harus berupa angka.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (harga <= 0)
            {
                pesan += "- Harga harus lebih dari 0.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (harga > 99_999_999)
            {
                pesan += "- Harga maksimal Rp 99.999.999.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }

            // 5. Stok — wajib, angka non-negatif
            if (string.IsNullOrWhiteSpace(txtStok.Text))
            {
                pesan += "- Stok tidak boleh kosong.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (!int.TryParse(txtStok.Text, out int stok))
            {
                pesan += "- Stok harus berupa angka.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (stok < 0)
            {
                pesan += "- Stok tidak boleh negatif.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (stok > 9999)
            {
                pesan += "- Stok maksimal 9999.\n";
                SetBorder(txtStok, true);
                valid = false;
            }

            if (!valid)
                MessageBox.Show(pesan, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return valid;
        }

        private void SetBorder(TextBox txt, bool error)
        {
            txt.BackColor = error ? Color.MistyRose : Color.White;
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox txt)
                txt.BackColor = Color.White;
        }

        private void txtAngka_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void LoadData()
        {
            try
            {
                // 3-TIER: Mengambil data view melalui fungsi terpusat DAL
                dgvBuku.DataSource = dbLogic.GetBukuPublic();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data dari database: " + ex.Message, "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormKelolaBuku_Load(object sender, EventArgs e)
        {
            // Menghindari crash local dataset bawaan C# wizard jika koneksi 3-Tier diubah
            try { this.bukuTableAdapter.Fill(this.tokoBukuDBDataSet.Buku); } catch { }

            LoadData();
            dgvBuku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtJudul.TextChanged += txt_TextChanged;
            txtPengarang.TextChanged += txt_TextChanged;
            txtPenerbit.TextChanged += txt_TextChanged;
            txtHarga.TextChanged += txt_TextChanged;
            txtStok.TextChanged += txt_TextChanged;

            txtHarga.KeyPress += txtAngka_KeyPress;
            txtStok.KeyPress += txtAngka_KeyPress;
        }

        private void dgvBuku_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBuku.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(row.Cells["bukuId"].Value);
                txtJudul.Text = row.Cells["judul"].Value.ToString();
                txtPengarang.Text = row.Cells["pengarang"].Value.ToString();
                txtPenerbit.Text = row.Cells["penerbit"].Value.ToString();
                txtHarga.Text = row.Cells["hargaSatuan"].Value.ToString();
                txtStok.Text = row.Cells["stok"].Value.ToString();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!Validasi()) return;

            try
            {
                // 3-TIER: Mengirim input form ke fungsi simpan DAL
                dbLogic.InsertBuku(
                    txtJudul.Text.Trim(),
                    txtPengarang.Text.Trim(),
                    txtPenerbit.Text.Trim(),
                    Convert.ToInt32(txtHarga.Text),
                    Convert.ToInt32(txtStok.Text)
                );

                MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal tambah data via DAL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih data pada tabel terlebih dahulu!", "Peringatan");
                return;
            }
            if (!Validasi()) return;

            try
            {
                // 3-TIER: Mengirim pembaruan data ke fungsi update DAL
                dbLogic.UpdateBuku(
                    selectedId,
                    txtJudul.Text.Trim(),
                    txtPengarang.Text.Trim(),
                    txtPenerbit.Text.Trim(),
                    Convert.ToInt32(txtHarga.Text),
                    Convert.ToInt32(txtStok.Text)
                );

                MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal update data via DAL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih data pada tabel yang ingin dihapus!", "Peringatan");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus data buku ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // 3-TIER: Menjalankan penghapusan melalui fungsi hapus DAL
                    int rowsAffected = dbLogic.DeleteBuku(selectedId);

                    if (rowsAffected > 0)
                        MessageBox.Show("Data berhasil dihapus dari sistem!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus data via DAL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetForm()
        {
            txtJudul.Clear();
            txtPengarang.Clear();
            txtPenerbit.Clear();
            txtHarga.Clear();
            txtStok.Clear();

            SetBorder(txtJudul, false);
            SetBorder(txtPengarang, false);
            SetBorder(txtPenerbit, false);
            SetBorder(txtHarga, false);
            SetBorder(txtStok, false);

            selectedId = -1;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                // 3-TIER: Menjalankan pemulihan data dari backup melalui fungsi DAL
                dbLogic.RecoveryBukuFromBackup();

                MessageBox.Show("Data berhasil dipulihkan dari Backup!", "Recovery Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset recovery gagal: " + ex.Message, "Error Recovery", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                // 3-TIER: Simulasi celah injeksi dilempar terstruktur melalui DAL
                int rowsAffected = dbLogic.ExecuteVulnerableInjection(txtJudul.Text);

                MessageBox.Show(rowsAffected + " data berhasil dimanipulasi (HACKED)!", "Status Injeksi SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error simulasi celah keamanan: " + ex.Message);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["FormTransaksi"];
            if (frm != null)
            {
                frm.Show();
            }
            else
            {
                FormTransaksi f = new FormTransaksi("admin", 1);
                f.Show();
            }
            this.Close();
        }

        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx;*.xls";
            openFileDialog.Title = "Pilih File Excel untuk Review Data";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            DataTable dtExcel = result.Tables[0];
                            dgvBuku.DataSource = dtExcel;
                            btnImpDB.Enabled = true;

                            MessageBox.Show("Data Excel berhasil dimuat! Silahkan tinjau kembali data pada tabel sebelum diimport.",
                                            "Review Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memproses file Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnImpDB_Click(object sender, EventArgs e)
        {
            if (dgvBuku.DataSource == null || dgvBuku.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data review yang bisa disimpan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int barisSukses = 0;
            int barisGagal = 0;
            string pesanError = "";

            DataTable dtReview = (DataTable)dgvBuku.DataSource;

            try
            {
                foreach (DataRow row in dtReview.Rows)
                {
                    if (row[0] == DBNull.Value || string.IsNullOrEmpty(row[0].ToString())) continue;

                    try
                    {
                        string judul = row["judul"].ToString();
                        string pengarang = row["pengarang"].ToString();
                        string penerbit = row["penerbit"].ToString();
                        int harga = Convert.ToInt32(row["hargaSatuan"]);
                        int stok = Convert.ToInt32(row["stok"]);

                        // 3-TIER: Menembak fungsi excel DAL bawaan modulmu
                        dbLogic.InsertBukuFromExcel(judul, pengarang, penerbit, harga, stok);
                        barisSukses++;
                    }
                    catch (Exception ex)
                    {
                        barisGagal++;
                        pesanError += $"\n- Judul '{row[0]}': {ex.Message}";
                    }
                }

                string statusHasil = $"Proses Sinkronisasi Selesai!\n• Berhasil Tersimpan: {barisSukses} data\n• Gagal/Ditolak SP: {barisGagal} data";
                if (barisGagal > 0)
                {
                    statusHasil += $"\n\nDetail Error dari Database:{pesanError}";
                }
                MessageBox.Show(statusHasil, "Informasi Sinkronisasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnImpDB.Enabled = false;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi masalah pada sistem utama: " + ex.Message, "Sistem Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}