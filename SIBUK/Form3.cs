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
