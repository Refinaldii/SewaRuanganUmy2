namespace SewaRuanganUmy2
{
    partial class Form1
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
            this.BtnTambahPelanggan = new System.Windows.Forms.Button();
            this.BtnPaket = new System.Windows.Forms.Button();
            this.BtnKeluar = new System.Windows.Forms.Button();
            this.txtRuangan = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnTambahPelanggan
            // 
            this.BtnTambahPelanggan.Location = new System.Drawing.Point(512, 51);
            this.BtnTambahPelanggan.Name = "BtnTambahPelanggan";
            this.BtnTambahPelanggan.Size = new System.Drawing.Size(94, 40);
            this.BtnTambahPelanggan.TabIndex = 0;
            this.BtnTambahPelanggan.Text = "Pelanggan ";
            this.BtnTambahPelanggan.UseVisualStyleBackColor = true;
            this.BtnTambahPelanggan.Click += new System.EventHandler(this.BtnPelanggan_Click);
            // 
            // BtnPaket
            // 
            this.BtnPaket.Location = new System.Drawing.Point(86, 141);
            this.BtnPaket.Name = "BtnPaket";
            this.BtnPaket.Size = new System.Drawing.Size(94, 47);
            this.BtnPaket.TabIndex = 1;
            this.BtnPaket.Text = "Paket";
            this.BtnPaket.UseVisualStyleBackColor = true;
            this.BtnPaket.Click += new System.EventHandler(this.BtnPaket_Click);
            // 
            // BtnKeluar
            // 
            this.BtnKeluar.Location = new System.Drawing.Point(303, 277);
            this.BtnKeluar.Name = "BtnKeluar";
            this.BtnKeluar.Size = new System.Drawing.Size(75, 23);
            this.BtnKeluar.TabIndex = 2;
            this.BtnKeluar.Text = "Keluar";
            this.BtnKeluar.UseVisualStyleBackColor = true;
            this.BtnKeluar.Click += new System.EventHandler(this.BtnKeluar_Click);
            // 
            // txtRuangan
            // 
            this.txtRuangan.Location = new System.Drawing.Point(86, 51);
            this.txtRuangan.Name = "txtRuangan";
            this.txtRuangan.Size = new System.Drawing.Size(94, 45);
            this.txtRuangan.TabIndex = 3;
            this.txtRuangan.Text = "Ruangan";
            this.txtRuangan.UseVisualStyleBackColor = true;
            this.txtRuangan.Click += new System.EventHandler(this.BtnRuangan_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(512, 123);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 40);
            this.button1.TabIndex = 4;
            this.button1.Text = "Reservasi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnReservasi_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(512, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 40);
            this.button2.TabIndex = 5;
            this.button2.Text = "Pembayaran";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.BtnPembayaran_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRuangan);
            this.Controls.Add(this.BtnKeluar);
            this.Controls.Add(this.BtnPaket);
            this.Controls.Add(this.BtnTambahPelanggan);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnTambahPelanggan;
        private System.Windows.Forms.Button BtnPaket;
        private System.Windows.Forms.Button BtnKeluar;
        private System.Windows.Forms.Button txtRuangan;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

