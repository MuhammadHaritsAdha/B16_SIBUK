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
