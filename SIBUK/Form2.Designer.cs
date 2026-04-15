namespace SIBUK
{
    partial class FormTransaksi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.cbBuku = new System.Windows.Forms.ComboBox();
            this.lblPilih = new System.Windows.Forms.Label();
            this.lblHarga = new System.Windows.Forms.Label();
            this.txtHarga = new System.Windows.Forms.TextBox();
            this.txtJumlah = new System.Windows.Forms.TextBox();
            this.lblJumlah = new System.Windows.Forms.Label();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dgvTransaksi = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnLaporan = new System.Windows.Forms.Button();
            this.btnKelola = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaksi)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(351, 41);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Transaksi";
            // 
            // cbBuku
            // 
            this.cbBuku.FormattingEnabled = true;
            this.cbBuku.Location = new System.Drawing.Point(235, 101);
            this.cbBuku.Name = "cbBuku";
            this.cbBuku.Size = new System.Drawing.Size(200, 33);
            this.cbBuku.TabIndex = 1;
            this.cbBuku.SelectedIndexChanged += new System.EventHandler(this.cbBuku_SelectedIndexChanged);
            // 
            // lblPilih
            // 
            this.lblPilih.AutoSize = true;
            this.lblPilih.Location = new System.Drawing.Point(74, 101);
            this.lblPilih.Name = "lblPilih";
            this.lblPilih.Size = new System.Drawing.Size(108, 25);
            this.lblPilih.TabIndex = 2;
            this.lblPilih.Text = "Pilih Buku";
            this.lblPilih.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblHarga
            // 
            this.lblHarga.AutoSize = true;
            this.lblHarga.Location = new System.Drawing.Point(79, 166);
            this.lblHarga.Name = "lblHarga";
            this.lblHarga.Size = new System.Drawing.Size(70, 25);
            this.lblHarga.TabIndex = 3;
            this.lblHarga.Text = "Harga";
            // 
            // txtHarga
            // 
            this.txtHarga.Location = new System.Drawing.Point(235, 166);
            this.txtHarga.Name = "txtHarga";
            this.txtHarga.Size = new System.Drawing.Size(200, 31);
            this.txtHarga.TabIndex = 4;
            // 
            // txtJumlah
            // 
            this.txtJumlah.Location = new System.Drawing.Point(235, 233);
            this.txtJumlah.Name = "txtJumlah";
            this.txtJumlah.Size = new System.Drawing.Size(200, 31);
            this.txtJumlah.TabIndex = 5;
            // 
            // lblJumlah
            // 
            this.lblJumlah.AutoSize = true;
            this.lblJumlah.Location = new System.Drawing.Point(79, 238);
            this.lblJumlah.Name = "lblJumlah";
            this.lblJumlah.Size = new System.Drawing.Size(81, 25);
            this.lblJumlah.TabIndex = 6;
            this.lblJumlah.Text = "Jumlah";
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(84, 302);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(152, 53);
            this.btnTambah.TabIndex = 7;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvTransaksi
            // 
            this.dgvTransaksi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransaksi.Location = new System.Drawing.Point(79, 397);
            this.dgvTransaksi.Name = "dgvTransaksi";
            this.dgvTransaksi.RowHeadersWidth = 82;
            this.dgvTransaksi.RowTemplate.Height = 33;
            this.dgvTransaksi.Size = new System.Drawing.Size(723, 133);
            this.dgvTransaksi.TabIndex = 8;
            this.dgvTransaksi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransaksi_CellContentClick);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(79, 687);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(60, 25);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "Total";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(235, 681);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(173, 31);
            this.txtTotal.TabIndex = 10;
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(79, 774);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(220, 54);
            this.btnSimpan.TabIndex = 11;
            this.btnSimpan.Text = "Simpan Transaksi";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnLaporan
            // 
            this.btnLaporan.Location = new System.Drawing.Point(356, 774);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(189, 54);
            this.btnLaporan.TabIndex = 12;
            this.btnLaporan.Text = "Laporan";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.btnLaporan_Click);
            // 
            // btnKelola
            // 
            this.btnKelola.Location = new System.Drawing.Point(615, 774);
            this.btnKelola.Name = "btnKelola";
            this.btnKelola.Size = new System.Drawing.Size(187, 54);
            this.btnKelola.TabIndex = 13;
            this.btnKelola.Text = "Kelola Buku";
            this.btnKelola.UseVisualStyleBackColor = true;
            this.btnKelola.Click += new System.EventHandler(this.btnKelola_Click);
            // 
            // FormTransaksi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 927);
            this.Controls.Add(this.btnKelola);
            this.Controls.Add(this.btnLaporan);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvTransaksi);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.lblJumlah);
            this.Controls.Add(this.txtJumlah);
            this.Controls.Add(this.txtHarga);
            this.Controls.Add(this.lblHarga);
            this.Controls.Add(this.lblPilih);
            this.Controls.Add(this.cbBuku);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormTransaksi";
            this.Text = "Transaksi";
            this.Load += new System.EventHandler(this.FormTransaksi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaksi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cbBuku;
        private System.Windows.Forms.Label lblPilih;
        private System.Windows.Forms.Label lblHarga;
        private System.Windows.Forms.TextBox txtHarga;
        private System.Windows.Forms.TextBox txtJumlah;
        private System.Windows.Forms.Label lblJumlah;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dgvTransaksi;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnLaporan;
        private System.Windows.Forms.Button btnKelola;
    }
}