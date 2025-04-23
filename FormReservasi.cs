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
    public partial class FormReservasi : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        public FormReservasi()
        {
            InitializeComponent();
        }

        private void FormReservasi_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTanggalReservasi.Text) ||
                string.IsNullOrWhiteSpace(txtJamMulai.Text) ||
                string.IsNullOrWhiteSpace(txtJamSelesai.Text) ||
                string.IsNullOrWhiteSpace(txtNamaPelanggan.Text) ||
                string.IsNullOrWhiteSpace(txtNamaPaket.Text) ||
                string.IsNullOrWhiteSpace(txtNamaRuangan.Text))
            {
                MessageBox.Show("Harap isi semua data.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggal;
            if (!DateTime.TryParse(txtTanggalReservasi.Text, out tanggal))
            {
                MessageBox.Show("Format tanggal tidak valid. Harap gunakan format yyyy-MM-dd.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamMulai;
            if (!TimeSpan.TryParse(txtJamMulai.Text, out jamMulai))
            {
                MessageBox.Show("Format jam mulai tidak valid. Harap gunakan format HH:mm.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamSelesai;
            if (!TimeSpan.TryParse(txtJamSelesai.Text, out jamSelesai))
            {
                MessageBox.Show("Format jam selesai tidak valid. Harap gunakan format HH:mm.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (jamMulai >= jamSelesai)
            {
                MessageBox.Show("Jam mulai harus lebih kecil dari jam selesai!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime jamMulaiFull = tanggal.Add(jamMulai);
            DateTime jamSelesaiFull = tanggal.Add(jamSelesai);

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO Reservasi (id_pelanggan, id_paket, id_ruangan, tanggal_reservasi, jam_mulai, jam_selesai) 
                                        VALUES (@Pelanggan, @Paket, @Ruangan, @Tanggal, @Mulai, @Selesai)";

                    cmd.Parameters.AddWithValue("@Pelanggan", txtNamaPelanggan.Text);
                    cmd.Parameters.AddWithValue("@Paket", txtNamaPaket.Text);
                    cmd.Parameters.AddWithValue("@Ruangan", txtNamaRuangan.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", tanggal);
                    cmd.Parameters.AddWithValue("@Mulai", jamMulaiFull);
                    cmd.Parameters.AddWithValue("@Selesai", jamSelesaiFull);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan data reservasi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menyimpan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string query = "SELECT * FROM Reservasi";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvReservasi.DataSource = dt;
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
            txtNamaPelanggan.Clear();
            txtNamaPaket.Clear();
            txtNamaRuangan.Clear();
            txtTanggalReservasi.Clear();
            txtJamMulai.Clear();
            txtJamSelesai.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaPelanggan.Text) ||
                string.IsNullOrWhiteSpace(txtNamaPaket.Text) ||
                string.IsNullOrWhiteSpace(txtNamaRuangan.Text) ||
                string.IsNullOrWhiteSpace(txtTanggalReservasi.Text) ||
                string.IsNullOrWhiteSpace(txtJamMulai.Text) ||
                string.IsNullOrWhiteSpace(txtJamSelesai.Text))
            {
                MessageBox.Show("Harap isi semua data.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggalReservasi;
            if (!DateTime.TryParse(txtTanggalReservasi.Text, out tanggalReservasi))
            {
                MessageBox.Show("Format tanggal tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamMulai;
            if (!TimeSpan.TryParse(txtJamMulai.Text, out jamMulai))
            {
                MessageBox.Show("Format jam mulai tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamSelesai;
            if (!TimeSpan.TryParse(txtJamSelesai.Text, out jamSelesai))
            {
                MessageBox.Show("Format jam selesai tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (jamMulai >= jamSelesai)
            {
                MessageBox.Show("Jam mulai harus lebih kecil dari jam selesai!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data reservasi yang ingin diubah.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idReservasi = dgvReservasi.SelectedRows[0].Cells["id_reservasi"].Value.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Reservasi 
                                     SET id_pelanggan = @Pelanggan, id_paket = @Paket, id_ruangan = @Ruangan, 
                                         tanggal_reservasi = @Tanggal, jam_mulai = @Mulai, jam_selesai = @Selesai 
                                     WHERE id_reservasi = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Pelanggan", txtNamaPelanggan.Text);
                    cmd.Parameters.AddWithValue("@Paket", txtNamaPaket.Text);
                    cmd.Parameters.AddWithValue("@Ruangan", txtNamaRuangan.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", tanggalReservasi);
                    cmd.Parameters.AddWithValue("@Mulai", tanggalReservasi.Add(jamMulai));
                    cmd.Parameters.AddWithValue("@Selesai", tanggalReservasi.Add(jamSelesai));
                    cmd.Parameters.AddWithValue("@Id", idReservasi);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Data reservasi gagal diupdate.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat mengupdate: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data reservasi yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idReservasi = dgvReservasi.SelectedRows[0].Cells["id_reservasi"].Value.ToString();

            DialogResult konfirmasi = MessageBox.Show("Apakah Anda yakin ingin menghapus data reservasi ini?",
                                                        "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Reservasi WHERE id_reservasi = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idReservasi);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Data reservasi gagal dihapus.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menghapus: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form
        }

        private void dgvReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvReservasi.Rows[e.RowIndex];

                txtNamaPelanggan.Text = row.Cells["id_pelanggan"].Value.ToString();
                txtNamaPaket.Text = row.Cells["id_paket"].Value.ToString();
                txtNamaRuangan.Text = row.Cells["id_ruangan"].Value.ToString();
                txtTanggalReservasi.Text = Convert.ToDateTime(row.Cells["tanggal_reservasi"].Value).ToString("yyyy-MM-dd");
                txtJamMulai.Text = Convert.ToDateTime(row.Cells["jam_mulai"].Value).ToString("HH:mm");
                txtJamSelesai.Text = Convert.ToDateTime(row.Cells["jam_selesai"].Value).ToString("HH:mm");
            }
        }
    }
}