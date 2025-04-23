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
            FormPelanggan formPelanggan = new FormPelanggan();
            formPelanggan.ShowDialog();
        }

        private void BtnPaket_Click(object sender, EventArgs e)
        {
            FormPaket formPaket = new FormPaket();
            formPaket.ShowDialog(); // agar muncul sebagai form popup dan fokus
        }

        private void BtnRuangan_Click(Object sender, EventArgs e)
        {
            FormRuangan formRuangan = new FormRuangan();
            formRuangan.ShowDialog();
        }

        private void BtnReservasi_Click(Object sender, EventArgs e)
        {
            FormReservasi formReservasi = new FormReservasi();
            formReservasi.ShowDialog();
        }

        private void BtnPembayaran_Click(Object sender, EventArgs e)
        {
            FormPembayaran formPembayaran = new FormPembayaran();
            formPembayaran.ShowDialog();
        }
    }
}
