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
using SewaRuanganUmy2;

namespace SewaRuanganUmy2
{
    public partial class FormReservasi : Form
    {
        private string connectionString = Koneksi.GetConnectionString();
        private Form1 _form1;

        public FormReservasi(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
        }

        private void FormReservasi_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBox();

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Dikonfirmasi");
            cmbStatus.Items.Add("Dibatalkan");
            cmbStatus.SelectedIndex = 0;
        }

        private void btnTambahkan_Click(object sender, EventArgs e)
        {
            if (cmbPelanggan.SelectedValue == null || cmbPaket.SelectedValue == null || cmbRuangan.SelectedValue == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggalReservasi = dtpTanggalReservasi.Value.Date;
            DateTime minimalTanggal = DateTime.Today.AddDays(3);

            if (tanggalReservasi < minimalTanggal)
            {
                MessageBox.Show("Tanggal reservasi harus minimal 3 hari dari hari ini.", "Validasi Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamMulai = new TimeSpan(txtJamMulai.Value.Hour, txtJamMulai.Value.Minute, txtJamMulai.Value.Second);
            TimeSpan jamSelesai = new TimeSpan(txtJamSelesai.Value.Hour, txtJamSelesai.Value.Minute, txtJamSelesai.Value.Second);

            if (jamMulai >= jamSelesai)
            {
                MessageBox.Show("Jam selesai harus setelah jam mulai.", "Validasi Jam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand checkCmd = new SqlCommand(@"
                    SELECT COUNT(*) FROM Reservasi
                    WHERE id_ruangan = @id_ruangan
                    AND tanggal_reservasi = @tanggal
                    AND (
                        (@jam_mulai BETWEEN jam_mulai AND jam_selesai)
                        OR (@jam_selesai BETWEEN jam_mulai AND jam_selesai)
                        OR (jam_mulai BETWEEN @jam_mulai AND @jam_selesai)
                    )
                ", conn))
                {
                    checkCmd.Parameters.AddWithValue("@id_ruangan", cmbRuangan.SelectedValue);
                    checkCmd.Parameters.AddWithValue("@tanggal", tanggalReservasi);
                    checkCmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                    checkCmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Ruangan sudah terisi pada waktu tersebut!", "Jadwal Bentrok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using (SqlCommand cmd = new SqlCommand("sp_InsertReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", cmbPelanggan.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_paket", cmbPaket.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_ruangan", cmbRuangan.SelectedValue);
                    cmd.Parameters.AddWithValue("@tanggal_reservasi", tanggalReservasi);
                    cmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                    cmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data reservasi berhasil ditambahkan.");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menambahkan data.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("UNIQUE KEY") || ex.Message.Contains("duplicate"))
                        {
                            MessageBox.Show("Reservasi gagal: data ruangan/paket mungkin sudah terpakai pada waktu tersebut.", "Error Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Kesalahan database: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kesalahan sistem: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data reservasi yang ingin diubah.");
                return;
            }

            string idReservasi = dgvReservasi.SelectedRows[0].Cells["id_reservasi"].Value.ToString();

            if (cmbPelanggan.SelectedValue == null || cmbPaket.SelectedValue == null || cmbRuangan.SelectedValue == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggalReservasi = dtpTanggalReservasi.Value.Date;
            DateTime batasMinimal = DateTime.Today.AddDays(3);

            if (tanggalReservasi < batasMinimal)
            {
                MessageBox.Show("Tanggal reservasi harus minimal 3 hari dari hari ini.", "Validasi Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan jamMulai = new TimeSpan(txtJamMulai.Value.Hour, txtJamMulai.Value.Minute, txtJamMulai.Value.Second);
            TimeSpan jamSelesai = new TimeSpan(txtJamSelesai.Value.Hour, txtJamSelesai.Value.Minute, txtJamSelesai.Value.Second);

            if (jamMulai >= jamSelesai)
            {
                MessageBox.Show("Jam selesai harus setelah jam mulai.", "Validasi Waktu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin mengubah data ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateReservasi", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reservasi", idReservasi);
                cmd.Parameters.AddWithValue("@id_pelanggan", cmbPelanggan.SelectedValue);
                cmd.Parameters.AddWithValue("@id_paket", cmbPaket.SelectedValue);
                cmd.Parameters.AddWithValue("@id_ruangan", cmbRuangan.SelectedValue);
                cmd.Parameters.AddWithValue("@tanggal_reservasi", tanggalReservasi);
                cmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                cmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);
                cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil diperbarui.");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal memperbarui data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvReservasi.CurrentRow == null || dgvReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvReservasi.CurrentRow.Cells["id_reservasi"].Value);

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteReservasi", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reservasi", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInput();
            }
        }

        private void LoadComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Pelanggan
                using (SqlCommand cmd = new SqlCommand("SELECT id_pelanggan, nama FROM Pelanggan", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<int, string> dicPelanggan = new Dictionary<int, string>();
                    while (reader.Read())
                        dicPelanggan.Add(reader.GetInt32(0), reader.GetString(1));
                    cmbPelanggan.DataSource = new BindingSource(dicPelanggan, null);
                    cmbPelanggan.DisplayMember = "Value";
                    cmbPelanggan.ValueMember = "Key";
                }

                // Paket
                using (SqlCommand cmd = new SqlCommand("SELECT id_paket, nama_paket FROM Paket", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<int, string> dicPaket = new Dictionary<int, string>();
                    while (reader.Read())
                        dicPaket.Add(reader.GetInt32(0), reader.GetString(1));
                    cmbPaket.DataSource = new BindingSource(dicPaket, null);
                    cmbPaket.DisplayMember = "Value";
                    cmbPaket.ValueMember = "Key";
                }

                // Ruangan
                using (SqlCommand cmd = new SqlCommand("SELECT id_ruangan, nama_ruangan FROM Ruangan", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<int, string> dicRuangan = new Dictionary<int, string>();
                    while (reader.Read())
                        dicRuangan.Add(reader.GetInt32(0), reader.GetString(1));
                    cmbRuangan.DataSource = new BindingSource(dicRuangan, null);
                    cmbRuangan.DisplayMember = "Value";
                    cmbRuangan.ValueMember = "Key";
                }
            }
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT 
                    r.id_reservasi,
                    p.nama AS nama_pelanggan,
                    pk.nama_paket,
                    ru.nama_ruangan,
                    r.tanggal_reservasi,
                    r.jam_mulai,
                    r.jam_selesai,
                    r.status
                FROM Reservasi r
                JOIN Pelanggan p ON r.id_pelanggan = p.id_pelanggan
                JOIN Paket pk ON r.id_paket = pk.id_paket
                JOIN Ruangan ru ON r.id_ruangan = ru.id_ruangan
                ORDER BY r.id_reservasi DESC;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvReservasi.DataSource = dt;
                }
            }
        }

        private void ClearInput()
        {
            dtpTanggalReservasi.Value = DateTime.Today;
            txtJamMulai.Value = DateTime.Now;
            txtJamSelesai.Value = DateTime.Now.AddHours(1);
            cmbPelanggan.SelectedIndex = -1;
            cmbPaket.SelectedIndex = -1;
            cmbRuangan.SelectedIndex = -1;
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            _form1.Show();
            this.Close();
        }

        private void dgvReservasi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvReservasi.Rows[e.RowIndex];
                    cmbPelanggan.SelectedIndex = cmbPelanggan.FindStringExact(row.Cells["nama_pelanggan"].Value.ToString());
                    cmbPaket.SelectedIndex = cmbPaket.FindStringExact(row.Cells["nama_paket"].Value.ToString());
                    cmbRuangan.SelectedIndex = cmbRuangan.FindStringExact(row.Cells["nama_ruangan"].Value.ToString());
                    dtpTanggalReservasi.Value = Convert.ToDateTime(row.Cells["tanggal_reservasi"].Value);
                    txtJamMulai.Value = DateTime.Today.Add((TimeSpan)row.Cells["jam_mulai"].Value);
                    txtJamSelesai.Value = DateTime.Today.Add((TimeSpan)row.Cells["jam_selesai"].Value);
                    cmbStatus.SelectedItem = row.Cells["status"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            ShowReservasiAnalysis();
        }


        private void ShowReservasiAnalysis()
        {
            StringBuilder result = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Analisis berdasarkan status
                string queryStatus = "SELECT status, COUNT(*) AS jumlah FROM Reservasi GROUP BY status";
                using (SqlCommand cmd = new SqlCommand(queryStatus, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    result.AppendLine("📊 Jumlah Reservasi Berdasarkan Status:");
                    while (reader.Read())
                    {
                        string status = reader["status"].ToString();
                        int jumlah = Convert.ToInt32(reader["jumlah"]);
                        result.AppendLine($"- {status}: {jumlah} reservasi");
                    }
                }

                result.AppendLine(); // baris kosong

                // 2. Analisis berdasarkan ruangan
                string queryRuangan = @"
            SELECT ru.nama_ruangan, COUNT(*) AS jumlah
            FROM Reservasi r
            JOIN Ruangan ru ON r.id_ruangan = ru.id_ruangan
            GROUP BY ru.nama_ruangan
            ORDER BY jumlah DESC";
                using (SqlCommand cmd = new SqlCommand(queryRuangan, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    result.AppendLine("🏢 Jumlah Reservasi per Ruangan:");
                    while (reader.Read())
                    {
                        string ruangan = reader["nama_ruangan"].ToString();
                        int jumlah = Convert.ToInt32(reader["jumlah"]);
                        result.AppendLine($"- {ruangan}: {jumlah} reservasi");
                    }
                }
            }

            MessageBox.Show(result.ToString(), "Analisis Reservasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void dgvReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kosongkan kalau tidak ada aksi khusus
        }
        private void txtJamSelesai_ValueChanged(object sender, EventArgs e)
        {
            // Bisa dikosongkan jika belum digunakan
        }

    }
}
