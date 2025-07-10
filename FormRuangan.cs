using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SewaRuanganUmy2
{
    public partial class FormRuangan : Form
    {
        private string connectionString;

        public FormRuangan()
        {
            InitializeComponent();
            connectionString = Koneksi.GetConnectionString();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("sp_GetAllRuangan", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

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

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaRuangan.Text) ||
                string.IsNullOrWhiteSpace(txtKapasitas.Text) ||
                string.IsNullOrWhiteSpace(txtLokasi.Text))
            {
                MessageBox.Show("Harap isi semua data ruangan!");
                return;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(txtNamaRuangan.Text.Trim(), @"[^A-Za-z0-9\s]"))
            {
                MessageBox.Show("Nama ruangan hanya boleh huruf, angka, dan spasi.");
                return;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(txtLokasi.Text.Trim(), @"[^A-Za-z0-9\s.-]"))
            {
                MessageBox.Show("Lokasi hanya boleh huruf, angka, titik, dan tanda hubung.");
                return;
            }

            int kapasitas;
            if (!int.TryParse(txtKapasitas.Text.Trim(), out kapasitas) || kapasitas <= 0)
            {
                MessageBox.Show("Kapasitas harus berupa angka positif.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertRuangan", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nama_ruangan", txtNamaRuangan.Text.Trim());
                cmd.Parameters.AddWithValue("@kapasitas", kapasitas);
                cmd.Parameters.AddWithValue("@lokasi", txtLokasi.Text.Trim());

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil disimpan.");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan data.");
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601) // duplikat key
                    {
                        MessageBox.Show("Nama ruangan sudah ada. Gunakan nama lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Kesalahan SQL: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message);
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRuangan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data ruangan yang ingin diubah.");
                return;
            }

            string idRuangan = dgvRuangan.SelectedRows[0].Cells["id_ruangan"].Value.ToString();

            if (string.IsNullOrWhiteSpace(txtNamaRuangan.Text) ||
                string.IsNullOrWhiteSpace(txtKapasitas.Text) ||
                string.IsNullOrWhiteSpace(txtLokasi.Text))
            {
                MessageBox.Show("Harap isi semua data ruangan!");
                return;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(txtNamaRuangan.Text.Trim(), @"[^A-Za-z0-9\s]"))
            {
                MessageBox.Show("Nama ruangan hanya boleh huruf, angka, dan spasi.");
                return;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(txtLokasi.Text.Trim(), @"[^A-Za-z0-9\s.-]"))
            {
                MessageBox.Show("Lokasi hanya boleh huruf, angka, titik, dan tanda hubung.");
                return;
            }

            int kapasitas;
            if (!int.TryParse(txtKapasitas.Text.Trim(), out kapasitas) || kapasitas <= 0 || kapasitas > 999)
            {
                MessageBox.Show("Kapasitas harus angka 1–999.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin mengubah data ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateRuangan", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_ruangan", idRuangan);
                cmd.Parameters.AddWithValue("@nama_ruangan", txtNamaRuangan.Text.Trim());
                cmd.Parameters.AddWithValue("@kapasitas", kapasitas);
                cmd.Parameters.AddWithValue("@lokasi", txtLokasi.Text.Trim());

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil diupdate.");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal update data.");
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MessageBox.Show("Nama ruangan sudah digunakan oleh ruangan lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Kesalahan SQL: " + ex.Message);
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
            if (dgvRuangan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus.");
                return;
            }

            string idRuangan = dgvRuangan.SelectedRows[0].Cells["id_ruangan"].Value.ToString();

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteRuangan", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_ruangan", idRuangan);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus.");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menghapus data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message);
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

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormRuangan_Load(object sender, EventArgs e)
        {
        }
    }
}
