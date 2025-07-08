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

            if (txtJamMulai.Value.TimeOfDay >= txtJamSelesai.Value.TimeOfDay)
            {
                MessageBox.Show("Jam selesai harus setelah jam mulai.", "Validasi Jam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertReservasi", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pelanggan", cmbPelanggan.SelectedValue);
                cmd.Parameters.AddWithValue("@id_paket", cmbPaket.SelectedValue);
                cmd.Parameters.AddWithValue("@id_ruangan", cmbRuangan.SelectedValue);
                cmd.Parameters.AddWithValue("@tanggal_reservasi", tanggalReservasi); // sesuai nama param SP
                cmd.Parameters.AddWithValue("@jam_mulai", txtJamMulai.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@jam_selesai", txtJamSelesai.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil ditambahkan.");
                        LoadData(); // refresh data grid atau lainnya
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
                    // Urutkan berdasarkan tanggal agar index 'idx_Reservasi_tanggal' bisa dimanfaatkan
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
            DateTime batasMaksimal = DateTime.Today.AddDays(3);

            if (tanggalReservasi > batasMaksimal)
            {
                MessageBox.Show("Tanggal reservasi tidak boleh lebih dari 3 hari dari hari ini.", "Validasi Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtJamMulai.Value.TimeOfDay >= txtJamSelesai.Value.TimeOfDay)
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
                cmd.Parameters.AddWithValue("@tanggal", tanggalReservasi);
                cmd.Parameters.AddWithValue("@jam_mulai", txtJamMulai.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@jam_selesai", txtJamSelesai.Value.TimeOfDay);
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
                {
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
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvReservasi.Rows[e.RowIndex];

                    // Ambil data nama dari kolom DataGridView
                    string namaPelanggan = row.Cells["nama_pelanggan"].Value.ToString();
                    string namaPaket = row.Cells["nama_paket"].Value.ToString();
                    string namaRuangan = row.Cells["nama_ruangan"].Value.ToString();
                    DateTime tanggal = Convert.ToDateTime(row.Cells["tanggal_reservasi"].Value);
                    TimeSpan jamMulai = (TimeSpan)row.Cells["jam_mulai"].Value;
                    TimeSpan jamSelesai = (TimeSpan)row.Cells["jam_selesai"].Value;
                    string status = row.Cells["status"].Value.ToString();

                    // Set SelectedValue berdasarkan display text di ComboBox
                    cmbPelanggan.SelectedIndex = cmbPelanggan.FindStringExact(namaPelanggan);
                    cmbPaket.SelectedIndex = cmbPaket.FindStringExact(namaPaket);
                    cmbRuangan.SelectedIndex = cmbRuangan.FindStringExact(namaRuangan);

                    dtpTanggalReservasi.Value = tanggal;
                    txtJamMulai.Value = DateTime.Today.Add(jamMulai);
                    txtJamSelesai.Value = DateTime.Today.Add(jamSelesai);
                    cmbStatus.SelectedItem = status;

                    // Jika kamu masih punya kolom id_reservasi, bisa simpan ID-nya
                    // currentIdReservasi = Convert.ToInt32(row.Cells["id_reservasi"].Value);
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