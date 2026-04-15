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
