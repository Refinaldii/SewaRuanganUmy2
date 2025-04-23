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
        public FormPembayaran()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(txtIDReservasi.Text) ||
                string.IsNullOrWhiteSpace(txtJumlah.Text) ||
                string.IsNullOrWhiteSpace(txtMetodePembayaran.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtTanggalPembayaran.Text))
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi tanggal
            DateTime tanggalPembayaran;
            if (!DateTime.TryParse(txtTanggalPembayaran.Text, out tanggalPembayaran))
            {
                MessageBox.Show("Format tanggal tidak valid. Gunakan format yyyy-MM-dd!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lakukan update data ke database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Pembayaran 
                             SET jumlah = @Jumlah, 
                                 metode_pembayaran = @Metode, 
                                 status = @Status, 
                                 tanggal_pembayaran = @Tanggal 
                             WHERE id_reservasi = @IdReservasi";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Jumlah", txtJumlah.Text);
                        cmd.Parameters.AddWithValue("@Metode", txtMetodePembayaran.Text);
                        cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                        cmd.Parameters.AddWithValue("@Tanggal", tanggalPembayaran);
                        cmd.Parameters.AddWithValue("@IdReservasi", txtIDReservasi.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm(); // Optional
                                         // LoadData(); // Kalau kamu punya fungsi ini, bisa diaktifkan
                        }
                        else
                        {
                            MessageBox.Show("Data gagal diupdate atau tidak ditemukan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat update: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDReservasi.Text))
            {
                MessageBox.Show("Harap masukkan ID Reservasi yang akan dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Pembayaran WHERE id_reservasi = @IdReservasi";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdReservasi", txtIDReservasi.Text);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm(); // Optional: hapus data dari textbox
                        }
                        else
                        {
                            MessageBox.Show("Data tidak ditemukan atau gagal dihapus.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Validasi kosong
            if (string.IsNullOrWhiteSpace(txtIDReservasi.Text) ||
                string.IsNullOrWhiteSpace(txtJumlah.Text) ||
                string.IsNullOrWhiteSpace(txtMetodePembayaran.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtTanggalPembayaran.Text))
            {
                MessageBox.Show("Harap isi semua field.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi jumlah
            if (!decimal.TryParse(txtJumlah.Text, out decimal jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah tidak valid. Masukkan angka lebih dari 0.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi tanggal
            if (!DateTime.TryParse(txtTanggalPembayaran.Text, out DateTime tanggalPembayaran))
            {
                MessageBox.Show("Tanggal pembayaran tidak valid. Format: yyyy-MM-dd.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi metode pembayaran
            string[] metodeValid = { "Transfer Bank", "Kartu Kredit", "Kartu Debit", "E-Wallet" };
            if (!metodeValid.Contains(txtMetodePembayaran.Text.Trim()))
            {
                MessageBox.Show("Metode pembayaran tidak valid. Pilihan yang benar: " + string.Join(", ", metodeValid),
                                "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi status
            string[] statusValid = { "Menunggu", "Lunas", "Gagal" };
            if (!statusValid.Contains(txtStatus.Text.Trim()))
            {
                MessageBox.Show("Status tidak valid. Pilihan yang benar: " + string.Join(", ", statusValid),
                                "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simpan ke database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Pembayaran (id_reservasi, jumlah, metode_pembayaran, status, tanggal_pembayaran)
                             VALUES (@IdReservasi, @Jumlah, @Metode, @Status, @Tanggal)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdReservasi", txtIDReservasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@Jumlah", jumlah);
                        cmd.Parameters.AddWithValue("@Metode", txtMetodePembayaran.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", txtStatus.Text.Trim());
                        cmd.Parameters.AddWithValue("@Tanggal", tanggalPembayaran);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data pembayaran berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyimpan data pembayaran.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Pembayaran";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPembayaran.DataSource = dt;
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data: " + ex.Message);
                }
            }
        }




        private void ClearForm()
        {
            txtIDReservasi.Clear();
            txtJumlah.Clear();
            txtMetodePembayaran.Clear();
            txtStatus.Clear();
            txtTanggalPembayaran.Clear();
        }


        private void dgvPembayaran_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPembayaran.Rows[e.RowIndex];

                txtIDReservasi.Text = row.Cells["id_reservasi"].Value.ToString();
                txtJumlah.Text = row.Cells["jumlah"].Value.ToString();
                txtMetodePembayaran.Text = row.Cells["metode_pembayaran"].Value.ToString();
                txtStatus.Text = row.Cells["status"].Value.ToString();
                txtTanggalPembayaran.Text = Convert.ToDateTime(row.Cells["tanggal_pembayaran"].Value).ToString("yyyy-MM-dd");
            }
        }


        private void FormPembayaran_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvPembayaranCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPembayaran.Rows[e.RowIndex];

                txtIDReservasi.Text = row.Cells["id_reservasi"].Value.ToString();
                txtJumlah.Text = row.Cells["jumlah"].Value.ToString();
                txtMetodePembayaran.Text = row.Cells["metode_pembayaran"].Value.ToString();
                txtStatus.Text = row.Cells["status"].Value.ToString();
                txtTanggalPembayaran.Text = Convert.ToDateTime(row.Cells["tanggal_pembayaran"].Value).ToString("yyyy-MM-dd");
            }
        }
    }
}
