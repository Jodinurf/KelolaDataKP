using System;
using System.Data;
using System.Data.SqlClient;

namespace KelolaDataKP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SqlConnection conn = null;
            string strKoneksi = "Data source = DESKTOP-CKQPUM7\\JODINURF;" +
                "Initial catalog = KelolaDataKP;User ID = sa; Password = Jodinurf12";
            conn = new SqlConnection(strKoneksi);
            Program prg = new Program();
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu Pengelola Data KP");
                    Console.WriteLine("1. Kelola Data Mahasiswa");
                    Console.WriteLine("2. Kelola Data Dosen");
                    Console.WriteLine("3. Kelola Data Transkrip_Nilai");
                    Console.WriteLine("4. Kelola Data Perusahaan");
                    Console.WriteLine("5. Kelola Data Dokumen_KP");
                    Console.WriteLine("6. Kelola Data Permintaan KP");
                    Console.WriteLine("7. Kelola Data Laporan KP");
                    Console.WriteLine("8. Kelola Data Surat Selesai KP");
                    Console.WriteLine("9. Kelola Data Persetujuan SKP");
                    Console.WriteLine("0. Exit");
                    Console.WriteLine("\n Enter your choice (0-9): ");
                    char ch = Convert.ToChar(Console.ReadLine());
                    switch (ch)
                    {
                        case '1':
                            {
                                vMHS vmhs = new vMHS();
                                vmhs.Main();
                            }
                            break;
                        case '2':
                            {
                                vDSN vdsn = new vDSN();
                                vdsn.Main();
                            }
                            break;
                        case '3':
                            {
                                vTRSKRPNL vTRSKRPNL = new vTRSKRPNL();
                                vTRSKRPNL.Main();
                            }
                            break;
                        case '4':
                            {
                                vPRS vPRS = new vPRS();
                                vPRS.Main();
                            }
                            break;
                        case '5':
                            {
                                vDOKKP vDOKKP = new vDOKKP();
                                vDOKKP.Main();
                            }
                            break;
                        case '6':
                            {
                                vPRMTKP vPRMTKP = new vPRMTKP();
                                vPRMTKP.Main();
                            }
                            break;
                        case '7':
                            {
                                vLPRNKP vLPRNKP = new vLPRNKP();
                                vLPRNKP.Main();
                            }
                            break;
                        case '8':
                            {
                                vSRTSLS vSRTSLS = new vSRTSLS();
                                vSRTSLS.Main();
                            }
                            break;
                        case '9':
                            {
                                vPRSTJSKP vPRSTJSKP = new vPRSTJSKP();
                                vPRSTJSKP.Main();
                            }
                            break;
                        case '0':
                            conn.Close();
                            Console.Clear();
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\n Invalid option");
                            }
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
    }
}
