using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class vPRSTJSKP
    {
        public void Main()
        {
            vPRSTJSKP pr = new vPRSTJSKP();
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
                            Console.WriteLine("\nMenu Kelola Data Persetujuan SKP");
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
                                    Console.WriteLine("Data Persetujuan SKP\n");
                                    Console.WriteLine();
                                    pr.baca(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Input Data Persetujuan SKP\n");
                                    Console.WriteLine("Masukkan KD Persetujuan (15 characters): ");
                                    string kdPersetujuan = Console.ReadLine();
                                    Console.WriteLine("Masukkan ID Dokumen (14 characters) : ");
                                    string idDokumen = Console.ReadLine();
                                    Console.WriteLine("Masukkan NIK Dosen : ");
                                    string nikDosen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Status (DITERIMA/DITOLAK) : ");
                                    string status = Console.ReadLine().ToUpper();
                                    Console.WriteLine("Masukkan Nama Dosen : ");
                                    string namaDosen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Jadwal SKP (YYYY-MM-DD) : ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime jadwalSKP))
                                    {
                                        try
                                        {
                                            pr.insert(kdPersetujuan, idDokumen, nikDosen, status, namaDosen, jadwalSKP, conn);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\nAnda tidak memiliki " +
                                                "akses untuk menambah data");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nFormat tanggal tidak valid.");
                                    }
                                    break;
                                case '3':
                                    Console.Clear();
                                    Console.WriteLine("Masukkan KD Persetujuan yang ingin dihapus:\n");
                                    string kdPersetujuanHapus = Console.ReadLine();
                                    try
                                    {
                                        pr.delete(kdPersetujuanHapus, conn);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nAnda tidak memiliki " +
                                            "akses untuk menghapus data atau data yang anda masukkan salah");
                                    }
                                    break;
                                case '4':
                                    Console.Clear();
                                    Console.WriteLine("Update Data Persetujuan SKP\n");
                                    Console.WriteLine("Masukkan KD Persetujuan yang akan diupdate: ");
                                    string kdPersetujuanToUpdate = Console.ReadLine();
                                    Console.WriteLine("Masukkan Status baru (DITERIMA/DITOLAK) : ");
                                    string newStatus = Console.ReadLine().ToUpper();
                                    Console.WriteLine("Masukkan Nama Dosen baru : ");
                                    string newNamaDosen = Console.ReadLine();
                                    Console.WriteLine("Masukkan Jadwal SKP baru (YYYY-MM-DD) : ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime newJadwalSKP))
                                    {
                                        try
                                        {
                                            pr.update(kdPersetujuanToUpdate, newStatus, newNamaDosen, newJadwalSKP, conn);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("\nAnda tidak memiliki akses untuk mengubah data atau data yang anda masukkan salah");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nFormat tanggal tidak valid.");
                                    }
                                    break;
                                case '5':
                                    Console.Clear();
                                    Console.WriteLine("Cari Data Persetujuan SKP Berdasarkan KD Persetujuan\n");
                                    Console.WriteLine("Masukkan KD Persetujuan yang ingin dicari: ");
                                    string searchKdPersetujuan = Console.ReadLine();
                                    try
                                    {
                                        pr.searchByKdPersetujuan(searchKdPersetujuan, conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM PersetujuanSKP", con);
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

        public void insert(string kdPersetujuan, string idDokumen, string nikDosen, string status, string namaDosen, DateTime jadwalSKP, SqlConnection conn)
        {
            string str = "INSERT INTO PersetujuanSKP (KD_Persetujuan, ID_Dokumen, NIK, status, namaDSN, JadwalSKP) VALUES (@kdPersetujuan, @idDokumen, @nikDosen, @status, @namaDosen, @jadwalSKP)";
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdPersetujuan", kdPersetujuan));
            cmd.Parameters.Add(new SqlParameter("@idDokumen", idDokumen));
            cmd.Parameters.Add(new SqlParameter("@nikDosen", nikDosen));
            cmd.Parameters.Add(new SqlParameter("@status", status));
            cmd.Parameters.Add(new SqlParameter("@namaDosen", namaDosen));
            cmd.Parameters.Add(new SqlParameter("@jadwalSKP", jadwalSKP));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void delete(string kdPersetujuan, SqlConnection con)
        {
            string str = "DELETE FROM PersetujuanSKP WHERE KD_Persetujuan = @kdPersetujuan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@kdPersetujuan", kdPersetujuan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Dihapus");
        }

        public void update(string kdPersetujuan, string newStatus, string newNamaDosen, DateTime newJadwalSKP, SqlConnection con)
        {
            string str = "UPDATE PersetujuanSKP SET ";
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(newStatus))
            {
                str += "status = @newStatus, ";
                parameters.Add("@newStatus");
            }
            if (!string.IsNullOrEmpty(newNamaDosen))
            {
                str += "namaDSN = @newNamaDosen, ";
                parameters.Add("@newNamaDosen");
            }
            if (newJadwalSKP != DateTime.MinValue)
            {
                str += "JadwalSKP = @newJadwalSKP, ";
                parameters.Add("@newJadwalSKP");
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE KD_Persetujuan = @kdPersetujuan";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            foreach (string parameter in parameters)
            {
                if (parameter == "@newStatus")
                    cmd.Parameters.AddWithValue(parameter, newStatus);
                else if (parameter == "@newNamaDosen")
                    cmd.Parameters.AddWithValue(parameter, newNamaDosen);
                else if (parameter == "@newJadwalSKP")
                    cmd.Parameters.AddWithValue(parameter, newJadwalSKP);
            }

            cmd.Parameters.AddWithValue("@kdPersetujuan", kdPersetujuan);

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


        public void searchByKdPersetujuan(string kdPersetujuan, SqlConnection con)
        {
            string query = "SELECT * FROM PersetujuanSKP WHERE KD_Persetujuan = @kdPersetujuan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kdPersetujuan", kdPersetujuan);

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
