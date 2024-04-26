using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vPRMTKP
    {
        public void Main()
        {
            vPRMTKP pr = new vPRMTKP();
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
                            Console.WriteLine("\nMenu Kelola Data Permintaan Magang");
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
                                    Console.WriteLine("Data Permintaan KP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Permintaan KP\n");
                                    Console.WriteLine("Masukkan KD Surat KP (15 characters): ");
                                    string kdSrtKP = Console.ReadLine();
                                    Console.WriteLine("Masukkan NIM Mahasiswa : ");
                                    string nim = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Perusahaan : ");
                                    int kdPerusahaan;
                                    if (int.TryParse(Console.ReadLine(), out kdPerusahaan))
                                    {
                                        Console.WriteLine("Masukkan Nama Penerima : ");
                                        string namaPenerima = Console.ReadLine();
                                        Console.WriteLine("Masukkan Tugas : ");
                                        string tugas = Console.ReadLine();
                                        Console.WriteLine("Masukkan Nama File DOC : ");
                                        string fileDOC = Console.ReadLine();
                                        try
                                        {
                                            pr.insert(kdSrtKP, nim, kdPerusahaan, namaPenerima, tugas, fileDOC, conn);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\nAnda tidak memiliki " +
                                                "akses untuk menambah data");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nKode Perusahaan harus berupa angka.");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan KD Surat KP yang ingin dihapus:\n");
                                    string kdSrtKPHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(kdSrtKPHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Permintaan KP\n");
                                    Console.WriteLine("Masukkan KD Surat KP yang akan diupdate: ");
                                    string kdSrtKPToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama Penerima baru : ");
                                    string newNamaPenerima = Console.ReadLine();
                                    Console.WriteLine("Masukkan Tugas baru : ");
                                    string newTugas = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama File DOC baru : ");
                                    string newFileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.update(kdSrtKPToUpdate, newNamaPenerima, newTugas, newFileDOC, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Permintaan KP Berdasarkan KD Surat KP\n");
                                    Console.WriteLine("Masukkan KD Surat KP yang ingin dicari: ");
                                    string searchKdSrtKP = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByKdSrtKP(searchKdSrtKP, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM PermintaanKP", con);
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

        public void insert(string kdSrtKP, string nim, int kdPerusahaan, string namaPenerima, string tugas, string fileDOC, SqlConnection conn)
        {
            string str = "INSERT INTO PermintaanKP (KD_SrtKP, nim, kd_Perusahaan, NamaPenerima, Tugas, fileDOC) VALUES (@kdSrtKP, @nim, @kdPerusahaan, @namaPenerima, @tugas, @fileDOC)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdSrtKP", kdSrtKP));
            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.Parameters.Add(new SqlParameter("@kdPerusahaan", kdPerusahaan));
            cmd.Parameters.Add(new SqlParameter("@namaPenerima", namaPenerima));
            cmd.Parameters.Add(new SqlParameter("@tugas", tugas));
            cmd.Parameters.Add(new SqlParameter("@fileDOC", fileDOC));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string kdSrtKP, SqlConnection con)
        {
            string str = "DELETE FROM PermintaanKP WHERE KD_SrtKP = @kdSrtKP";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdSrtKP", kdSrtKP));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string kdSrtKP, string newNamaPenerima, string newTugas, string newFileDOC, SqlConnection con)
        {
            string str = "UPDATE PermintaanKP SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newNamaPenerima))
            {
                str += "NamaPenerima = @newNamaPenerima, ";
                parameters.Add("@newNamaPenerima");
            }

            if (!string.IsNullOrEmpty(newTugas))
            {
                str += "Tugas = @newTugas, ";
                parameters.Add("@newTugas");
            }

            if (!string.IsNullOrEmpty(newFileDOC))
            {
                str += "fileDOC = @newFileDOC, ";
                parameters.Add("@newFileDOC");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE KD_SrtKP = @kdSrtKP";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            foreach (string parameter in parameters)
            {
                if (parameter == "@newNamaPenerima")
                    cmd.Parameters.AddWithValue(parameter, newNamaPenerima);
                else if (parameter == "@newTugas")
                    cmd.Parameters.AddWithValue(parameter, newTugas);
                else if (parameter == "@newFileDOC")
                    cmd.Parameters.AddWithValue(parameter, newFileDOC);
            }

            cmd.Parameters.AddWithValue("@kdSrtKP", kdSrtKP);

            // Eksekusi command SQL
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


        public void searchByKdSrtKP(string kdSrtKP, SqlConnection con)
        {
            string query = "SELECT * FROM PermintaanKP WHERE KD_SrtKP = @kdSrtKP";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kdSrtKP", kdSrtKP);

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
