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
    public partial class FormRuangan : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";
        public FormRuangan()
        {
            InitializeComponent();
            LoadData();
        }

        private void FormRuangan_Load(object sender, EventArgs e)
        {

        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaRuangan.Text) ||
                string.IsNullOrWhiteSpace(txtKapasitas.Text) ||
                string.IsNullOrWhiteSpace(txtLokasi.Text))
            {
                MessageBox.Show("Harap isi semua data ruangan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi kapasitas
            int kapasitas;
            if (!int.TryParse(txtKapasitas.Text.Trim(), out kapasitas) || kapasitas <= 0)
            {
                MessageBox.Show("Kapasitas tidak valid! Harus berupa angka dan lebih besar dari 0.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Menyimpan data ruangan ke database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ruangan (nama_ruangan, kapasitas, lokasi ) VALUES (@Nama, @Kapasitas, @Lokasi)";

                    // Menambahkan parameter untuk mencegah SQL Injection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nama", txtNamaRuangan.Text.Trim());
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.Parameters.AddWithValue("@Lokasi", txtLokasi.Text.Trim());             

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data ruangan berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();  // Memastikan fungsi LoadData() ada untuk memperbarui tampilan data ruangan
                        }
                        else
                        {
                            MessageBox.Show("Data ruangan gagal disimpan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menyimpan data ruangan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string query = "SELECT * FROM Ruangan";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvRuangan.DataSource = dt;
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
            txtNamaRuangan.Clear();
            txtKapasitas.Clear();
            txtLokasi.Clear();
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvRuangan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data ruangan yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idRuangan = dgvRuangan.SelectedRows[0].Cells["id_ruangan"].Value.ToString();

            DialogResult konfirmasi = MessageBox.Show("Apakah Anda yakin ingin menghapus data ruangan ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM Ruangan WHERE id_ruangan = @id";
                    cmd.Parameters.AddWithValue("@id", idRuangan);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data ruangan berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Pastikan fungsi LoadData() merefresh data ruangan
                    }
                    else
                    {
                        MessageBox.Show("Data ruangan gagal dihapus.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menghapus data ruangan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRuangan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data ruangan yang ingin diubah.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idRuangan = dgvRuangan.SelectedRows[0].Cells["id_ruangan"].Value.ToString();

            // Validasi input
            if (string.IsNullOrWhiteSpace(txtNamaRuangan.Text) ||
                string.IsNullOrWhiteSpace(txtKapasitas.Text) ||
                string.IsNullOrWhiteSpace(txtLokasi.Text))
                
            {
                MessageBox.Show("Harap isi semua data ruangan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi kapasitas
            if (!int.TryParse(txtKapasitas.Text.Trim(), out int kapasitas) || kapasitas <= 0)
            {
                MessageBox.Show("Kapasitas tidak valid! Harus berupa angka dan lebih dari 0.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = @"UPDATE Ruangan 
                                SET nama_ruangan = @Nama, kapasitas = @Kapasitas, lokasi = @Lokasi 
                                WHERE id_ruangan = @Id";

                    cmd.Parameters.AddWithValue("@Nama", txtNamaRuangan.Text.Trim());
                    cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                    cmd.Parameters.AddWithValue("@Lokasi", txtLokasi.Text.Trim());
                   
                    cmd.Parameters.AddWithValue("@Id", idRuangan);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data ruangan berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Pastikan LoadData merefresh isi DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Data ruangan gagal diupdate.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat update data ruangan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvRuangan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRuangan.Rows[e.RowIndex];
                txtNamaRuangan.Text = row.Cells["nama_ruangan"].Value.ToString();
                txtKapasitas.Text = row.Cells["kapasitas"].Value.ToString();
                txtLokasi.Text = row.Cells["lokasi"].Value.ToString();
                
            }
        }
    }
}
