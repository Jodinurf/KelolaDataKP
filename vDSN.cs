using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vDSN
    {
        public void Main()
        {
            vDSN pr = new vDSN();
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
                            Console.WriteLine("\nMenu Kelola Data Dosen Pembimbing");
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
                                    Console.WriteLine("Data Dosen Pembimbing KP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Dosen Pembimbing\n");
                                    Console.WriteLine("Masukan NIK Dosen Pembimbing : ");
                                    string nik = Console.ReadLine();
                                    Console.WriteLine("Masukan Nama Dosen Pembimbing : ");
                                    string nama = Console.ReadLine();
                                    Console.WriteLine("Masukkan E - mail Dosen Pembimbing : ");
                                    string email = Console.ReadLine();
                                    Console.WriteLine("Masukkan Nomor Telephone Dosen Pembimbing :");
                                    string noHP = Console.ReadLine();
                                    try
                                    {
                                        pr.insert(nik, nama, email, noHP, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menambah data");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan NIK Dosen Pembimbing yang ingin dihapus:\n");
                                    string nikHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(nikHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Dosen Pembimbing\n");
                                    Console.WriteLine("Masukkan NIK Dosen Pembimbing yang akan diupdate: ");
                                    string nikToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan data baru:\n");
                                    Console.WriteLine("Masukan NIK Dosen Pembimbing Baru : ");
                                    string newNIK = Console.ReadLine();
                                    Console.WriteLine("Masukan Nama Dosen Pembimbing Baru : ");
                                    string newName = Console.ReadLine();
                                    Console.WriteLine("Masukan E-mail Dosen Pembimbing Baru : ");
                                    string newEmail = Console.ReadLine();
                                    Console.WriteLine("Masukan Nomor Telephone Dosen Pembimbing Baru : ");
                                    string newNoHP = Console.ReadLine();
                                    try
                                    {
                                        pr.update(nikToUpdate, newNIK, newName, newEmail, newNoHP, conn);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Dosen Pembimbing Berdasarkan NIK\n");
                                    Console.WriteLine("Masukkan NIK Dosen Pembimbing yang ingin dicari: ");
                                    string searchNIK = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByNIK(searchNIK, conn);
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
            SqlCommand cmd = new SqlCommand("Select NIK, nama, email, NoHp From Dosen", con);
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

        public void insert(string nik, string nama, string email, string noHP, SqlConnection conn)
        {
            string str = "insert into Dosen (NIK, nama, email, NoHP) values (@nik, @nm, @e, @tlp)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nik", nik));
            cmd.Parameters.Add(new SqlParameter("@nm", nama));
            cmd.Parameters.Add(new SqlParameter("@e", email));
            cmd.Parameters.Add(new SqlParameter("@tlp", noHP));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string nik, SqlConnection con)
        {
            string str = "Delete from Dosen where NIK = @nik";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nik", nik));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string oldNIK, string newNIK, string newName, string newEmail, string newNoHP, SqlConnection con)
        {
            // Buat query untuk update
            string str = "UPDATE Dosen SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newNIK))
            {
                str += "NIK = @newNIK, ";
                parameters.Add("@newNIK");
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
            if (!string.IsNullOrEmpty(newNoHP))
            {
                str += "NoHP = @newNoHP, ";
                parameters.Add("@newNoHP");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE NIK = @oldNIK";

            // Buat command SQL
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            // Tambahkan parameter baru sesuai dengan data yang diinputkan
            foreach (string parameter in parameters)
            {
                if (parameter == "@newNIK")
                    cmd.Parameters.AddWithValue(parameter, newNIK);
                else if (parameter == "@newName")
                    cmd.Parameters.AddWithValue(parameter, newName);
                else if (parameter == "@newEmail")
                    cmd.Parameters.AddWithValue(parameter, newEmail);
                else if (parameter == "@newNoHP")
                    cmd.Parameters.AddWithValue(parameter, newNoHP);
            }

            // Tambahkan parameter untuk NIK lama
            cmd.Parameters.AddWithValue("@oldNIK", oldNIK);

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


        public void searchByNIK(string nik, SqlConnection con)
        {
            string query = "SELECT * FROM Dosen WHERE NIK = @nik";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nik", nik);

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
