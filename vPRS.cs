using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vPRS
    {
        public void Main()
        {
            vPRS pr = new vPRS();
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
                            Console.WriteLine("\nMenu Kelola Data Perusahaan");
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
                                    Console.WriteLine("Data Perusahaan\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Perusahaan\n");
                                    Console.WriteLine("Masukkan Nama Perusahaan : ");
                                    string namaPerusahaan = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telephone Perusahaan : ");
                                    string telephone = Console.ReadLine();
                                    Console.WriteLine("Masukkan Alamat Perusahaan : ");
                                    string alamat = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(namaPerusahaan, telephone, alamat, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Kode Perusahaan yang ingin dihapus:\n");
                                    int kdPerusahaan;
                                    if (int.TryParse(Console.ReadLine(), out kdPerusahaan))
                                    {
                                        try
                                        {
                                            pr.delete(kdPerusahaan, conn);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\nAnda tidak memiliki " +
                                                "akses untuk menghapus data atau data yang anda masukkan salah");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nMasukkan harus berupa angka.");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Perusahaan\n");
                                    Console.WriteLine("Masukkan Kode Perusahaan yang akan diupdate: ");
                                    int kdPerusahaanToUpdate;
                                    if (int.TryParse(Console.ReadLine(), out kdPerusahaanToUpdate))
                                    {
                                        Console.WriteLine("Masukkan data baru:\n");
                                        Console.WriteLine("Masukkan Nama Perusahaan Baru : ");
                                        string newNamaPerusahaan = Console.ReadLine();
                                        Console.WriteLine("Masukkan Nomor Telephone Perusahaan Baru : ");
                                        string newTelephone = Console.ReadLine();
                                        Console.WriteLine("Masukkan Alamat Perusahaan Baru : ");
                                        string newAlamat = Console.ReadLine();
                                        try
                                        {
                                            pr.update(kdPerusahaanToUpdate, newNamaPerusahaan, newTelephone, newAlamat, conn);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nMasukkan harus berupa angka.");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Perusahaan Berdasarkan Kode Perusahaan\n");
                                    Console.WriteLine("Masukkan Kode Perusahaan yang ingin dicari: ");
                                    int searchKdPerusahaan;
                                    if (int.TryParse(Console.ReadLine(), out searchKdPerusahaan))
                                    {
                                        try
                                        {
                                            pr.searchByKodePerusahaan(searchKdPerusahaan, conn);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\nTerjadi kesalahan dalam pencarian data: " + e.Message);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nMasukkan harus berupa angka.");
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Perusahaan", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine($"Kode Perusahaan: {r["kd_Perusahaan"]}, Nama Perusahaan: {r["NamaPerusahaan"]}, Telephone: {r["Telephone"]}, Alamat: {r["Alamat"]}");
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string namaPerusahaan, string telephone, string alamat, SqlConnection conn)
        {
            string str = "INSERT INTO Perusahaan (NamaPerusahaan, Telephone, Alamat) VALUES (@namaPerusahaan, @telephone, @alamat)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@namaPerusahaan", namaPerusahaan));
            cmd.Parameters.Add(new SqlParameter("@telephone", telephone));
            cmd.Parameters.Add(new SqlParameter("@alamat", alamat));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(int kdPerusahaan, SqlConnection con)
        {
            string str = "DELETE FROM Perusahaan WHERE kd_Perusahaan = @kdPerusahaan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdPerusahaan", kdPerusahaan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(int kdPerusahaan, string newNamaPerusahaan, string newTelephone, string newAlamat, SqlConnection con)
        {
            // Buat query untuk update
            string str = "UPDATE Perusahaan SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newNamaPerusahaan))
            {
                str += "NamaPerusahaan = @newNamaPerusahaan, ";
                parameters.Add("@newNamaPerusahaan");
            }
            if (!string.IsNullOrEmpty(newTelephone))
            {
                str += "Telephone = @newTelephone, ";
                parameters.Add("@newTelephone");
            }
            if (!string.IsNullOrEmpty(newAlamat))
            {
                str += "Alamat = @newAlamat, ";
                parameters.Add("@newAlamat");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE kd_Perusahaan = @kdPerusahaan";

            // Buat command SQL
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            // Tambahkan parameter baru sesuai dengan data yang diinputkan
            foreach (string parameter in parameters)
            {
                if (parameter == "@newNamaPerusahaan")
                    cmd.Parameters.AddWithValue(parameter, newNamaPerusahaan);
                else if (parameter == "@newTelephone")
                    cmd.Parameters.AddWithValue(parameter, newTelephone);
                else if (parameter == "@newAlamat")
                    cmd.Parameters.AddWithValue(parameter, newAlamat);
            }

            // Tambahkan parameter untuk Kode Perusahaan
            cmd.Parameters.AddWithValue("@kdPerusahaan", kdPerusahaan);

            // Eksekusi command SQL
            int rowsAffected = cmd.ExecuteNonQuery();

            // Beri pesan sesuai dengan hasil eksekusi
            if (rowsAffected > 0)
            {
                Console.WriteLine("Data berhasil diupdate.");
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan atau gagal diupdate.");
            }
        }


        public void searchByKodePerusahaan(int kdPerusahaan, SqlConnection con)
        {
            string query = "SELECT * FROM Perusahaan WHERE kd_Perusahaan = @kdPerusahaan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kdPerusahaan", kdPerusahaan);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"Kode Perusahaan: {reader["kd_Perusahaan"]}, Nama Perusahaan: {reader["NamaPerusahaan"]}, Telephone: {reader["Telephone"]}, Alamat: {reader["Alamat"]}");
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
