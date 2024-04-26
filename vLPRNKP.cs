using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vLPRNKP
    {
        public void Main()
        {
            vLPRNKP pr = new vLPRNKP();
            Program prg = new Program();
            while (true)
            {
                try
                {
                    SqlConnection conn = null;
                    string strKoneksi = "Data source = DESKTOP-CKQPUM7\\JODINURF;" +
                        "Initial catalog = KelolaDataKP;User ID = sa; Password = Jodinurf12";
                    conn = new SqlConnection(strKoneksi);
                    conn.Open();
                    Console.Clear();
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\nMenu Kelola Data Laporan KP");
                            Console.WriteLine("1. Melihat Seluruh Data");
                            Console.WriteLine("2. Tambah Data");
                            Console.WriteLine("3. Hapus Data");
                            Console.WriteLine("4. Edit Data");
                            Console.WriteLine("5. Cari Data");
                            Console.WriteLine("6. Keluar");
                            Console.WriteLine("\n Enter your choice (1-6): ");
                            char ch = Convert.ToChar(Console.ReadLine());
                            switch (ch)
                            {
                                case '1':
                                    Console.Clear();
                                    Console.WriteLine("Data Laporan KP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Laporan KP\n");
                                    Console.WriteLine("Masukkan ID Laporan (15 characters): ");
                                    string idLaporan = Console.ReadLine();
                                    Console.WriteLine("Masukkan NIM Mahasiswa : ");
                                    string nim = Console.ReadLine();
                                    Console.WriteLine("Masukkan NIK Dosen Pembimbing : ");
                                    string nikDosen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Tanggal (yyyy-mm-dd) : ");
                                    DateTime tanggal = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("Masukkan Nama File DOC : ");
                                    string fileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(idLaporan, nim, nikDosen, tanggal, fileDOC, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan ID Laporan yang ingin dihapus:\n");
                                    string idLaporanHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(idLaporanHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Laporan KP\n");
                                    Console.WriteLine("Masukkan ID Laporan yang akan diupdate: ");
                                    string idLaporanToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan NIK Dosen Pembimbing baru : ");
                                    string newNikDosen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Tanggal baru (yyyy-mm-dd) : ");
                                    DateTime newTanggal = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("Masukkan Nama File DOC baru : ");
                                    string newFileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idLaporanToUpdate, newNikDosen, newTanggal, newFileDOC, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Laporan KP Berdasarkan ID Laporan\n");
                                    Console.WriteLine("Masukkan ID Laporan yang ingin dicari: ");
                                    string searchIdLaporan = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByIdLaporan(searchIdLaporan, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nTerjadi kesalahan dalam pencarian data: " + e.Message);
                                    }
                                    break;
                                case '6':
                                    conn.Close();
                                    Console.Clear();
                                    //prg.Main(new string[0]);
                                    return;
                                default:
                                    Console.Clear();
                                    Console.WriteLine("\n Invalid option");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("\nCheck for the value entered");
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tidak Dapat Mengakses Database Tersebut\n");
                    Console.ResetColor();
                }
            }
        }

        public void baca(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Laporan", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    Console.WriteLine(r.GetValue(i));
                }
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string idLaporan, string nim, string nikDosen, DateTime tanggal, string fileDOC, SqlConnection conn)
        {
            string str = "INSERT INTO LaporanKP (ID_Laporan, nim, NIK, Tanggal, fileDOC) VALUES (@idLaporan, @nim, @nikDosen, @tanggal, @fileDOC)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idLaporan", idLaporan));
            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.Parameters.Add(new SqlParameter("@nikDosen", nikDosen));
            cmd.Parameters.Add(new SqlParameter("@tanggal", tanggal));
            cmd.Parameters.Add(new SqlParameter("@fileDOC", fileDOC));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string idLaporan, SqlConnection con)
        {
            string str = "DELETE FROM LaporanKP WHERE ID_Laporan = @idLaporan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idLaporan", idLaporan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string idLaporan, string newNikDosen, DateTime newTanggal, string newFileDOC, SqlConnection con)
        {
            string str = "UPDATE LaporanKP SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newNikDosen))
            {
                str += "NIK = @newNikDosen, ";
                parameters.Add("@newNikDosen");
            }

            if (newTanggal != DateTime.MinValue)
            {
                str += "Tanggal = @newTanggal, ";
                parameters.Add("@newTanggal");
            }

            if (!string.IsNullOrEmpty(newFileDOC))
            {
                str += "fileDOC = @newFileDOC, ";
                parameters.Add("@newFileDOC");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE ID_Laporan = @idLaporan";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            foreach (string parameter in parameters)
            {
                if (parameter == "@newNikDosen")
                    cmd.Parameters.AddWithValue(parameter, newNikDosen);
                else if (parameter == "@newTanggal")
                    cmd.Parameters.AddWithValue(parameter, newTanggal);
                else if (parameter == "@newFileDOC")
                    cmd.Parameters.AddWithValue(parameter, newFileDOC);
            }

            cmd.Parameters.AddWithValue("@idLaporan", idLaporan);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Data berhasil diupdate.");
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan atau gagal diupdate.");
            }
        }


        public void searchByIdLaporan(string idLaporan, SqlConnection con)
        {
            string query = "SELECT * FROM LaporanKP WHERE ID_Laporan = @idLaporan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idLaporan", idLaporan);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine(reader.GetValue(i));
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan.");
            }

            reader.Close();
        }
    }
}
