using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vSRTSLS
    {
        public void Main()
        {
            vSRTSLS pr = new vSRTSLS();
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
                            Console.WriteLine("\nMenu Kelola Data Surat Selesai SKP");
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
                                    Console.WriteLine("Data Surat Selesai KP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Surat Selesai KP\n");
                                    Console.WriteLine("Masukkan KD Surat (15 characters): ");
                                    string kdSurat = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Dokumen (14 characters) : ");
                                    string idDokumen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Disetujui Oleh : ");
                                    string disetujuiOleh = Console.ReadLine();
                                    Console.WriteLine("Masukkan Status (DITERIMA/DITOLAK) : ");
                                    string status = Console.ReadLine().ToUpper();
                                    Console.WriteLine("Masukkan Nama File DOC : ");
                                    string fileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(kdSurat, idDokumen, disetujuiOleh, status, fileDOC, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan KD Surat yang ingin dihapus:\n");
                                    string kdSuratHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(kdSuratHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Surat Selesai KP\n");
                                    Console.WriteLine("Masukkan KD Surat yang akan diupdate: ");
                                    string kdSuratToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Disetujui Oleh baru : ");
                                    string newDisetujuiOleh = Console.ReadLine();
                                    Console.WriteLine("Masukkan Status baru (DITERIMA/DITOLAK) : ");
                                    string newStatus = Console.ReadLine().ToUpper();
                                    Console.WriteLine("Masukkan Nama File DOC baru : ");
                                    string newFileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.update(kdSuratToUpdate, newDisetujuiOleh, newStatus, newFileDOC, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Surat Selesai KP Berdasarkan KD Surat\n");
                                    Console.WriteLine("Masukkan KD Surat yang ingin dicari: ");
                                    string searchKdSurat = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByKdSurat(searchKdSurat, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM SuratSelesaiKP", con);
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

        public void insert(string kdSurat, string idDokumen, string disetujuiOleh, string status, string fileDOC, SqlConnection conn)
        {
            string str = "INSERT INTO SuratSelesaiKP (kd_Surat, ID_Dokumen, DisetujuiOleh, status, fileDOC) VALUES (@kdSurat, @idDokumen, @disetujuiOleh, @status, @fileDOC)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdSurat", kdSurat));
            cmd.Parameters.Add(new SqlParameter("@idDokumen", idDokumen));
            cmd.Parameters.Add(new SqlParameter("@disetujuiOleh", disetujuiOleh));
            cmd.Parameters.Add(new SqlParameter("@status", status));
            cmd.Parameters.Add(new SqlParameter("@fileDOC", fileDOC));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string kdSurat, SqlConnection con)
        {
            string str = "DELETE FROM SuratSelesaiKP WHERE kd_Surat = @kdSurat";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdSurat", kdSurat));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string kdSurat, string newDisetujuiOleh, string newStatus, string newFileDOC, SqlConnection con)
        {
    
            string str = "UPDATE SuratSelesaiKP SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newDisetujuiOleh))
            {
                str += "DisetujuiOleh = @newDisetujuiOleh, ";
                parameters.Add("@newDisetujuiOleh");
            }

            if (!string.IsNullOrEmpty(newStatus))
            {
                str += "status = @newStatus, ";
                parameters.Add("@newStatus");
            }

            if (!string.IsNullOrEmpty(newFileDOC))
            {
                str += "fileDOC = @newFileDOC, ";
                parameters.Add("@newFileDOC");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE kd_Surat = @kdSurat";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            foreach (string parameter in parameters)
            {
                if (parameter == "@newDisetujuiOleh")
                    cmd.Parameters.AddWithValue(parameter, newDisetujuiOleh);
                else if (parameter == "@newStatus")
                    cmd.Parameters.AddWithValue(parameter, newStatus);
                else if (parameter == "@newFileDOC")
                    cmd.Parameters.AddWithValue(parameter, newFileDOC);
            }

            cmd.Parameters.AddWithValue("@kdSurat", kdSurat);

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


        public void searchByKdSurat(string kdSurat, SqlConnection con)
        {
            string query = "SELECT * FROM SuratSelesaiKP WHERE kd_Surat = @kdSurat";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kdSurat", kdSurat);

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
