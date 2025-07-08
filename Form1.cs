using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SewaRuanganUmy2
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUmy;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // default tampilan awal
        }

        private void BtnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Menutup aplikasi
        }

        private void BtnPelanggan_Click(object sender, EventArgs e)
        {
            this.Hide(); // Sembunyikan form saat ini
            FormPelanggan formPelanggan = new FormPelanggan();

            // Saat FormPelanggan ditutup, tampilkan kembali form ini
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


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCetakNota cetakNota = new FormCetakNota();
            cetakNota.ShowDialog();
            this.Show(); // Tampilkan kembali form utama setelah form nota ditutup
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide(); // Sembunyikan form menu
            FormDashboard formDashboard = new FormDashboard();
            formDashboard.FormClosed += (s, args) => this.Show(); // Tampilkan kembali saat ditutup
            formDashboard.Show(); // Tampilkan dashboard
        }
    

    }
}
