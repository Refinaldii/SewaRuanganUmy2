﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExcelDataReader;

namespace SewaRuanganUmy2
{
    public partial class FormPelanggan : Form
    {
        private string connectionString;
        private Form1 mainForm;

        public FormPelanggan(Form1 formUtama)
        {
            InitializeComponent();
            mainForm = formUtama;
            connectionString = mainForm.GetConnectionString();
        }

        private void FormPelanggan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("sp_GetAllPelanggan", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPelanggan.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data pelanggan: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            txtNamaPelanggan.Clear();
            txtNoTelp.Clear();
            txtEmail.Clear();
            txtAlamat.Clear();
        }

        private bool ValidasiInput()
        {
            if (string.IsNullOrWhiteSpace(txtNamaPelanggan.Text) ||
                string.IsNullOrWhiteSpace(txtNoTelp.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Harap isi semua data pelanggan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (Regex.IsMatch(txtNamaPelanggan.Text.Trim(), @"[^A-Za-z\s]"))
            {
                MessageBox.Show("Nama pelanggan hanya boleh mengandung huruf dan spasi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Format email tidak valid!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!long.TryParse(txtNoTelp.Text.Trim(), out long telp) || txtNoTelp.Text.Trim().Length < 10)
            {
                MessageBox.Show("Nomor telepon tidak valid! Harus berupa angka dan minimal 10 digit.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput()) return;

            string noHP = txtNoTelp.Text.Trim();
            if (noHP.Length > 13)
            {
                MessageBox.Show("Nomor HP tidak boleh lebih dari 13 digit.", "Validasi Nomor HP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertPelanggan", conn))
            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nama", txtNamaPelanggan.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", noHP);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data pelanggan berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan data pelanggan.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MessageBox.Show("Email atau No HP sudah digunakan oleh pelanggan lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Kesalahan saat menyimpan data: " + ex.Message);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPelanggan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih data pelanggan yang ingin diubah.");
                return;
            }

            if (!ValidasiInput()) return;

            int id = Convert.ToInt32(dgvPelanggan.SelectedRows[0].Cells["id_pelanggan"].Value);

            DialogResult konfirmasi = MessageBox.Show("Yakin update data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdatePelanggan", conn))
            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", id);
                    cmd.Parameters.AddWithValue("@nama", txtNamaPelanggan.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data pelanggan berhasil diupdate.");
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data pelanggan gagal diupdate.");
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MessageBox.Show("Email atau No HP sudah digunakan oleh pelanggan lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Kesalahan saat menyimpan data: " + ex.Message);
                    }
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvPelanggan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus.");
                return;
            }

            int id = Convert.ToInt32(dgvPelanggan.SelectedRows[0].Cells["id_pelanggan"].Value);

            DialogResult konfirmasi = MessageBox.Show("Yakin hapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (konfirmasi != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeletePelanggan", conn))
            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", id);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus.");
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menghapus data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menghapus: " + ex.Message);
                }
            }
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("AnalisisPelanggan", conn))
            {
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int totalPelanggan = reader.GetInt32(0);
                            int emailUnik = reader.GetInt32(1);
                            int noHpInvalid = reader.GetInt32(2);

                            MessageBox.Show(
                                $"Total Pelanggan : {totalPelanggan}\n" +
                                $"Email Unik       : {emailUnik}\n" +
                                $"No HP Tidak Valid: {noHpInvalid}",
                                "Analisis Pelanggan",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal analisis: " + ex.Message);
                }
            }
        }

        private void dgvPelanggan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPelanggan.Rows[e.RowIndex];
                txtNamaPelanggan.Text = row.Cells["nama"].Value.ToString();
                txtNoTelp.Text = row.Cells["no_hp"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtAlamat.Text = row.Cells["alamat"].Value.ToString();
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                Title = "Pilih File Excel"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });

                        DataTable dt = result.Tables[0];
                        dgvPelanggan.DataSource = dt;

                        DialogResult konfirmasi = MessageBox.Show("Apakah ingin menyimpan semua data ini ke database?", "Konfirmasi Import", MessageBoxButtons.YesNo);
                        if (konfirmasi == DialogResult.Yes)
                        {
                            ImportDataToDatabase(dt);
                        }
                    }
                }
            }
        }

        private void ImportDataToDatabase(DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string nama = row["nama"].ToString().Trim();
                        string no_hp = row["no_hp"].ToString().Trim();
                        string email = row["email"].ToString().Trim();
                        string alamat = row["alamat"].ToString().Trim();

                        if (string.IsNullOrWhiteSpace(nama) || string.IsNullOrWhiteSpace(no_hp) ||
                            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(alamat))
                            continue;

                        using (SqlCommand cmd = new SqlCommand("sp_InsertPelanggan", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@nama", nama);
                            cmd.Parameters.AddWithValue("@no_hp", no_hp);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@alamat", alamat);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan salah satu data: " + ex.Message);
                    }
                }

                MessageBox.Show("Import selesai.");
                LoadData();
            }
        }
    }
}
