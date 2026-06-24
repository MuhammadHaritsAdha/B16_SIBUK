using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormLogin : Form
    {
        DAL dal = new DAL();
        string connString;

        public FormLogin()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            connString = DAL.GetConnectionString(); // Mengambil string koneksi terpusat
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validasi input kosong di UI (Presentation Layer)
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. MANGGIL DAL (Model 3-Tier): Mengirim input ke Data Access Layer
                DataRow userRow = dal.CekLogin(txtUsername.Text, txtPassword.Text);

                if (userRow != null)
                {
                    // Ambil data hasil olahan DAL dari baris tabel
                    string role = userRow["role"].ToString();
                    int userId = Convert.ToInt32(userRow["userId"]);

                    MessageBox.Show("Login berhasil sebagai " + role + "!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Buka form utama transaksi dengan parameter role & userId
                    FormTransaksi f = new FormTransaksi(role, userId);
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau Password Salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Sistem Login: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            CekKoneksi();
        }

        private void CekKoneksi()
        {
            // Pengecekan status koneksi awal aplikasi
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {
                    conn.Open();
                    lblKoneksi.Text = "Terhubung ke Database";
                    lblKoneksi.ForeColor = Color.Green;
                }
            }
            catch
            {
                lblKoneksi.Text = "Tidak Terhubung ke Database";
                lblKoneksi.ForeColor = Color.Red;
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void lblKoneksi_Click(object sender, EventArgs e)
        {
        }
    }
}