using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using System.Data;
using System.IO;





namespace SewaRuanganUmy2
{
    public partial class FormPaket : Form
    {
        private string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        private Form1 _form1;

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

            // Validasi nama paket hanya boleh huruf, angka, dan spasi
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPaket.Text.Trim(), @"[^A-Za-z0-9\s]"))
            {
                MessageBox.Show("Nama paket hanya boleh mengandung huruf, angka, dan spasi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi harga
            if (!decimal.TryParse(txtHarga.Text.Trim(), out decimal hargaPaket))
            {
                MessageBox.Show("Format harga tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi harga tidak boleh lebih kecil dari 1000
            if (hargaPaket < 500000)
            {
                MessageBox.Show("Harga paket tidak boleh kurang dari 500000 rupiah!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Menyimpan data paket ke database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertPaket", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nama_paket", txtPaket.Text.Trim());
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga", hargaPaket);

                        int result = cmd.ExecuteNonQuery();
                        // Menambahkan parameter untuk mencegah SQL Injection

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
                    
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601) // Pelanggaran UNIQUE constraint
                    {
                        MessageBox.Show("Nama paket sudah digunakan. Harap gunakan nama lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan Paket: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

                // Validasi nama paket hanya boleh huruf, angka, dan spasi
                if (System.Text.RegularExpressions.Regex.IsMatch(txtPaket.Text.Trim(), @"[^A-Za-z0-9\s]"))
                {
                    MessageBox.Show("Nama paket hanya boleh mengandung huruf, angka, dan spasi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validasi harga
                decimal harga;
                if (!decimal.TryParse(txtHarga.Text.Trim(), out harga))
                {
                    MessageBox.Show("Format harga tidak valid! Harap masukkan angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validasi harga tidak boleh kurang dari 1000
                if (harga < 1000)
                {
                    MessageBox.Show("Harga paket tidak boleh kurang dari 1000 rupiah!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Konfirmasi update
                DialogResult konfirmasi = MessageBox.Show("Apakah Anda yakin ingin mengupdate data paket ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (konfirmasi != DialogResult.Yes)
                {
                    return; // Batalkan jika pengguna memilih "No"
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_UpdatePaket", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Tambahkan parameter ke stored procedure
                            cmd.Parameters.AddWithValue("@id_paket", Convert.ToInt32(id));
                            cmd.Parameters.AddWithValue("@nama_paket", txtPaket.Text.Trim());
                            cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                            cmd.Parameters.AddWithValue("@harga", harga);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Paket berhasil diubah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData(); // Refresh data
                            }
                            else
                            {
                                MessageBox.Show("Paket gagal diubah.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            MessageBox.Show("Nama paket sudah digunakan oleh paket lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Terjadi kesalahan saat mengubah paket: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
                        using (SqlCommand cmd = new SqlCommand("sp_DeletePaket", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Tambahkan parameter untuk stored procedure
                            cmd.Parameters.AddWithValue("@id_paket", Convert.ToInt32(id));

                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Paket berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData(); // Refresh tampilan
                            }
                            else
                            {
                                MessageBox.Show("Paket gagal dihapus. Data mungkin tidak ditemukan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (SqlException ex)
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

        private void ImportPaketFromExcel(string filePath)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable dt = result.Tables[0];

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        foreach (DataRow row in dt.Rows)
                        {
                            if (!ValidateRow(row))
                                continue;

                            using (SqlCommand cmd = new SqlCommand("sp_InsertPaket", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@nama_paket", row["nama_paket"].ToString());
                                cmd.Parameters.AddWithValue("@deskripsi", row["deskripsi"].ToString());
                                cmd.Parameters.AddWithValue("@harga", Convert.ToDecimal(row["harga"]));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("Data paket berhasil diimpor menggunakan stored procedure!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengimpor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateRow(DataRow row)
        {
            return !string.IsNullOrWhiteSpace(row["nama_paket"]?.ToString()) &&
                   !string.IsNullOrWhiteSpace(row["deskripsi"]?.ToString()) &&
                   decimal.TryParse(row["harga"]?.ToString(), out decimal harga) &&
                   harga >= 1000;
        }




        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ImportPaketFromExcel(filePath);
            }
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

        private void dgvPaket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
