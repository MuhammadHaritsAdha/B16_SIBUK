using System;
using System.Data;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormLaporan : Form
    {
        // 3-TIER: Menggunakan objek objek DAL (dbLogic) untuk interaksi database
        DAL dbLogic = new DAL();

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

            try
            {
                // Memanggil fungsi data melalui class DAL
                DataTable dt = dbLogic.GetLaporanDetail(
                    dtpAwal.Value.Date,
                    dtpAkhir.Value.Date,
                    txtCariBuku.Text.Trim()
                );

                // Pasang data ke GridView
                dgvLaporan.DataSource = dt;

                jumlahBaris = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    // DI SINI PENYEBAB ERRORNYA! 
                    // Pastikan kodenya sudah diganti menjadi "hargaSatuan" bukan "subTotal"
                    if (row["hargaSatuan"] != DBNull.Value)
                    {
                        totalLaporan += Convert.ToDecimal(row["hargaSatuan"]);
                    }
                }

                // Tampilkan hasil ke UI TextBox setelah loop sukses tanpa error
                txtTotal.Text = totalLaporan.ToString("N0");
                txtJumlah.Text = jumlahBaris.ToString();

                if (jumlahBaris == 0)
                {
                    MessageBox.Show("Data tidak ditemukan untuk kriteria tersebut.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Pesan error yang muncul di gambar berasal dari blok catch ini
                MessageBox.Show("Gagal memuat laporan via DAL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnCetak_Click(object sender, EventArgs e)
        {
            // 1. Validasi: Jika DataGridView masih kosong atau belum melakukan pencarian (Cari), batalkan proses cetak
            if (dgvLaporan.DataSource == null || dgvLaporan.Rows.Count == 0)
            {
                MessageBox.Show("Silakan klik tombol 'Cari' terlebih dahulu untuk memastikan data tersedia sebelum dicetak.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 3-TIER: Memanggil Stored Procedure crystal report melalui fungsi terpusat DAL
                DataTable dt = dbLogic.GetLaporanCrystal(
                    dtpAwal.Value.Date,
                    dtpAkhir.Value.Date,
                    txtCariBuku.Text.Trim()
                );

                // Cek lagi apakah setelah di-fill datanya benar-benar ada
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data tidak ditemukan untuk dicetak.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Panggil Form Baru bernama 'Report' dan kirim datanya lewat Constructor
                Report frmCetak = new Report(dt, txtTotal.Text);

                // Menampilkan form Report sebagai Pop-Up Jendela Terfokus
                frmCetak.ShowDialog();

                // Dispose setelah ditutup agar memori bersih dan tidak menahan cache
                frmCetak.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses cetakan via DAL: " + ex.Message, "Error Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvLaporan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}