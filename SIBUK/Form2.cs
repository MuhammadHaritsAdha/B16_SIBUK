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
            if (roleUser != "admin")
                btnLaporan.Visible = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT bukuId, judul, hargaSatuan FROM Buku";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                cbBuku.DataSource = dt;
                cbBuku.DisplayMember = "judul";
                cbBuku.ValueMember = "bukuId";
            }

            txtHarga.ReadOnly = true;

            dgvTransaksi.Columns.Clear();

            dgvTransaksi.Columns.Add("bukuId", "ID");
            dgvTransaksi.Columns.Add("judul", "Judul");
            dgvTransaksi.Columns.Add("harga", "Harga");
            dgvTransaksi.Columns.Add("jumlah", "Jumlah");
            dgvTransaksi.Columns.Add("subtotal", "Subtotal");

            dgvTransaksi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtJumlah.Text, out int jumlah))
            {
                MessageBox.Show("Jumlah harus angka!");
                return;
            }

            int bukuId = Convert.ToInt32(cbBuku.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // CEK STOK
                string qCek = "SELECT stok FROM Buku WHERE bukuId = @b";
                SqlCommand cmd = new SqlCommand(qCek, conn);
                cmd.Parameters.AddWithValue("@b", bukuId);

                int stok = (int)cmd.ExecuteScalar();

                if (jumlah > stok)
                {
                    MessageBox.Show("Stok tidak cukup!");
                    return;
                }
            }

            // kalau lolos → baru masuk grid
            string judul = cbBuku.Text;
            int harga = Convert.ToInt32(txtHarga.Text);
            int subtotal = harga * jumlah;

            bool found = false;

            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;

                if (Convert.ToInt32(row.Cells["bukuId"].Value) == bukuId)
                {
                    int jumlahLama = Convert.ToInt32(row.Cells["jumlah"].Value);
                    int jumlahBaru = jumlahLama + jumlah;

                    row.Cells["jumlah"].Value = jumlahBaru;
                    row.Cells["subtotal"].Value = jumlahBaru * harga;

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dgvTransaksi.Rows.Add(bukuId, judul, harga, jumlah, subtotal);
            }

            HitungTotal();
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
