using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SIBUK
{
    public partial class FormTransaksi : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader reader;
        string connString = "Data Source=RITS;Initial Catalog=TokoBukuDB;Integrated Security=True";
        string roleUser;
        int userId;
        public FormTransaksi(string role, int id)
        {
            InitializeComponent();
            roleUser = role;
            userId = id;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            if (roleUser != "admin")
                btnLaporan.Visible = false;

            if (dgvTransaksi.Columns.Count == 0)
            {
                dgvTransaksi.Columns.Add("bukuId", "ID Buku");
                dgvTransaksi.Columns.Add("judul", "Judul Buku");
                dgvTransaksi.Columns.Add("harga", "Harga");
                dgvTransaksi.Columns.Add("jumlah", "Qty");
                dgvTransaksi.Columns.Add("subtotal", "Subtotal");
            }
            txtTotal.ReadOnly = true;
            txtTotal.Text = "0";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBukuSimple", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                string query = "SELECT bukuId, judul, hargaSatuan FROM Buku";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                cbBuku.DataSource = dt;
                cbBuku.DisplayMember = "judul";
                cbBuku.ValueMember = "bukuId";
            }

            txtHarga.ReadOnly = true;

            dgvTransaksi.Columns.Clear();

            dgvTransaksi.Columns.Add("bukuId", "ID");
            dgvTransaksi.Columns.Add("judul", "Judul");
            dgvTransaksi.Columns.Add("harga", "Harga");
            dgvTransaksi.Columns.Add("jumlah", "Jumlah");
            dgvTransaksi.Columns.Add("subtotal", "Subtotal");

            dgvTransaksi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbBuku_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBuku.SelectedItem is DataRowView row)
            {
                txtHarga.Text = row["hargaSatuan"].ToString();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtJumlah.Text, out int jumlah))
            {
                MessageBox.Show("Jumlah harus angka!");
                return;
            }

            int bukuId = Convert.ToInt32(cbBuku.SelectedValue);

            // 2. HITUNG AKUMULASI: Cek berapa jumlah buku ini yang sudah ada di grid
            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToInt32(row.Cells["bukuId"].Value) == bukuId)
                {
                    jumlahDiKeranjang = Convert.ToInt32(row.Cells["jumlah"].Value);
                    break;
                }
            }

            int totalAkanDibeli = jumlahInput + jumlahDiKeranjang;

            // 3. CEK STOk (Bandingkan Stok Gudang vs Total Akumulasi)
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // CEK STOK
                string qCek = "SELECT stok FROM Buku WHERE bukuId = @b";
                SqlCommand cmd = new SqlCommand(qCek, conn);
                cmd.Parameters.AddWithValue("@b", bukuId);

                int stok = (int)cmd.ExecuteScalar();

                if (jumlah > stok)
                {
                    MessageBox.Show("Stok tidak cukup!");
                    return;
                }
            }

            // kalau lolos → baru masuk grid
            string judul = cbBuku.Text;
            int harga = Convert.ToInt32(txtHarga.Text);
            int subtotal = harga * jumlah;

            bool found = false;

            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue;

                if (Convert.ToInt32(row.Cells["bukuId"].Value) == bukuId)
                {
                    int jumlahLama = Convert.ToInt32(row.Cells["jumlah"].Value);
                    int jumlahBaru = jumlahLama + jumlah;

                    row.Cells["jumlah"].Value = jumlahBaru;
                    row.Cells["subtotal"].Value = jumlahBaru * harga;

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dgvTransaksi.Rows.Add(bukuId, judul, harga, jumlah, subtotal);
            }

            HitungTotal();
        }
        private void HitungTotal()
        {
            int total = 0;

            foreach (DataGridViewRow row in dgvTransaksi.Rows)
            {
                if (row.IsNewRow) continue; // penting!
                total += Convert.ToInt32(row.Cells["subtotal"].Value);
            }

            txtTotal.Text = total.ToString();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (dgvTransaksi.Rows.Count == 0)
            {
                MessageBox.Show("Belum ada item!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Konversi DataGridView ke format XML string untuk dikirim ke SP
                string xmlItems = "<root>";
                foreach (DataGridViewRow row in dgvTransaksi.Rows)
                {
                    if (row.IsNewRow) continue;
                    xmlItems += $"<item bukuId='{row.Cells["bukuId"].Value}' " +
                                $"jumlah='{row.Cells["jumlah"].Value}' " +
                                $"subtotal='{row.Cells["subtotal"].Value}' />";
                }
                xmlItems += "</root>";

                using (SqlTransaction trx = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) insert header
                        string q = @"INSERT INTO Transaksi (tanggal, userId, totalHarga, statusBayar)
                             OUTPUT INSERTED.transaksiId
                             VALUES (@tgl, @uid, @total, @status)";
                        SqlCommand cmd = new SqlCommand(q, conn, trx);
                        cmd.Parameters.AddWithValue("@tgl", DateTime.Now);
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@total", Convert.ToInt32(txtTotal.Text));
                        cmd.Parameters.AddWithValue("@status", "lunas");

                        int transaksiId = (int)cmd.ExecuteScalar();

                        // 2) insert detail
                        foreach (DataGridViewRow row in dgvTransaksi.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int bukuId = Convert.ToInt32(row.Cells["bukuId"].Value);
                            int jumlah = Convert.ToInt32(row.Cells["jumlah"].Value);

                            // 1. CEK STOK DULU
                            string qCek = "SELECT stok FROM Buku WHERE bukuId = @b";
                            SqlCommand cmdCek = new SqlCommand(qCek, conn, trx);
                            cmdCek.Parameters.AddWithValue("@b", bukuId);

                            int stok = (int)cmdCek.ExecuteScalar();

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // 1. Validasi baris yang dipilih
            if (dgvTransaksi.CurrentRow == null || dgvTransaksi.CurrentRow.IsNewRow)
                            {
                                MessageBox.Show("Stok tidak cukup!");
                                trx.Rollback();
                                return;
                            }

                            // 2. INSERT DETAIL
                            string qd = "INSERT INTO Detail_Transaksi (transaksiId, bukuId, jumlah, subTotal) VALUES (@t, @b, @j, @s)";
                            SqlCommand cd = new SqlCommand(qd, conn, trx);

                            cd.Parameters.AddWithValue("@t", transaksiId);
                            cd.Parameters.AddWithValue("@b", bukuId);
                            cd.Parameters.AddWithValue("@j", jumlah);
                            cd.Parameters.AddWithValue("@s", row.Cells["subtotal"].Value);

                            cd.ExecuteNonQuery();

                            // 3. UPDATE STOK
                            string qUpdate = "UPDATE Buku SET stok = stok - @j WHERE bukuId = @b";
                            SqlCommand cmdUpdate = new SqlCommand(qUpdate, conn, trx);

                            cmdUpdate.Parameters.AddWithValue("@j", jumlah);
                            cmdUpdate.Parameters.AddWithValue("@b", bukuId);

                            cmdUpdate.ExecuteNonQuery();
                        }

                        trx.Commit();
                        MessageBox.Show("Transaksi berhasil disimpan!");

                        dgvTransaksi.Rows.Clear();
                        txtTotal.Clear();
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnKelola_Click(object sender, EventArgs e)
        {
            FormKelolaBuku f = new FormKelolaBuku();
            f.ShowDialog();
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FormLaporan f = new FormLaporan();
            f.ShowDialog();
        }
    }
}