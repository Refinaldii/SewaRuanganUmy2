namespace SewaRuanganUmy2
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cmbTahun = new System.Windows.Forms.ComboBox();
            this.chartReservasi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.btnkembali = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartReservasi)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbTahun
            // 
            this.cmbTahun.FormattingEnabled = true;
            this.cmbTahun.Location = new System.Drawing.Point(249, 32);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Size = new System.Drawing.Size(179, 24);
            this.cmbTahun.TabIndex = 1;
            // 
            // chartReservasi
            // 
            chartArea2.Name = "ChartArea1";
            this.chartReservasi.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartReservasi.Legends.Add(legend2);
            this.chartReservasi.Location = new System.Drawing.Point(92, 77);
            this.chartReservasi.Name = "chartReservasi";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartReservasi.Series.Add(series2);
            this.chartReservasi.Size = new System.Drawing.Size(562, 333);
            this.chartReservasi.TabIndex = 2;
            this.chartReservasi.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pilih Tahun";
            // 
            // btnkembali
            // 
            this.btnkembali.Location = new System.Drawing.Point(676, 415);
            this.btnkembali.Name = "btnkembali";
            this.btnkembali.Size = new System.Drawing.Size(75, 23);
            this.btnkembali.TabIndex = 4;
            this.btnkembali.Text = "Kembali";
            this.btnkembali.UseVisualStyleBackColor = true;
            this.btnkembali.Click += new System.EventHandler(this.btnkembali_Click);
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnkembali);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartReservasi);
            this.Controls.Add(this.cmbTahun);
            this.Name = "FormDashboard";
            this.Text = "Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.chartReservasi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbTahun;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartReservasi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnkembali;
    }
}