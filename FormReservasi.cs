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
using static Org.BouncyCastle.Math.Primes;

namespace SewaRuanganUmy2
{
    public partial class FormReservasi : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        private Form1 _form1;


        public FormReservasi(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1; // ← Ini menyimpan objek Form1
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
            if (cmbPelanggan.SelectedValue == null ||
                cmbPaket.SelectedValue == null ||
                cmbRuangan.SelectedValue == null ||
                cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtJamMulai.Value.TimeOfDay >= txtJamSelesai.Value.TimeOfDay)
            {
                MessageBox.Show("Jam selesai harus setelah jam mulai.", "Validasi Waktu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string query = @"INSERT INTO Reservasi 
                (id_pelanggan, id_paket, id_ruangan, tanggal_reservasi, jam_mulai, jam_selesai, status)
                VALUES (@id_pelanggan, @id_paket, @id_ruangan, @tanggal, @jam_mulai, @jam_selesai, @status)";

                    using (SqlCommand cmd = new SqlCommand(query, con, tran))
                    {
                        cmd.Parameters.Add("@id_pelanggan", SqlDbType.Int).Value = cmbPelanggan.SelectedValue;
                        cmd.Parameters.Add("@id_paket", SqlDbType.Int).Value = cmbPaket.SelectedValue;
                        cmd.Parameters.Add("@id_ruangan", SqlDbType.Int).Value = cmbRuangan.SelectedValue;
                        cmd.Parameters.Add("@tanggal", SqlDbType.Date).Value = dtpTanggalReservasi.Value.Date;
                        cmd.Parameters.Add("@jam_mulai", SqlDbType.Time).Value = txtJamMulai.Value.TimeOfDay;
                        cmd.Parameters.Add("@jam_selesai", SqlDbType.Time).Value = txtJamSelesai.Value.TimeOfDay;
                        cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = cmbStatus.SelectedItem.ToString();

                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    MessageBox.Show("Reservasi berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { /* rollback gagal */ }
                    MessageBox.Show("Gagal menambahkan reservasi:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // --- Pelanggan ---
                    using (SqlCommand cmd = new SqlCommand("SELECT id_pelanggan, nama FROM Pelanggan", conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dictionary<int, string> dicPelanggan = new Dictionary<int, string>();
                        while (reader.Read())
                        {
                            dicPelanggan.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                        cmbPelanggan.DataSource = new BindingSource(dicPelanggan, null);
                        cmbPelanggan.DisplayMember = "Value";  // Nama pelanggan
                        cmbPelanggan.ValueMember = "Key";      // ID pelanggan
                    }

                    // --- Paket ---
                    using (SqlCommand cmd = new SqlCommand("SELECT id_paket, nama_paket FROM Paket", conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dictionary<int, string> dicPaket = new Dictionary<int, string>();
                        while (reader.Read())
                        {
                            dicPaket.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                        cmbPaket.DataSource = new BindingSource(dicPaket, null);
                        cmbPaket.DisplayMember = "Value";  // Nama paket
                        cmbPaket.ValueMember = "Key";      // ID paket
                    }

                    // --- Ruangan ---
                    using (SqlCommand cmd = new SqlCommand("SELECT id_ruangan, nama_ruangan FROM Ruangan", conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dictionary<int, string> dicRuangan = new Dictionary<int, string>();
                        while (reader.Read())
                        {
                            dicRuangan.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                        cmbRuangan.DataSource = new BindingSource(dicRuangan, null);
                        cmbRuangan.DisplayMember = "Value";  // Nama ruangan
                        cmbRuangan.ValueMember = "Key";      // ID ruangan
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Memuat Data: " + ex.Message);
                }
            }
        }



        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_reservasi, id_pelanggan, id_paket, id_ruangan, tanggal_reservasi, jam_mulai, jam_selesai, status FROM Reservasi";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvReservasi.DataSource = dt;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih reservasi yang akan diperbarui.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPelanggan.SelectedValue == null || cmbPaket.SelectedValue == null ||
                cmbRuangan.SelectedValue == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Semua kolom harus diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idReservasi = Convert.ToInt32(dgvReservasi.SelectedRows[0].Cells["id_reservasi"].Value);
            TimeSpan jamMulai = txtJamMulai.Value.TimeOfDay;
            TimeSpan jamSelesai = txtJamSelesai.Value.TimeOfDay;

            if (jamMulai >= jamSelesai)
            {
                MessageBox.Show("Jam selesai harus setelah jam mulai.", "Validasi Waktu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi update
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin memperbarui data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string query = @"UPDATE Reservasi SET 
                id_pelanggan = @id_pelanggan,
                id_paket = @id_paket,
                id_ruangan = @id_ruangan,
                tanggal_reservasi = @tanggal_reservasi,
                jam_mulai = @jam_mulai,
                jam_selesai = @jam_selesai,
                status = @status
                WHERE id_reservasi = @id_reservasi";

                    using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@id_pelanggan", cmbPelanggan.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_paket", cmbPaket.SelectedValue);
                        cmd.Parameters.AddWithValue("@id_ruangan", cmbRuangan.SelectedValue);
                        cmd.Parameters.AddWithValue("@tanggal_reservasi", dtpTanggalReservasi.Value.Date);
                        cmd.Parameters.AddWithValue("@jam_mulai", jamMulai);
                        cmd.Parameters.AddWithValue("@jam_selesai", jamSelesai);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@id_reservasi", idReservasi);

                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    MessageBox.Show("Reservasi berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    MessageBox.Show("Gagal memperbarui reservasi:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string query = "DELETE FROM Reservasi WHERE id_reservasi = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    MessageBox.Show("Data berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearInput();
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    MessageBox.Show("Gagal menghapus data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            _form1.Show();   // Tampilkan kembali Form1
            this.Close();    // Tutup FormReservasi
        }



        private void dgvReservasi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Pastikan baris yang dipilih bukan header (e.RowIndex > -1)
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvReservasi.Rows[e.RowIndex];
                    // Ambil data dari sel yang dipilih berdasarkan kolom
                    int idReservasi = Convert.ToInt32(dgvReservasi.Rows[e.RowIndex].Cells["id_reservasi"].Value);
                    int idPelanggan = Convert.ToInt32(dgvReservasi.Rows[e.RowIndex].Cells["id_pelanggan"].Value);
                    int idPaket = Convert.ToInt32(dgvReservasi.Rows[e.RowIndex].Cells["id_paket"].Value);
                    int idRuangan = Convert.ToInt32(dgvReservasi.Rows[e.RowIndex].Cells["id_ruangan"].Value);
                    DateTime tanggal = Convert.ToDateTime(dgvReservasi.Rows[e.RowIndex].Cells["tanggal_reservasi"].Value);
                    TimeSpan jamMulai = (TimeSpan)dgvReservasi.Rows[e.RowIndex].Cells["jam_mulai"].Value;
                    TimeSpan jamSelesai = (TimeSpan)dgvReservasi.Rows[e.RowIndex].Cells["jam_selesai"].Value;
                    string status = row.Cells["status"].Value.ToString();
                    

                    // Tampilkan ke form (misal pada TextBox atau ComboBox)
                    cmbPelanggan.SelectedValue = idPelanggan;
                    cmbPaket.SelectedValue = idPaket;
                    cmbRuangan.SelectedValue = idRuangan;
                    dtpTanggalReservasi.Value = tanggal;
                    txtJamMulai.Value = DateTime.Today.Add(jamMulai);
                    txtJamSelesai.Value = DateTime.Today.Add(jamSelesai);
                    cmbStatus.SelectedItem = status;

                    // Menyimpan ID untuk referensi pengeditan atau penghapusan
                    // currentIdReservasi = idReservasi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void txtJamSelesai_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            DataTable dtPelanggan = new DataTable();
            DataTable dtReservasi = new DataTable();
            string statistikInfo = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Tangkap pesan InfoMessage dari SQL Server (STATISTICS INFO)
                conn.InfoMessage += (s, e2) =>
                {
                    statistikInfo += e2.Message + Environment.NewLine;
                };

                conn.Open();

                // Aktifkan STATISTICS TIME dan IO agar SQL Server kirim info statistik
                using (SqlCommand cmdStat = new SqlCommand("SET STATISTICS TIME ON; SET STATISTICS IO ON;", conn))
                {
                    cmdStat.ExecuteNonQuery();
                }

                // Ambil data Pelanggan
                using (SqlCommand cmd = new SqlCommand("SELECT id_pelanggan, nama FROM Pelanggan", conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dtPelanggan);
                }

                // Ambil data Reservasi
                using (SqlCommand cmd = new SqlCommand("SELECT id_pelanggan FROM Reservasi", conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dtReservasi);
                }
            }

            // Proses analisis: hitung jumlah reservasi per pelanggan
            var analisis = from p in dtPelanggan.AsEnumerable()
                           join r in dtReservasi.AsEnumerable()
                           on p.Field<int>("id_pelanggan") equals r.Field<int>("id_pelanggan") into joinReservasi
                           select new
                           {
                               NamaPelanggan = p.Field<string>("nama"),
                               JumlahReservasi = joinReservasi.Count()
                           };

            // Urutkan berdasarkan jumlah reservasi terbanyak, ambil top 10 saja
            var top10 = analisis.OrderByDescending(a => a.JumlahReservasi).Take(10);

            // Bangun string hasil analisis
            StringBuilder hasilAnalisis = new StringBuilder();
            hasilAnalisis.AppendLine("Top 10 Pelanggan berdasarkan jumlah reservasi:");
            hasilAnalisis.AppendLine("----------------------------------------------");
            foreach (var item in top10)
            {
                hasilAnalisis.AppendLine($"{item.NamaPelanggan}: {item.JumlahReservasi} reservasi");
            }
            hasilAnalisis.AppendLine();
            hasilAnalisis.AppendLine("=== STATISTICS INFO ===");
            hasilAnalisis.AppendLine(statistikInfo);

            // Tampilkan di MessageBox
            MessageBox.Show(hasilAnalisis.ToString(), "Hasil Analisis Reservasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }







        private void dgvReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}