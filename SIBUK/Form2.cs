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
