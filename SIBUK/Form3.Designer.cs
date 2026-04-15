namespace SIBUK
{
    partial class FormKelolaBuku
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
            this.lblJudul = new System.Windows.Forms.Label();
            this.lblPengarang = new System.Windows.Forms.Label();
            this.lblPenerbit = new System.Windows.Forms.Label();
            this.lblHarga = new System.Windows.Forms.Label();
            this.lblStok = new System.Windows.Forms.Label();
            this.txtJudul = new System.Windows.Forms.TextBox();
            this.txtPengarang = new System.Windows.Forms.TextBox();
            this.txtPenerbit = new System.Windows.Forms.TextBox();
            this.txtHarga = new System.Windows.Forms.TextBox();
            this.txtStok = new System.Windows.Forms.TextBox();
            this.btnTambah = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgvBuku = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuku)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(302, 45);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(178, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Kelola Data Buku";
            // 
            // lblJudul
            // 
            this.lblJudul.AutoSize = true;
            this.lblJudul.Location = new System.Drawing.Point(79, 110);
            this.lblJudul.Name = "lblJudul";
            this.lblJudul.Size = new System.Drawing.Size(64, 25);
            this.lblJudul.TabIndex = 1;
            this.lblJudul.Text = "Judul";
            // 
            // lblPengarang
            // 
            this.lblPengarang.AutoSize = true;
            this.lblPengarang.Location = new System.Drawing.Point(79, 161);
            this.lblPengarang.Name = "lblPengarang";
            this.lblPengarang.Size = new System.Drawing.Size(117, 25);
            this.lblPengarang.TabIndex = 2;
            this.lblPengarang.Text = "Pengarang";
            // 
            // lblPenerbit
            // 
            this.lblPenerbit.AutoSize = true;
            this.lblPenerbit.Location = new System.Drawing.Point(79, 216);
            this.lblPenerbit.Name = "lblPenerbit";
            this.lblPenerbit.Size = new System.Drawing.Size(92, 25);
            this.lblPenerbit.TabIndex = 3;
            this.lblPenerbit.Text = "Penerbit";
            // 
            // lblHarga
            // 
            this.lblHarga.AutoSize = true;
            this.lblHarga.Location = new System.Drawing.Point(79, 267);
            this.lblHarga.Name = "lblHarga";
            this.lblHarga.Size = new System.Drawing.Size(70, 25);
            this.lblHarga.TabIndex = 4;
            this.lblHarga.Text = "Harga";
            // 
            // lblStok
            // 
            this.lblStok.AutoSize = true;
            this.lblStok.Location = new System.Drawing.Point(84, 319);
            this.lblStok.Name = "lblStok";
            this.lblStok.Size = new System.Drawing.Size(55, 25);
            this.lblStok.TabIndex = 5;
            this.lblStok.Text = "Stok";
            // 
            // txtJudul
            // 
            this.txtJudul.Location = new System.Drawing.Point(259, 110);
            this.txtJudul.Name = "txtJudul";
            this.txtJudul.Size = new System.Drawing.Size(221, 31);
            this.txtJudul.TabIndex = 6;
            // 
            // txtPengarang
            // 
            this.txtPengarang.Location = new System.Drawing.Point(259, 161);
            this.txtPengarang.Name = "txtPengarang";
            this.txtPengarang.Size = new System.Drawing.Size(221, 31);
            this.txtPengarang.TabIndex = 7;
            // 
            // txtPenerbit
            // 
            this.txtPenerbit.Location = new System.Drawing.Point(259, 216);
            this.txtPenerbit.Name = "txtPenerbit";
            this.txtPenerbit.Size = new System.Drawing.Size(221, 31);
            this.txtPenerbit.TabIndex = 8;
            // 
            // txtHarga
            // 
            this.txtHarga.Location = new System.Drawing.Point(259, 267);
            this.txtHarga.Name = "txtHarga";
            this.txtHarga.Size = new System.Drawing.Size(221, 31);
            this.txtHarga.TabIndex = 9;
            // 
            // txtStok
            // 
            this.txtStok.Location = new System.Drawing.Point(259, 319);
            this.txtStok.Name = "txtStok";
            this.txtStok.Size = new System.Drawing.Size(221, 31);
            this.txtStok.TabIndex = 10;
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(84, 399);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(146, 64);
            this.btnTambah.TabIndex = 11;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(292, 400);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(157, 62);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(519, 400);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(156, 63);
            this.btnHapus.TabIndex = 13;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(742, 400);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(149, 63);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgvBuku
            // 
            this.dgvBuku.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuku.Location = new System.Drawing.Point(84, 531);
            this.dgvBuku.Name = "dgvBuku";
            this.dgvBuku.RowHeadersWidth = 82;
            this.dgvBuku.RowTemplate.Height = 33;
            this.dgvBuku.Size = new System.Drawing.Size(807, 291);
            this.dgvBuku.TabIndex = 15;
            this.dgvBuku.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBuku_CellContentClick);
            // 
            // FormKelolaBuku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 873);
            this.Controls.Add(this.dgvBuku);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.txtStok);
            this.Controls.Add(this.txtHarga);
            this.Controls.Add(this.txtPenerbit);
            this.Controls.Add(this.txtPengarang);
            this.Controls.Add(this.txtJudul);
            this.Controls.Add(this.lblStok);
            this.Controls.Add(this.lblHarga);
            this.Controls.Add(this.lblPenerbit);
            this.Controls.Add(this.lblPengarang);
            this.Controls.Add(this.lblJudul);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormKelolaBuku";
            this.Text = "Kelola Buku";
            this.Load += new System.EventHandler(this.FormKelolaBuku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuku)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblJudul;
        private System.Windows.Forms.Label lblPengarang;
        private System.Windows.Forms.Label lblPenerbit;
        private System.Windows.Forms.Label lblHarga;
        private System.Windows.Forms.Label lblStok;
        private System.Windows.Forms.TextBox txtJudul;
        private System.Windows.Forms.TextBox txtPengarang;
        private System.Windows.Forms.TextBox txtPenerbit;
        private System.Windows.Forms.TextBox txtHarga;
        private System.Windows.Forms.TextBox txtStok;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dgvBuku;
    }
}