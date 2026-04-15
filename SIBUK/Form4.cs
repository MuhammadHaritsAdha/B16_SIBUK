using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormLaporan : Form
    {
        string connString = "Data Source=RITS;Initial Catalog=TokoBukuDB;Integrated Security=True";
        public FormLaporan()
        {
            InitializeComponent();
        }
        private void FormLaporan_Load(object sender, EventArgs e)
        {
            dgvLaporan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnTampil_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM Transaksi WHERE tanggal BETWEEN @awal AND @akhir";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@awal", dtpAwal.Value.Date);
                cmd.Parameters.AddWithValue("@akhir", dtpAkhir.Value.Date);

