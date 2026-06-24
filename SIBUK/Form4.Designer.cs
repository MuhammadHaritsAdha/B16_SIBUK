namespace SIBUK
{
    partial class FormLaporan
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
            this.lblTanggalAwal = new System.Windows.Forms.Label();
            this.lblTanggalAkhir = new System.Windows.Forms.Label();
            this.dtpAwal = new System.Windows.Forms.DateTimePicker();
            this.dtpAkhir = new System.Windows.Forms.DateTimePicker();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvLaporan = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtJumlah = new System.Windows.Forms.TextBox();
            this.txtCariBuku = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnKembali = new System.Windows.Forms.Button();
            this.lblJudul = new System.Windows.Forms.Label();
            this.btnCetak = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaporan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(223, 37);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(142, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Laporan Penjualan";
            // 
            // lblTanggalAwal
            // 
            this.lblTanggalAwal.AutoSize = true;
            this.lblTanggalAwal.Location = new System.Drawing.Point(47, 87);
            this.lblTanggalAwal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTanggalAwal.Name = "lblTanggalAwal";
            this.lblTanggalAwal.Size = new System.Drawing.Size(104, 20);
            this.lblTanggalAwal.TabIndex = 1;
            this.lblTanggalAwal.Text = "Tanggal Awal";
            // 
            // lblTanggalAkhir
            // 
            this.lblTanggalAkhir.AutoSize = true;
            this.lblTanggalAkhir.Location = new System.Drawing.Point(47, 150);
            this.lblTanggalAkhir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTanggalAkhir.Name = "lblTanggalAkhir";
            this.lblTanggalAkhir.Size = new System.Drawing.Size(106, 20);
            this.lblTanggalAkhir.TabIndex = 2;
            this.lblTanggalAkhir.Text = "Tanggal Akhir";
            // 
            // dtpAwal
            // 
            this.dtpAwal.Location = new System.Drawing.Point(211, 87);
            this.dtpAwal.Margin = new System.Windows.Forms.Padding(2);
            this.dtpAwal.Name = "dtpAwal";
            this.dtpAwal.Size = new System.Drawing.Size(269, 26);
            this.dtpAwal.TabIndex = 3;
            // 
            // dtpAkhir
            // 
            this.dtpAkhir.Location = new System.Drawing.Point(211, 146);
            this.dtpAkhir.Margin = new System.Windows.Forms.Padding(2);
            this.dtpAkhir.Name = "dtpAkhir";
            this.dtpAkhir.Size = new System.Drawing.Size(269, 26);
            this.dtpAkhir.TabIndex = 4;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(701, 201);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(116, 42);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvLaporan
            // 
            this.dgvLaporan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLaporan.Location = new System.Drawing.Point(58, 290);
            this.dgvLaporan.Margin = new System.Windows.Forms.Padding(2);
            this.dgvLaporan.Name = "dgvLaporan";
            this.dgvLaporan.RowHeadersWidth = 82;
            this.dgvLaporan.RowTemplate.Height = 33;
            this.dgvLaporan.Size = new System.Drawing.Size(759, 244);
            this.dgvLaporan.TabIndex = 7;
            this.dgvLaporan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLaporan_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 577);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Total Penjualan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 637);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Jumlah Transaksi";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(240, 577);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(2);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(314, 26);
            this.txtTotal.TabIndex = 10;
            // 
            // txtJumlah
            // 
            this.txtJumlah.Location = new System.Drawing.Point(240, 637);
            this.txtJumlah.Margin = new System.Windows.Forms.Padding(2);
            this.txtJumlah.Name = "txtJumlah";
            this.txtJumlah.Size = new System.Drawing.Size(314, 26);
            this.txtJumlah.TabIndex = 11;
            // 
            // txtCariBuku
            // 
            this.txtCariBuku.Location = new System.Drawing.Point(211, 208);
            this.txtCariBuku.Margin = new System.Windows.Forms.Padding(2);
            this.txtCariBuku.Name = "txtCariBuku";
            this.txtCariBuku.Size = new System.Drawing.Size(189, 26);
            this.txtCariBuku.TabIndex = 12;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(533, 204);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(130, 38);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnKembali
            // 
            this.btnKembali.Location = new System.Drawing.Point(830, 543);
            this.btnKembali.Margin = new System.Windows.Forms.Padding(2);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(122, 54);
            this.btnKembali.TabIndex = 14;
            this.btnKembali.Text = "Kembali";
            this.btnKembali.UseVisualStyleBackColor = true;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click);
            // 
            // lblJudul
            // 
            this.lblJudul.AutoSize = true;
            this.lblJudul.Location = new System.Drawing.Point(51, 212);
            this.lblJudul.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblJudul.Name = "lblJudul";
            this.lblJudul.Size = new System.Drawing.Size(88, 20);
            this.lblJudul.TabIndex = 15;
            this.lblJudul.Text = "Judul Buku";
            // 
            // btnCetak
            // 
            this.btnCetak.Location = new System.Drawing.Point(830, 619);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(116, 57);
            this.btnCetak.TabIndex = 16;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.UseVisualStyleBackColor = true;
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // FormLaporan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 1008);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.lblJudul);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtCariBuku);
            this.Controls.Add(this.txtJumlah);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLaporan);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.dtpAkhir);
            this.Controls.Add(this.dtpAwal);
            this.Controls.Add(this.lblTanggalAkhir);
            this.Controls.Add(this.lblTanggalAwal);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormLaporan";
            this.Text = "Laporan Penjualan";
            this.Load += new System.EventHandler(this.FormLaporan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaporan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTanggalAwal;
        private System.Windows.Forms.Label lblTanggalAkhir;
        private System.Windows.Forms.DateTimePicker dtpAwal;
        private System.Windows.Forms.DateTimePicker dtpAkhir;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dgvLaporan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtJumlah;
        private System.Windows.Forms.TextBox txtCariBuku;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnKembali;
        private System.Windows.Forms.Label lblJudul;
        private System.Windows.Forms.Button btnCetak;
    }
}