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
    public partial class FormPaket : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        public FormPaket()
        {
            InitializeComponent();
            LoadData();

        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Paket";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPaket.DataSource = dt;
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
            txtPaket.Clear();
            txtDeskripsi.Clear();
            txtHarga.Clear();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(txtPaket.Text) ||
                string.IsNullOrWhiteSpace(txtDeskripsi.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("Harap isi semua data paket!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi harga
            if (!decimal.TryParse(txtHarga.Text.Trim(), out decimal hargaPaket))
            {
                MessageBox.Show("Format harga tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO Paket (nama_paket, deskripsi, harga) VALUES (@Nama, @Deskripsi, @Harga)";

                    cmd.Parameters.AddWithValue("@Nama", txtPaket.Text.Trim());
                    cmd.Parameters.AddWithValue("@Deskripsi", txtDeskripsi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Harga", hargaPaket);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Paket berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Pastikan LoadData() sesuai dengan grid atau tampilan yang kamu gunakan
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan Paket. Tidak ada data yang disimpan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambahkan Paket: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPaket.SelectedRows.Count > 0)
            {
                // Mendapatkan ID paket dari baris yang dipilih
                string id = dgvPaket.SelectedRows[0].Cells["id_paket"].Value.ToString();

                // Validasi input
                if (string.IsNullOrWhiteSpace(txtPaket.Text) ||
                    string.IsNullOrWhiteSpace(txtDeskripsi.Text) ||
                    string.IsNullOrWhiteSpace(txtHarga.Text))
                {
                    MessageBox.Show("Harap isi semua data paket!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validasi harga
                decimal harga;
                if (!decimal.TryParse(txtHarga.Text.Trim(), out harga))
                {
                    MessageBox.Show("Format harga tidak valid! Harap masukkan angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE Paket SET nama_paket=@Nama, deskripsi=@Deskripsi, harga=@Harga WHERE id_paket=@Id";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Menambahkan parameter ke query
                        cmd.Parameters.AddWithValue("@Nama", txtPaket.Text.Trim());
                        cmd.Parameters.AddWithValue("@Deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@Harga", harga);
                        cmd.Parameters.AddWithValue("@Id", id);

                        // Mengeksekusi query
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Paket berhasil diubah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();  // Pastikan fungsi LoadData() sudah didefinisikan untuk memperbarui tampilan data
                        }
                        else
                        {
                            MessageBox.Show("Paket gagal diubah.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan saat mengubah paket: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih baris paket yang ingin diubah.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvPaket.SelectedRows.Count > 0)
            {
                string id = dgvPaket.SelectedRows[0].Cells["id_paket"].Value.ToString();

                // Konfirmasi penghapusan
                DialogResult konfirmasi = MessageBox.Show("Apakah Anda yakin ingin menghapus paket ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (konfirmasi != DialogResult.Yes)
                    return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Paket WHERE id_paket=@Id";

                        // Menggunakan 'using' untuk SqlCommand untuk otomatis membersihkan objek setelah selesai digunakan
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Paket berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();  // Pastikan LoadData() sudah ada untuk memperbarui tampilan data
                            }
                            else
                            {
                                MessageBox.Show("Paket gagal dihapus. Mungkin data sudah tidak ada.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan saat menghapus paket: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih paket yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close(); // Tutup form
        }


        private void FormPaket_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvPaket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPaket.Rows[e.RowIndex];
                txtPaket.Text = row.Cells["nama_paket"].Value.ToString();
                txtDeskripsi.Text = row.Cells["deskripsi"].Value.ToString();
                txtHarga.Text = row.Cells["harga"].Value.ToString();
            }
        }
    }
}
