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
            this.btnTampil = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvLaporan = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtJumlah = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaporan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(297, 46);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(193, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Laporan Penjualan";
            // 
            // lblTanggalAwal
            // 
            this.lblTanggalAwal.AutoSize = true;
            this.lblTanggalAwal.Location = new System.Drawing.Point(63, 109);
            this.lblTanggalAwal.Name = "lblTanggalAwal";
            this.lblTanggalAwal.Size = new System.Drawing.Size(142, 25);
            this.lblTanggalAwal.TabIndex = 1;
            this.lblTanggalAwal.Text = "Tanggal Awal";
            // 
            // lblTanggalAkhir
            // 
            this.lblTanggalAkhir.AutoSize = true;
            this.lblTanggalAkhir.Location = new System.Drawing.Point(63, 187);
            this.lblTanggalAkhir.Name = "lblTanggalAkhir";
            this.lblTanggalAkhir.Size = new System.Drawing.Size(145, 25);
            this.lblTanggalAkhir.TabIndex = 2;
            this.lblTanggalAkhir.Text = "Tanggal Akhir";
            // 
            // dtpAwal
            // 
            this.dtpAwal.Location = new System.Drawing.Point(281, 109);
            this.dtpAwal.Name = "dtpAwal";
            this.dtpAwal.Size = new System.Drawing.Size(357, 31);
            this.dtpAwal.TabIndex = 3;
            // 
            // dtpAkhir
            // 
            this.dtpAkhir.Location = new System.Drawing.Point(281, 182);
            this.dtpAkhir.Name = "dtpAkhir";
            this.dtpAkhir.Size = new System.Drawing.Size(357, 31);
            this.dtpAkhir.TabIndex = 4;
            // 
            // btnTampil
            // 
            this.btnTampil.Location = new System.Drawing.Point(68, 262);
            this.btnTampil.Name = "btnTampil";
            this.btnTampil.Size = new System.Drawing.Size(140, 52);
            this.btnTampil.TabIndex = 5;
            this.btnTampil.Text = "Tampilkan";
            this.btnTampil.UseVisualStyleBackColor = true;
            this.btnTampil.Click += new System.EventHandler(this.btnTampil_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(281, 262);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(154, 52);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvLaporan
            // 
            this.dgvLaporan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLaporan.Location = new System.Drawing.Point(77, 363);
            this.dgvLaporan.Name = "dgvLaporan";
            this.dgvLaporan.RowHeadersWidth = 82;
            this.dgvLaporan.RowTemplate.Height = 33;
            this.dgvLaporan.Size = new System.Drawing.Size(1012, 305);
            this.dgvLaporan.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 721);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Total Penjualan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 796);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Jumlah Transaksi";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(320, 721);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(417, 31);
            this.txtTotal.TabIndex = 10;
            // 
            // txtJumlah
            // 
            this.txtJumlah.Location = new System.Drawing.Point(320, 796);
            this.txtJumlah.Name = "txtJumlah";
            this.txtJumlah.Size = new System.Drawing.Size(417, 31);
            this.txtJumlah.TabIndex = 11;
            // 
            // FormLaporan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1399, 918);
            this.Controls.Add(this.txtJumlah);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLaporan);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnTampil);
            this.Controls.Add(this.dtpAkhir);
            this.Controls.Add(this.dtpAwal);
            this.Controls.Add(this.lblTanggalAkhir);
            this.Controls.Add(this.lblTanggalAwal);
            this.Controls.Add(this.lblTitle);
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
        private System.Windows.Forms.Button btnTampil;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dgvLaporan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtJumlah;
    }
}