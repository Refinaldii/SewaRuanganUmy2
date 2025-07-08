using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SewaRuanganUmy2
{
    



    public partial class Reportviewer : Form
    {
        private int _idPelanggan;   

        public Reportviewer(int idPelanggan)
        {
            InitializeComponent();
            _idPelanggan = idPelanggan;
        }


        private void Reportviewer_Load(object sender, EventArgs e)
        {
            // Setup ReportViewer data
            SetupReportViewer();

            // Refresh report to display data
            this.reportViewer1.RefreshReport();
        }

        private void SetupReportViewer()
        {
            string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";
            string query = @"SELECT 
    Reservasi.id_reservasi, 
    Pelanggan.nama, 
    Paket.nama_paket, 
    Reservasi.tanggal_reservasi, 
    Reservasi.jam_mulai, 
    Reservasi.jam_selesai, 
    Reservasi.status, 
    ISNULL(Pembayaran.jumlah, 0) AS jumlah,
    ISNULL(Pembayaran.metode_pembayaran, '-') AS metode_pembayaran,
    ISNULL(Pembayaran.status, 'Menunggu') AS status_pembayaran,
    Ruangan.nama_ruangan
FROM Paket
INNER JOIN Reservasi ON Paket.id_paket = Reservasi.id_paket
INNER JOIN Pelanggan ON Reservasi.id_pelanggan = Pelanggan.id_pelanggan
LEFT JOIN Pembayaran ON Reservasi.id_reservasi = Pembayaran.id_reservasi
INNER JOIN Ruangan ON Reservasi.id_ruangan = Ruangan.id_ruangan
WHERE Pelanggan.id_pelanggan = @idPelanggan";


            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idPelanggan", _idPelanggan);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    MessageBox.Show("Jumlah baris ditemukan: " + dt.Rows.Count);
                }
            }

            // GANTI "DataTable1" DENGAN NAMA DATASET DI RDLC KAMU
            ReportDataSource rds = new ReportDataSource("DataTable1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.ReportPath = "D:\\semster 4\\PABD\\SewaRuanganUmy2\\NotaPembayaranSewaRuangan.rdlc";
            reportViewer1.RefreshReport();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Close(); // Tutup form saat ini
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
