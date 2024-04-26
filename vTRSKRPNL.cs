using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vTRSKRPNL
    {
        public void Main()
        {
            vTRSKRPNL pr = new vTRSKRPNL();
            Program prg = new Program();
            while (true)
            {
                try
                {
                    SqlConnection conn = null;
                    string strKoneksi = "Data source = DESKTOP-CKQPUM7\\JODINURF;" +
                        "Initial catalog = KelolaDataKP;User ID = sa; Password = Jodinurf12";
                    conn = new SqlConnection(string.Format(strKoneksi));
                    conn.Open();
                    Console.Clear();
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\nMenu Kelola Data Transkrip Nilai");
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
                                    Console.WriteLine("Data Transkrip Nilai\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Transkrip Nilai\n");
                                    Console.WriteLine("Masukan NIM Mahasiswa : ");
                                    string nim = Console.ReadLine();
                                    Console.WriteLine("Masukan Jumlah SKS : ");
                                    string jumlahSKS = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama File DOC : ");
                                    string fileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(nim, jumlahSKS, fileDOC, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan NIM Mahasiswa yang ingin dihapus:\n");
                                    string nimHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(nimHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Transkrip Nilai\n");
                                    Console.WriteLine("Masukkan NIM Mahasiswa yang akan diupdate: ");
                                    string nimToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan data baru:\n");
                                    Console.WriteLine("Masukan Jumlah SKS Baru : ");
                                    string newJumlahSKS = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nama File DOC Baru : ");
                                    string newFileDOC = Console.ReadLine();
                                    try
                                    {
                                        pr.update(nimToUpdate, newJumlahSKS, newFileDOC, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Transkrip Nilai Berdasarkan NIM\n");
                                    Console.WriteLine("Masukkan NIM Mahasiswa yang ingin dicari: ");
                                    string searchNIM = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNIM(searchNIM, conn);
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
            SqlCommand cmd = new SqlCommand("Select nim, jumlahSKS, fileDOC From transkrip_Nilai", con);
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

        public void insert(string nim, string jumlahSKS, string fileDOC, SqlConnection conn)
        {
            string str = "insert into transkrip_Nilai (nim, jumlahSKS, fileDOC) values (@nim, @jumlahSKS, @fileDOC)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.Parameters.Add(new SqlParameter("@jumlahSKS", jumlahSKS));
            cmd.Parameters.Add(new SqlParameter("@fileDOC", fileDOC));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string nim, SqlConnection con)
        {
            string str = "Delete from transkrip_Nilai where nim = @nim";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string nim, string newJumlahSKS, string newFileDOC, SqlConnection con)
        {
            // Buat query untuk update
            string str = "UPDATE transkrip_Nilai SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newJumlahSKS))
            {
                str += "jumlahSKS = @newJumlahSKS, ";
                parameters.Add("@newJumlahSKS");
            }
            if (!string.IsNullOrEmpty(newFileDOC))
            {
                str += "fileDOC = @newFileDOC, ";
                parameters.Add("@newFileDOC");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE nim = @nim";

            // Buat command SQL
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            // Tambahkan parameter baru sesuai dengan data yang diinputkan
            foreach (string parameter in parameters)
            {
                if (parameter == "@newJumlahSKS")
                    cmd.Parameters.AddWithValue(parameter, newJumlahSKS);
                else if (parameter == "@newFileDOC")
                    cmd.Parameters.AddWithValue(parameter, newFileDOC);
            }

            cmd.Parameters.AddWithValue("@nim", nim);

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


        public void searchByNIM(string nim, SqlConnection con)
        {
            string query = "SELECT * FROM transkrip_Nilai WHERE nim = @nim";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nim", nim);

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
