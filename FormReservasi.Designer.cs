namespace SewaRuanganUmy2
{
    partial class FormReservasi
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtJamSelesai = new System.Windows.Forms.DateTimePicker();
            this.txtJamMulai = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbPaket = new System.Windows.Forms.ComboBox();
            this.cmbRuangan = new System.Windows.Forms.ComboBox();
            this.dtpTanggalReservasi = new System.Windows.Forms.DateTimePicker();
            this.cmbPelanggan = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnTambahkan = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.dgvReservasi = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtJamSelesai, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtJamMulai, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbPaket, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbRuangan, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dtpTanggalReservasi, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbPelanggan, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbStatus, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.01205F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.98795F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(538, 284);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 16);
            this.label7.TabIndex = 23;
            this.label7.Text = "Status";
            // 
            // txtJamSelesai
            // 
            this.txtJamSelesai.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtJamSelesai.Location = new System.Drawing.Point(272, 213);
            this.txtJamSelesai.Name = "txtJamSelesai";
            this.txtJamSelesai.ShowUpDown = true;
            this.txtJamSelesai.Size = new System.Drawing.Size(263, 22);
            this.txtJamSelesai.TabIndex = 20;
            this.txtJamSelesai.ValueChanged += new System.EventHandler(this.txtJamSelesai_ValueChanged);
            // 
            // txtJamMulai
            // 
            this.txtJamMulai.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtJamMulai.Location = new System.Drawing.Point(272, 179);
            this.txtJamMulai.Name = "txtJamMulai";
            this.txtJamMulai.ShowUpDown = true;
            this.txtJamMulai.Size = new System.Drawing.Size(263, 22);
            this.txtJamMulai.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "ID Pelanggan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "NamaPaket";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Nama Ruangan";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tanggal Reservasi";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Jam Mulai";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Jam Selesai";
            // 
            // cmbPaket
            // 
            this.cmbPaket.FormattingEnabled = true;
            this.cmbPaket.Location = new System.Drawing.Point(272, 47);
            this.cmbPaket.Name = "cmbPaket";
            this.cmbPaket.Size = new System.Drawing.Size(263, 24);
            this.cmbPaket.TabIndex = 17;
            // 
            // cmbRuangan
            // 
            this.cmbRuangan.FormattingEnabled = true;
            this.cmbRuangan.Location = new System.Drawing.Point(272, 86);
            this.cmbRuangan.Name = "cmbRuangan";
            this.cmbRuangan.Size = new System.Drawing.Size(263, 24);
            this.cmbRuangan.TabIndex = 18;
            // 
            // dtpTanggalReservasi
            // 
            this.dtpTanggalReservasi.Location = new System.Drawing.Point(272, 131);
            this.dtpTanggalReservasi.Name = "dtpTanggalReservasi";
            this.dtpTanggalReservasi.Size = new System.Drawing.Size(263, 22);
            this.dtpTanggalReservasi.TabIndex = 19;
            // 
            // cmbPelanggan
            // 
            this.cmbPelanggan.FormattingEnabled = true;
            this.cmbPelanggan.Location = new System.Drawing.Point(272, 3);
            this.cmbPelanggan.Name = "cmbPelanggan";
            this.cmbPelanggan.Size = new System.Drawing.Size(263, 24);
            this.cmbPelanggan.TabIndex = 21;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(272, 249);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(263, 24);
            this.cmbStatus.TabIndex = 22;
            // 
            // btnTambahkan
            // 
            this.btnTambahkan.Location = new System.Drawing.Point(647, 15);
            this.btnTambahkan.Name = "btnTambahkan";
            this.btnTambahkan.Size = new System.Drawing.Size(94, 45);
            this.btnTambahkan.TabIndex = 1;
            this.btnTambahkan.Text = "Tambah";
            this.btnTambahkan.UseVisualStyleBackColor = true;
            this.btnTambahkan.Click += new System.EventHandler(this.btnTambahkan_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(647, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 45);
            this.button2.TabIndex = 2;
            this.button2.Text = "Update";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(647, 137);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 45);
            this.button3.TabIndex = 3;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(647, 201);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 45);
            this.button4.TabIndex = 4;
            this.button4.Text = "Tutup";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnTutup_Click);
            // 
            // dgvReservasi
            // 
            this.dgvReservasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservasi.Location = new System.Drawing.Point(12, 302);
            this.dgvReservasi.Name = "dgvReservasi";
            this.dgvReservasi.RowHeadersWidth = 51;
            this.dgvReservasi.RowTemplate.Height = 24;
            this.dgvReservasi.Size = new System.Drawing.Size(776, 150);
            this.dgvReservasi.TabIndex = 5;
            this.dgvReservasi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReservasi_CellClick);
            this.dgvReservasi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReservasi_CellContentClick);
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Location = new System.Drawing.Point(632, 249);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(123, 47);
            this.btnAnalisis.TabIndex = 6;
            this.btnAnalisis.Text = "Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // FormReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAnalisis);
            this.Controls.Add(this.dgvReservasi);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnTambahkan);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormReservasi";
            this.Text = "FormReservasi";
            this.Load += new System.EventHandler(this.FormReservasi_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservasi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnTambahkan;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dgvReservasi;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ComboBox cmbPaket;
        private System.Windows.Forms.ComboBox cmbRuangan;
        private System.Windows.Forms.DateTimePicker dtpTanggalReservasi;
        private System.Windows.Forms.DateTimePicker txtJamSelesai;
        private System.Windows.Forms.DateTimePicker txtJamMulai;
        private System.Windows.Forms.ComboBox cmbPelanggan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Button btnAnalisis;
    }
}