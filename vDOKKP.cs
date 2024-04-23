using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace KelolaDataKP
{
    internal class vDOKKP
    {
        public void Main()
        {
            vDOKKP pr = new vDOKKP();
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
                            Console.WriteLine("\nMenu Kelola Data Dokumen KP");
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
                                    Console.WriteLine("Data Dokumen_KP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Dokumen_KP\n");
                                    Console.WriteLine("Masukkan ID Dokumen (14 characters): ");
                                    string idDokumen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Logbook: ");
                                    string logbook = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(idDokumen, logbook, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan ID Dokumen yang ingin dihapus:\n");
                                    string idDokumenHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(idDokumenHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Dokumen_KP\n");
                                    Console.WriteLine("Masukkan ID Dokumen yang akan diupdate: ");
                                    string idDokumenToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Logbook baru: ");
                                    string newLogbook = Console.ReadLine();
                                    try
                                    {
                                        pr.update(idDokumenToUpdate, newLogbook, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Dokumen_KP Berdasarkan ID Dokumen\n");
                                    Console.WriteLine("Masukkan ID Dokumen yang ingin dicari: ");
                                    string searchIdDokumen = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByIdDokumen(searchIdDokumen, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Dokumen_KP", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"ID Dokumen: {r["ID_Dokumen"]}, Logbook: {r["logbook"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string idDokumen, string logbook, SqlConnection conn)
        {
            string str = "INSERT INTO Dokumen_KP (ID_Dokumen, logbook) VALUES (@idDokumen, @logbook)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idDokumen", idDokumen));
            cmd.Parameters.Add(new SqlParameter("@logbook", logbook));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string idDokumen, SqlConnection con)
        {
            string str = "DELETE FROM Dokumen_KP WHERE ID_Dokumen = @idDokumen";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idDokumen", idDokumen));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string idDokumen, string newLogbook, SqlConnection con)
        {
            string str = "UPDATE Dokumen_KP SET logbook = @newLogbook WHERE ID_Dokumen = @idDokumen";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@newLogbook", newLogbook);
            cmd.Parameters.AddWithValue("@idDokumen", idDokumen);

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

        public void searchByIdDokumen(string idDokumen, SqlConnection con)
        {
            string query = "SELECT * FROM Dokumen_KP WHERE ID_Dokumen = @idDokumen";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idDokumen", idDokumen);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"ID Dokumen: {reader["ID_Dokumen"]}, Logbook: {reader["logbook"]}");
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
