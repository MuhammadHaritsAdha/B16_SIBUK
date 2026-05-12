using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIBUK
{
    public partial class FormLogin : Form
    {
        SqlCommand cmd;
        SqlConnection conn;
        string connString = "Data Source=RITS;Initial Catalog=TokoBukuDB;Integrated Security=True";
        public FormLogin()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // MEMANGGIL VIEW: Query diarahkan ke vw_UserLogin, bukan tabel Users
                    string query = "SELECT * FROM vw_UserLogin WHERE username=@u AND password=@p";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@p", txtPassword.Text);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["role"].ToString();
                                int userId = Convert.ToInt32(reader["userId"]);

                                MessageBox.Show("Login berhasil sebagai " + role + "!");

                                FormTransaksi f = new FormTransaksi(role, userId);
                                f.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Username atau Password Salah!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Login: " + ex.Message);
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            CekKoneksi();
        }

        private void CekKoneksi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
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
            // Jika user menutup form login secara manual, matikan seluruh aplikasi
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}