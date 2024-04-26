using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vMHS
    {
        public void Main()
        {
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
                            Console.WriteLine("\nMenu Kelola Data Mahasiswa");
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
                                    Console.WriteLine("Data Mahasiswa KP\n");
                                    Console.WriteLine();
                                    baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Mahasiswa\n");
                                    Console.WriteLine("Masukan NIM Mahasiswa : ");
                                    string nim = Console.ReadLine();
                                    Console.WriteLine("Masukan Nama Mahasiswa : ");
                                    string nama = Console.ReadLine();
                                    Console.WriteLine("Masukkan E - mail Mahasiswa : ");
                                    string email = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telephone Mahasiswa :");
                                    string NoHP = Console.ReadLine();
                                    Console.WriteLine("Masukan Jenis Kelamin (L/P) : ");
                                    string JenisKelamin = Console.ReadLine();
                                    try
                                    {
                                        insert(nim, nama, email, NoHP, JenisKelamin, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan Data Mahasiswa ingin dihapus:\n");
                                    string nimHapus = Console.ReadLine();
                                    try
                                    {
                                        delete(nimHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Mahasiswa\n");
                                    Console.WriteLine("Masukkan NIM Mahasiswa yang akan diupdate: ");
                                    string nimToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan data baru:\n");
                                    Console.WriteLine("Masukan NIM Mahasiswa Baru : ");
                                    string newNIM = Console.ReadLine();
                                    Console.WriteLine("Masukan Nama Mahasiswa Baru : ");
                                    string newName = Console.ReadLine();
                                    Console.WriteLine("Masukan E-mail Mahasiswa Baru : ");
                                    string newEmail = Console.ReadLine();
                                    Console.WriteLine("Masukan Nomor Telephone Mahasiswa Baru : ");
                                    string newPhoneNumber = Console.ReadLine();
                                    Console.WriteLine("Masukan Jenis Kelamin (L/P) Baru : ");
                                    string newJK = Console.ReadLine();
                                    try
                                    {
                                        update(nimToUpdate, newNIM, newName, newEmail, newPhoneNumber, newJK, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Mahasiswa Berdasarkan NIM\n");
                                    Console.WriteLine("Masukkan NIM Mahasiswa yang ingin dicari: ");
                                    string searchNIM = Console.ReadLine();
                                    try
                                    {
                                        searchByNIM(searchNIM, conn);
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
            SqlCommand cmd = new SqlCommand("Select nim, nama, email, NoHp, JenisKelamin From Mahasiswa", con);
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

        public void insert(string nim, string nama, string email, string NoHP, string JenisKelamin, SqlConnection conn)
        {
            string str = "insert into Mahasiswa (nim,nama,email,NoHP,JenisKelamin)" +
                "values(@nim,@nm,@e,@tlp,@jk)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("nim", nim));
            cmd.Parameters.Add(new SqlParameter("nm", nama));
            cmd.Parameters.Add(new SqlParameter("e", email));
            cmd.Parameters.Add(new SqlParameter("tlp", NoHP));
            cmd.Parameters.Add(new SqlParameter("jk", JenisKelamin));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string nim, SqlConnection con)
        {
            string str = "DELETE FROM Mahasiswa WHERE nim = @nim";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }


        public void update(string oldNIM, string newNIM, string newName, string newEmail, string newPhoneNumber, string newJK, SqlConnection con)
        {
            // Buat query untuk update
            string str = "UPDATE Mahasiswa SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newNIM))
            {
                str += "nim = @newNIM, ";
                parameters.Add("@newNIM");
            }
            if (!string.IsNullOrEmpty(newName))
            {
                str += "nama = @newName, ";
                parameters.Add("@newName");
            }
            if (!string.IsNullOrEmpty(newEmail))
            {
                str += "email = @newEmail, ";
                parameters.Add("@newEmail");
            }
            if (!string.IsNullOrEmpty(newPhoneNumber))
            {
                str += "NoHP = @newPhoneNumber, ";
                parameters.Add("@newPhoneNumber");
            }
            if (!string.IsNullOrEmpty(newJK))
            {
                str += "JenisKelamin = @newJK, ";
                parameters.Add("@newJK");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE nim = @oldNIM";

            // Buat command SQL
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            // Tambahkan parameter baru sesuai dengan data yang diinputkan
            foreach (string parameter in parameters)
            {
                if (parameter == "@newNIM")
                    cmd.Parameters.AddWithValue(parameter, newNIM);
                else if (parameter == "@newName")
                    cmd.Parameters.AddWithValue(parameter, newName);
                else if (parameter == "@newEmail")
                    cmd.Parameters.AddWithValue(parameter, newEmail);
                else if (parameter == "@newPhoneNumber")
                    cmd.Parameters.AddWithValue(parameter, newPhoneNumber);
                else if (parameter == "@newJK")
                    cmd.Parameters.AddWithValue(parameter, newJK);
            }

            // Tambahkan parameter untuk NIM lama
            cmd.Parameters.AddWithValue("@oldNIM", oldNIM);

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


        public void searchByNIM(string nim, SqlConnection con)
        {
            string query = "SELECT * FROM Mahasiswa WHERE nim = @nim";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nim", nim);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("Hasil Pencarian:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"NIM: {reader["nim"]}, Nama: {reader["nama"]}, Email: {reader["email"]}, No HP: {reader["NoHP"]}, Jenis Kelamin: {reader["JenisKelamin"]}");
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
