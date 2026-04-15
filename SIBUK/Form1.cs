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
