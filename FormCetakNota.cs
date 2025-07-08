using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SewaRuanganUmy2
{
    public partial class FormCetakNota : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        public FormCetakNota()
        {
            InitializeComponent();
        }

        private void FormCetakNota_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT id_pelanggan, nama FROM Pelanggan", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<int, string> pelangganList = new Dictionary<int, string>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nama = reader.GetString(1);
                    pelangganList.Add(id, nama);
                }

                cmbPelanggan.DataSource = new BindingSource(pelangganList, null);
                cmbPelanggan.DisplayMember = "Value";
                cmbPelanggan.ValueMember = "Key";
            }
        }

        private void btnCetakNota_Click(object sender, EventArgs e)
        {
            if (cmbPelanggan.SelectedItem != null)
            {
                int selectedPelangganId = ((KeyValuePair<int, string>)cmbPelanggan.SelectedItem).Key;
                Reportviewer reportForm = new Reportviewer(selectedPelangganId);
                reportForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Pilih pelanggan terlebih dahulu.");
            }
        }

        // HAPUS atau kosongkan event ini di Designer
        private void cmbPelanggan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kosongkan agar tidak auto-munculkan report
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
