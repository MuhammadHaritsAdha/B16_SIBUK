using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SIBUK
{
    public partial class Report : Form
    {

        CrystalReportBuku cryRpt = new CrystalReportBuku();
        DataTable _dtLaporan;

        string TotalPenjualan { get; set; }

        public Report(DataTable dt, string totalPenjualan)
        {
            InitializeComponent();
            _dtLaporan = dt;
            TotalPenjualan = totalPenjualan;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                // Pastikan di desainer Report.cs, nama komponen CrystalReportViewer adalah crystalReportViewer1
                crystalReportViewer1.Dock = DockStyle.Fill;

                // 3-TIER AMAN: Data '_dtLaporan' sudah merupakan data matang hasil operan dari DAL via FormLaporan
                // Tinggal kita ikat (bind) ke dalam engine Crystal Report
                cryRpt.SetDataSource(_dtLaporan);

                // Tampilkan ke komponen viewer di layar UI
                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat cetakan ke Viewer: " + ex.Message, "Error Cetak", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}