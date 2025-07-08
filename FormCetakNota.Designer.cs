namespace SewaRuanganUmy2
{
    partial class FormCetakNota
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbPelanggan = new System.Windows.Forms.ComboBox();
            this.btnCetakNota = new System.Windows.Forms.Button();
            this.btnTutup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbPelanggan
            // 
            this.cmbPelanggan.FormattingEnabled = true;
            this.cmbPelanggan.Location = new System.Drawing.Point(114, 55);
            this.cmbPelanggan.Name = "cmbPelanggan";
            this.cmbPelanggan.Size = new System.Drawing.Size(379, 24);
            this.cmbPelanggan.TabIndex = 0;
            this.cmbPelanggan.SelectedIndexChanged += new System.EventHandler(this.cmbPelanggan_SelectedIndexChanged);
            // 
            // btnCetakNota
            // 
            this.btnCetakNota.Location = new System.Drawing.Point(114, 107);
            this.btnCetakNota.Name = "btnCetakNota";
            this.btnCetakNota.Size = new System.Drawing.Size(379, 23);
            this.btnCetakNota.TabIndex = 1;
            this.btnCetakNota.Text = "Cetak Nota";
            this.btnCetakNota.UseVisualStyleBackColor = true;
            this.btnCetakNota.Click += new System.EventHandler(this.btnCetakNota_Click);
            // 
            // btnTutup
            // 
            this.btnTutup.Location = new System.Drawing.Point(628, 388);
            this.btnTutup.Name = "btnTutup";
            this.btnTutup.Size = new System.Drawing.Size(160, 50);
            this.btnTutup.TabIndex = 2;
            this.btnTutup.Text = "Tutup";
            this.btnTutup.UseVisualStyleBackColor = true;
            this.btnTutup.Click += new System.EventHandler(this.btnTutup_Click);
            // 
            // FormCetakNota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTutup);
            this.Controls.Add(this.btnCetakNota);
            this.Controls.Add(this.cmbPelanggan);
            this.Name = "FormCetakNota";
            this.Text = "FormCetakNota";
            this.Load += new System.EventHandler(this.FormCetakNota_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPelanggan;
        private System.Windows.Forms.Button btnCetakNota;
        private System.Windows.Forms.Button btnTutup;
    }
}
