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
    public partial class FormPembayaran : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";
        private int selectedIdPembayaran = -1;
        private Form1 _form1;


        public FormPembayaran(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1; // ← Ini menyimpan objek Form1
        }

        private void FormPembayaran_Load(object sender, EventArgs e)
        {
            LoadComboReservasi();
            LoadComboMetodePembayaran();
            LoadComboStatus();
            LoadDataPembayaran();
        }

        private void LoadComboReservasi()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT id_reservasi FROM Reservasi";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbReservasi.DataSource = dt;
                cmbReservasi.DisplayMember = "id_reservasi";
                cmbReservasi.ValueMember = "id_reservasi";
                cmbReservasi.SelectedIndex = -1;
            }
        }

        private void LoadComboMetodePembayaran()
        {
            cmbMetodePembayaran.Items.Clear();
            cmbMetodePembayaran.Items.Add("Transfer Bank");
            cmbMetodePembayaran.Items.Add("Kartu Kredit");
            cmbMetodePembayaran.Items.Add("Kartu Debit");
            cmbMetodePembayaran.Items.Add("E-Wallet");
            cmbMetodePembayaran.SelectedIndex = -1;
        }

        private void LoadComboStatus()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Menunggu");
            cmbStatus.Items.Add("Lunas");
            cmbStatus.Items.Add("Gagal");
            cmbStatus.SelectedIndex = -1;
        }


        private void LoadDataPembayaran()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllPembayaran", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvPembayaran.DataSource = dt;
                }
            }
        }
        private void ClearForm()
        {
            cmbReservasi.SelectedIndex = -1;
            txtJumlah.Clear();
            cmbMetodePembayaran.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            dtpTanggalPembayaran.Value = DateTime.Today;
        }


        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (cmbReservasi.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtJumlah.Text) ||
                cmbMetodePembayaran.SelectedItem == null ||
                cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Harap isi semua field.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jumlahText = txtJumlah.Text.Trim().Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(jumlahText, out decimal jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah tidak valid.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertPembayaran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_reservasi", cmbReservasi.SelectedValue);
                        cmd.Parameters.AddWithValue("@jumlah", jumlah);
                        cmd.Parameters.AddWithValue("@metode_pembayaran", cmbMetodePembayaran.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                            LoadDataPembayaran();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyimpan data.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedIdPembayaran == -1)
            {
                MessageBox.Show("Silakan pilih data yang ingin diupdate.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbReservasi.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtJumlah.Text) ||
                cmbMetodePembayaran.SelectedItem == null ||
                cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua field harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jumlahText = txtJumlah.Text.Trim().Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(jumlahText, out decimal jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah tidak valid.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin mengupdate data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdatePembayaran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_pembayaran", selectedIdPembayaran);
                        cmd.Parameters.AddWithValue("@id_reservasi", cmbReservasi.SelectedValue);
                        cmd.Parameters.AddWithValue("@jumlah", jumlah);
                        cmd.Parameters.AddWithValue("@metode_pembayaran", cmbMetodePembayaran.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                            LoadDataPembayaran();
                            selectedIdPembayaran = -1;
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate data.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedIdPembayaran == -1)
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var konfirmasi = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeletePembayaran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_pembayaran", selectedIdPembayaran);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                            LoadDataPembayaran();
                            selectedIdPembayaran = -1;
                        }
                        else
                        {
                            MessageBox.Show("Gagal menghapus data.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnTutup_Click(object sender, EventArgs e)
        {
            _form1.Show();   // Tampilkan kembali Form1
            this.Close();    // Tutup FormReservasi
        }

        private void dgvPembayaran_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPembayaran.Rows[e.RowIndex];

                selectedIdPembayaran = Convert.ToInt32(row.Cells["id_pembayaran"].Value);
                cmbReservasi.SelectedValue = row.Cells["id_reservasi"].Value;
                txtJumlah.Text = row.Cells["jumlah"].Value.ToString();
                cmbMetodePembayaran.SelectedItem = row.Cells["metode_pembayaran"].Value.ToString();
                cmbStatus.SelectedItem = row.Cells["status"].Value.ToString();

                // Update ke DateTimePicker
                if (DateTime.TryParse(row.Cells["tanggal_pembayaran"].Value.ToString(), out DateTime tanggal))
                {
                    dtpTanggalPembayaran.Value = tanggal;
                }
                else
                {
                    // fallback ke tanggal hari ini jika parsing gagal
                    dtpTanggalPembayaran.Value = DateTime.Today;
                }
            }
        }

    }
}