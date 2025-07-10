using System;
using System.Windows.Forms;

namespace SewaRuanganUmy2
{
    public partial class Form1 : Form
    {
        private string connectionString = Koneksi.GetConnectionString();

        public string GetConnectionString()
        {
            return connectionString;
        }

        public Form1()
        {
            InitializeComponent();
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void BtnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPelanggan_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPelanggan formPelanggan = new FormPelanggan(this);
            formPelanggan.FormClosed += (s, args) => this.Show();
            formPelanggan.Show();
        }

        private void BtnPaket_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPaket formPaket = new FormPaket();
            formPaket.FormClosed += (s, args) => this.Show();
            formPaket.Show();
        }

        private void BtnRuangan_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormRuangan formRuangan = new FormRuangan();
            formRuangan.FormClosed += (s, args) => this.Show();
            formRuangan.Show();
        }

        private void BtnReservasi_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormReservasi formReservasi = new FormReservasi(this);
            formReservasi.FormClosed += (s, args) => this.Show();
            formReservasi.Show();
        }

        private void BtnPembayaran_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPembayaran formPembayaran = new FormPembayaran(this);
            formPembayaran.FormClosed += (s, args) => this.Show();
            formPembayaran.Show();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCetakNota cetakNota = new FormCetakNota();
            cetakNota.ShowDialog();
            this.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormDashboard formDashboard = new FormDashboard();
            formDashboard.FormClosed += (s, args) => this.Show();
            formDashboard.Show();
        }
    }
}
