using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace SewaRuanganUmy2
{
    internal class Koneksi
    {
        private static readonly string connectionString = "Data Source=YUUTA\\YUUTA;Initial Catalog=SewaRuanganUMY;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }   

        public static string GetConnectionString()
        {
            return connectionString;
        }

        // Method tambahan untuk ambil IP lokal jika butuh
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Tidak ada alamat IP lokal (IPv4) yang ditemukan.");
        }
    }
}
