using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIBUK
{
    internal class DAL
    {
        private SqlConnection conn;
        private string connString;

        public DAL()
        {
            connString = GetConnectionString();
            conn = new SqlConnection(connString);
        }

        // Ambil IP server dari App.config, bukan dari komputer yang jalanin app
        public static string GetServerIP()
        {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];
            return serverIP;
        }

        public static string GetConnectionString()
        {
            string connectionString = $"Data Source={GetServerIP()};Initial Catalog=TokoBukuDB;User ID=sa;Password=Harits19;";
            return connectionString;
        }

        public void InsertBukuFromExcel(string judul, string pengarang, string penerbit, int harga, int stok)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                using (SqlCommand command = new SqlCommand("sp_InsertBuku", conn, trans))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@judul", judul);
                    command.Parameters.AddWithValue("@pengarang", pengarang);
                    command.Parameters.AddWithValue("@penerbit", penerbit);
                    command.Parameters.AddWithValue("@harga", harga);
                    command.Parameters.AddWithValue("@stok", stok);

                    command.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataRow CekLogin(string username, string password)
        {
            DataTable dt = new DataTable();
            // Menggunakan connString yang sudah ada di level class DAL
            string query = "SELECT userId, role FROM vw_UserLogin WHERE username = @u AND password = @p";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            // Jika ada data yang cocok, kembalikan baris pertama
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }

            return null; // Jika username atau password salah
        }

        // FUNGSI : Mengambil daftar buku untuk ComboBox di awal load form
        public DataTable GetBukuSimple()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBukuSimple", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // FUNGSI : Mengambil angka stok buku dari View (Digunakan di Tombol Tambah dan Update)
        public int GetStokBuku(int bukuId)
        {
            int stok = 0;
            string query = "SELECT stok FROM vw_StokBuku WHERE bukuId = @b";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@b", bukuId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        stok = Convert.ToInt32(result);
                    }
                }
            }
            return stok;
        }

        // FUNGSI : Menyimpan data transaksi besar dengan parameter format XML
        public void SimpanTransaksi(int userId, int total, string status, string xmlItems)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SimpanTransaksi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@items", xmlItems);

                    conn.Open();
                    cmd.ExecuteNonQuery(); // Eksekusi ke DB (Memicu Trigger INSTEAD OF INSERT)
                }
            }
        }

        // FUNGSI 1: Load semua data buku dari VIEW (vw_BukuPublic)
        public DataTable GetBukuPublic()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT * FROM vw_BukuPublic";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // FUNGSI 2: Tambah Buku Baru melalui Stored Procedure
        public void InsertBuku(string judul, string pengarang, string penerbit, int harga, int stok)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@judul", judul);
                    cmd.Parameters.AddWithValue("@pengarang", pengarang);
                    cmd.Parameters.AddWithValue("@penerbit", penerbit);
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // FUNGSI 3: Update Buku melalui Stored Procedure
        public void UpdateBuku(int id, string judul, string pengarang, string penerbit, int harga, int stok)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@judul", judul);
                    cmd.Parameters.AddWithValue("@pengarang", pengarang);
                    cmd.Parameters.AddWithValue("@penerbit", penerbit);
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // FUNGSI 4: Hapus Buku melalui Stored Procedure
        public int DeleteBuku(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteBuku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // FUNGSI 5: Pulihkan/Recovery Data Buku dari Tabel Backup
        public void RecoveryBukuFromBackup()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
            IF OBJECT_ID('dbo.Buku_Backup') IS NOT NULL
            BEGIN
                UPDATE b
                SET b.judul = bk.judul,
                    b.pengarang = bk.pengarang,
                    b.penerbit = bk.penerbit,
                    b.hargaSatuan = bk.hargaSatuan,
                    b.stok = bk.stok
                FROM dbo.Buku b
                INNER JOIN dbo.Buku_Backup bk ON b.bukuId = bk.bukuId;
            END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // FUNGSI 6: Simulasi SQL Injection
        public int ExecuteVulnerableInjection(string judulInput)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Buku SET judul = 'HACKED', pengarang = 'HACKED' WHERE judul = '" + judulInput + "'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // FUNGSI : Mengambil data laporan dari VIEW berdasarkan range tanggal dan filter judul buku
        // UPDATE FUNGSI DI DAL.CS
        public DataTable GetLaporanDetail(DateTime awal, DateTime akhir, string judul)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // FIX QUERY: Pastikan di dalam SQL, kolom yang difilter atau dipanggil sesuai dengan View baru
                string query = @"SELECT * FROM vw_LaporanDetail 
                         WHERE (tanggal BETWEEN @awal AND @akhir) 
                         AND (judul LIKE @judul)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@awal", awal);
                    cmd.Parameters.AddWithValue("@akhir", akhir);
                    cmd.Parameters.AddWithValue("@judul", "%" + judul + "%");

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // FUNGSI : Mengambil data cetak laporan melalui Stored Procedure (sp_GetLaporanCrystal)
        public DataTable GetLaporanCrystal(DateTime awal, DateTime akhir, string judul)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetLaporanCrystal", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@awal", awal);
                    cmd.Parameters.AddWithValue("@akhir", akhir);
                    cmd.Parameters.AddWithValue("@judul", judul);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}