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
        public Reportviewer()
        {
            InitializeComponent();
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
            string query = @"SELECT Reservasi.id_reservasi, Pelanggan.nama, Paket.nama_paket, 
                            Reservasi.tanggal_reservasi, Reservasi.jam_mulai, Reservasi.jam_selesai, 
                            Reservasi.status, Pembayaran.jumlah, Pembayaran.metode_pembayaran, 
                            Pembayaran.status AS Expr1, Ruangan.nama_ruangan
                     FROM Paket
                     INNER JOIN Reservasi ON Paket.id_paket = Reservasi.id_paket
                     INNER JOIN Pelanggan ON Reservasi.id_pelanggan = Pelanggan.id_pelanggan
                     INNER JOIN Pembayaran ON Reservasi.id_reservasi = Pembayaran.id_reservasi
                     INNER JOIN Ruangan ON Reservasi.id_ruangan = Ruangan.id_ruangan";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dt); // "DataSet1" HARUS sama dengan nama dataset di RDLC
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Pastikan ini mengarah ke file .rdlc kamu
            reportViewer1.LocalReport.ReportPath = "D:\\semster 4'\\PABD\\SewaRuanganUmy2\\SewaRuanganReport.rdlc";

            reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close(); // Tutup form saat ini
        }

    }
}
