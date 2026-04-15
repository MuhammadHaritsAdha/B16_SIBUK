using System;
using System.Windows.Forms;
using System.Data.SqlClient;

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

                    string query = "SELECT * FROM Users WHERE username=@u AND password=@p";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader["role"].ToString();
                        int userId = Convert.ToInt32(reader["userId"]);

                        MessageBox.Show("Login berhasil!");

                        FormTransaksi f = new FormTransaksi(role, userId);
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Valid!");
                    }
                }
            }

