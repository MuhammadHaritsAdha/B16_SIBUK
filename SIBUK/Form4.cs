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

            txtTotal.ReadOnly = true;
            txtJumlah.ReadOnly = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvLaporan.DataSource = null;
            txtTotal.Clear();
            txtJumlah.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            decimal totalLaporan = 0;
            int jumlahBaris = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // QUERY GABUNGAN: Filter Tanggal DAN Judul Buku sekaligus
                    // Menggunakan vw_LaporanDetail agar data yang muncul detail per buku
                    string query = @"SELECT * FROM vw_LaporanDetail 
                             WHERE (tanggal BETWEEN @awal AND @akhir) 
                             AND (judul LIKE @judul)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Ambil nilai dari DateTimePicker dan TextBox
                        cmd.Parameters.AddWithValue("@awal", dtpAwal.Value.Date);
                        cmd.Parameters.AddWithValue("@akhir", dtpAkhir.Value.Date);
                        cmd.Parameters.AddWithValue("@judul", "%" + txtCariBuku.Text.Trim() + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvLaporan.DataSource = dt;

                        //Hitung langsung dari DataTable
                        jumlahBaris = dt.Rows.Count;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["subTotal"] != DBNull.Value)
                            {
                                totalLaporan += Convert.ToDecimal(row["subTotal"]);
                            }
                        }

                        // Tampilkan hasil
                        txtTotal.Text = totalLaporan.ToString("N0"); // Format ribuan (1,000,000)
                        txtJumlah.Text = jumlahBaris.ToString();

                        if (jumlahBaris == 0)
                        {
                            MessageBox.Show("Data tidak ditemukan untuk kriteria tersebut.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat laporan: " + ex.Message);
                }
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
                FormTransaksi f = new FormTransaksi("admin", 1);
                f.Show();
            }

            this.Close(); // Tutup form saat ini (Kelola/Laporan)
        }
    }
}