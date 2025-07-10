using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SewaRuanganUmy2
{
    public partial class FormDashboard : Form
    {
       
        private string connectionString;

        private int tahunDefault;

        public FormDashboard()
        {
            InitializeComponent();
            connectionString = Koneksi.GetConnectionString();

            cmbTahun.SelectedIndexChanged += cmbTahun_SelectedIndexChanged;
            this.Load += FormDashboard_Load;
        }


        private void FormDashboard_Load(object sender, EventArgs e)
        {
            LoadTahunReservasi();

            if (cmbTahun.Items.Count > 0)
            {
                cmbTahun.SelectedIndex = 0;
                int tahun = Convert.ToInt32(cmbTahun.SelectedItem);
                TampilkanChartReservasi(tahun);
            }
        }

        private void LoadTahunReservasi()
        {
            cmbTahun.Items.Clear();

            string query = "SELECT DISTINCT YEAR(tanggal_reservasi) AS Tahun FROM Reservasi ORDER BY Tahun";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbTahun.Items.Add(reader.GetInt32(0)); // Tambahkan tahun ke ComboBox
                }
            }
        }

        private void cmbTahun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTahun.SelectedItem != null)
            {
                int tahun = Convert.ToInt32(cmbTahun.SelectedItem);
                TampilkanChartReservasi(tahun);
            }
        }

        private void TampilkanChartReservasi(int tahun)
        {
            string query = @"
                SELECT FORMAT(tanggal_reservasi, 'MMMM') AS Bulan,
                       COUNT(*) AS JumlahReservasi
                FROM Reservasi
                WHERE YEAR(tanggal_reservasi) = @tahun
                GROUP BY FORMAT(tanggal_reservasi, 'MMMM'), MONTH(tanggal_reservasi)
                ORDER BY MONTH(tanggal_reservasi)";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tahun", tahun);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            chartReservasi.Series.Clear();
            chartReservasi.ChartAreas.Clear();
            chartReservasi.Titles.Clear();

            chartReservasi.ChartAreas.Add(new ChartArea("MainArea"));

            Series series = new Series("Jumlah Reservasi");
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;

            foreach (DataRow row in dt.Rows)
            {
                string bulan = row["Bulan"].ToString();
                int jumlah = Convert.ToInt32(row["JumlahReservasi"]);
                series.Points.AddXY(bulan, jumlah);
            }

            chartReservasi.Series.Add(series);
            chartReservasi.Titles.Add($"Jumlah Reservasi Tahun {tahun}");
        }

        private void btnkembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
