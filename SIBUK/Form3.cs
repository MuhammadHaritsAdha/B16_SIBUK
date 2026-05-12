using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormKelolaBuku : Form
    {
        string connString = "Data Source=RITS;Initial Catalog=TokoBukuDB;Integrated Security=True";
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
                pesan += "Pengarang tidak boleh kosong.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPengarang.Text.Trim(), @"^[a-zA-Z\s.,']+$"))
            {
                pesan += "Pengarang hanya boleh berisi huruf dan spasi.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }
            else if (txtPengarang.Text.Trim().Length > 30)
            {
                pesan += "Pengarang maksimal 30 karakter.\n";
                SetBorder(txtPengarang, true);
                valid = false;
            }

            // 3. Penerbit — wajib, max 50 karakter
            if (txtPenerbit.Text.Trim().Length < 2)
            {
                pesan += "Nama penerbit terlalu pendek.\n";
                SetBorder(txtPenerbit, true);
                valid = false;
            }
            else if (txtPenerbit.Text.Trim().Length > 50)
            {
                pesan += "Penerbit maksimal 50 karakter.\n";
                SetBorder(txtPenerbit, true);
                valid = false;
            }

            // 4. Harga — wajib, angka positif
            if (string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                pesan += "Harga tidak boleh kosong.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (!int.TryParse(txtHarga.Text, out int harga))
            {
                pesan += "Harga harus berupa angka.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (harga <= 0)
            {
                pesan += "Harga harus lebih dari 0.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }
            else if (harga > 99_999_999)
            {
                pesan += "Harga maksimal Rp 99.999.999.\n";
                SetBorder(txtHarga, true);
                valid = false;
            }

            // 5. Stok — wajib, angka non-negatif
            if (string.IsNullOrWhiteSpace(txtStok.Text))
            {
                pesan += "Stok tidak boleh kosong.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (!int.TryParse(txtStok.Text, out int stok))
            {
                pesan += "Stok harus berupa angka.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (stok < 0)
            {
                pesan += "Stok tidak boleh negatif.\n";
                SetBorder(txtStok, true);
                valid = false;
            }
            else if (stok > 9999)
            {
                pesan += "Stok maksimal 9999.\n";
                SetBorder(txtStok, true);
                valid = false;
            }

            if (!valid)
                MessageBox.Show(pesan, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return valid;
        }

        // Tandai field error dengan border merah
        private void SetBorder(TextBox txt, bool error)
        {
            txt.BackColor = error ? Color.MistyRose : Color.White;
        }

        // Hapus tanda error saat user mulai mengetik
        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox txt)
                txt.BackColor = Color.White;
        }

        // Hanya izinkan angka di field Harga & Stok
        private void txtAngka_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

      
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    // Mengambil data dari VIEW (vw_BukuPublic) 
                    string query = "SELECT * FROM vw_BukuPublic";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvBuku.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void FormKelolaBuku_Load(object sender, EventArgs e)
        {
            this.bukuTableAdapter.Fill(this.tokoBukuDBDataSet.Buku);
            LoadData();
            dgvBuku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Pasang event TextChanged di semua field
            txtJudul.TextChanged += txt_TextChanged;
            txtPengarang.TextChanged += txt_TextChanged;
            txtPenerbit.TextChanged += txt_TextChanged;
            txtHarga.TextChanged += txt_TextChanged;
            txtStok.TextChanged += txt_TextChanged;

            // Pasang event KeyPress untuk field angka saja
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
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertBuku", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@judul", txtJudul.Text.Trim());
                        cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text.Trim());
                        cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga", Convert.ToInt32(txtHarga.Text));
                        cmd.Parameters.AddWithValue("@stok", Convert.ToInt32(txtStok.Text));

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil ditambahkan!");
                        LoadData();
                        ResetForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal tambah data: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1) return;
            if (!Validasi()) return;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", selectedId);
                    cmd.Parameters.AddWithValue("@judul", txtJudul.Text.Trim());
                    cmd.Parameters.AddWithValue("@pengarang", txtPengarang.Text.Trim());
                    cmd.Parameters.AddWithValue("@penerbit", txtPenerbit.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", Convert.ToInt32(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@stok", Convert.ToInt32(txtStok.Text));

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil diupdate!");
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedId == -1) return;

            if (MessageBox.Show("Yakin hapus?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteBuku", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = selectedId;

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Data berhasil dihapus");

                        LoadData();
                        ResetForm();
                    }
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

            // Reset warna background
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
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Gunakan UPDATE JOIN agar tidak melanggar Foreign Key
                    // Mengembalikan nilai judul dan pengarang dari tabel backup ke tabel utama
                    string query = @"
                IF OBJECT_ID('dbo.Buku_Backup') IS NOT NULL
                BEGIN
                    UPDATE b
                    SET b.judul = bk.judul,
                        b.pengarang = bk.pengarang,
                        b.penerbit = bk.penerbit,
                        b.hargaSatuan = bk.hargaSatuan,
                        b.stok = bk.stok
                    FROM dbo.Buku b
                    INNER JOIN dbo.Buku_Backup bk ON b.bukuId = bk.bukuId;
                END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil dipulihkan dari Backup!", "Recovery Berhasil");

                // Bersihkan input dan refresh VIEW
                txtJudul.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }
        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = "UPDATE Buku SET judul = 'HACKED', pengarang = 'HACKED' WHERE judul = '" + txtJudul.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected + " data berhasil dimanipulasi (HACKED)!", "Status Injeksi");
                    }
                }

                // Memuat ulang data dari VIEW untuk melihat hasilnya di Grid
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error simulasi: " + ex.Message);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            // Cek apakah FormTransaksi sudah terbuka di background
            Form frm = Application.OpenForms["FormTransaksi"];

            if (frm != null)
            {
                frm.Show(); // Munculkan kembali form transaksi yang tadi di-hide
            }
            else
            {
                // Jika karena suatu hal form transaksi tidak ada, buat baru
                FormTransaksi f = new FormTransaksi("admin", 1); // sesuaikan role/id
                f.Show();
            }

            this.Close(); // Tutup form saat ini (Kelola/Laporan) agar memori bersih
        }
    }
}