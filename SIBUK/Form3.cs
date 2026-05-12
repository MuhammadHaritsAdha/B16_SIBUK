using System;
using System.Data;
using System.Data.SqlClient;
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
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM Buku";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBuku.DataSource = dt;
            }
        }
        private void FormKelolaBuku_Load(object sender, EventArgs e)
        {
            LoadData();

            dgvBuku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                //  1. CEK DULU
                string cek = "SELECT COUNT(*) FROM Buku WHERE judul=@j AND pengarang=@p";
                SqlCommand cekCmd = new SqlCommand(cek, conn);

                cekCmd.Parameters.AddWithValue("@j", txtJudul.Text);
                cekCmd.Parameters.AddWithValue("@p", txtPengarang.Text);

                int count = (int)cekCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Data buku sudah ada! Gunakan Update.");
                    return;
                }

                // 2. BARU INSERT
                string query = "INSERT INTO Buku (judul, pengarang, penerbit, hargaSatuan, stok) VALUES (@j, @p, @pn, @h, @s)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@j", txtJudul.Text);
                cmd.Parameters.AddWithValue("@p", txtPengarang.Text);
                cmd.Parameters.AddWithValue("@pn", txtPenerbit.Text);
                cmd.Parameters.AddWithValue("@h", Convert.ToInt32(txtHarga.Text));
                cmd.Parameters.AddWithValue("@s", Convert.ToInt32(txtStok.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil ditambahkan!");

                LoadData();
                ResetForm();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih data dulu!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "UPDATE Buku SET judul=@j, pengarang=@p, penerbit=@pn, hargaSatuan=@h, stok=@s WHERE bukuId=@id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@j", txtJudul.Text);
                cmd.Parameters.AddWithValue("@p", txtPengarang.Text);
                cmd.Parameters.AddWithValue("@pn", txtPenerbit.Text);
                cmd.Parameters.AddWithValue("@h", Convert.ToInt32(txtHarga.Text));
                cmd.Parameters.AddWithValue("@s", Convert.ToInt32(txtStok.Text));
                cmd.Parameters.AddWithValue("@id", selectedId);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil diupdate!");

                LoadData();
                ResetForm();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Pilih data dulu!");
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = "DELETE FROM Buku WHERE bukuId=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@id", selectedId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil dihapus!");

                    LoadData();
                    ResetForm();
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

            selectedId = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}
