using System;
using System.Collections.Generic;
using PoinSiswa.Library.Model;
using PoinSiswa.Library.Service;
using PoinSiswa.Library.Configuration;
using PoinSiswa.Library.TableDriven;
using PoinSiswa.Library.Components;
using Tubes_Tahap1_KPL_kelompok3.Components;
using Tubes_Tahap1_KPL_kelompok3.Configuration;
using Tubes_Tahap1_KPL_kelompok3.Model;
using Tubes_Tahap1_KPL_kelompok3.table_driven;

namespace PoinSiswa.App
{
    class Program
    {
        static SiswaService siswaService = new SiswaService();
        static PelanggaranService pelanggaranService = new PelanggaranService();
        static ConfigManager configManager = new ConfigManager();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== SISTEM POIN SISWA =====");
                Console.WriteLine("1. Manajemen Siswa");
                Console.WriteLine("2. Manajemen Pelanggaran");
                Console.WriteLine("3. Statistik");
                Console.WriteLine("4. Konfigurasi");
                Console.WriteLine("0. Keluar");
                Console.Write("Pilih menu: ");
                string menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        MenuManajemenSiswa();
                        break;
                    case "2":
                        MenuManajemenPelanggaran();
                        break;
                    case "3":
                        MenuStatistik();
                        break;
                    case "4":
                        MenuKonfigurasi();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }

                Console.WriteLine("\nTekan Enter untuk kembali ke menu...");
                Console.ReadLine();
            }
        }

        static void MenuManajemenSiswa()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== MANAJEMEN SISWA =====");
                Console.WriteLine("1. Tambah Siswa");
                Console.WriteLine("2. Lihat Semua Siswa");
                Console.WriteLine("3. Cari Siswa (berdasarkan NIS)");
                Console.WriteLine("0. Kembali ke Menu Utama");
                Console.Write("Pilih menu: ");
                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        TambahSiswa();
                        break;
                    case "2":
                        LihatSemuaSiswa();
                        break;
                    case "3":
                        CariSiswa();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }

                Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                Console.ReadLine();
            }
        }

        static void MenuManajemenPelanggaran()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== MANAJEMEN PELANGGARAN =====");
                Console.WriteLine("1. Tambah Pelanggaran");
                Console.WriteLine("2. Lihat Semua Pelanggaran");
                Console.WriteLine("3. Lihat Pelanggaran per Siswa");
                Console.WriteLine("4. Ubah Status Pelanggaran");
                Console.WriteLine("0. Kembali ke Menu Utama");
                Console.Write("Pilih menu: ");
                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        TambahPelanggaran();
                        break;
                    case "2":
                        LihatSemuaPelanggaran();
                        break;
                    case "3":
                        LihatPelanggaran();
                        break;
                    case "4":
                        UbahStatusPelanggaran();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }

                Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                Console.ReadLine();
            }
        }

        static void MenuStatistik()
        {
            Console.Clear();
            Console.WriteLine("===== STATISTIK =====");
            Console.WriteLine("1. Lihat Total Poin Siswa");
            Console.WriteLine("2. Lihat Jumlah Pelanggaran per Kelas");
            Console.WriteLine("0. Kembali ke Menu Utama");
            Console.Write("Pilih menu: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    LihatSemuaSiswa(); // Menampilkan poin siswa
                    break;
                case "2":
                    Console.WriteLine("Fitur belum diimplementasikan.");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }
        }

        static void MenuKonfigurasi()
        {
            Console.Clear();
            Console.WriteLine("===== KONFIGURASI SISTEM =====");
            Console.WriteLine("1. Tampilkan Konfigurasi Aktif");
            Console.WriteLine("2. Ubah Batas Poin");
            Console.WriteLine("0. Kembali ke Menu Utama");
            Console.Write("Pilih menu: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    configManager.TampilkanKonfigurasi();
                    break;
                case "2":
                    configManager.UbahBatasPoin();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }
        }

        static void TambahSiswa()
        {
            Console.Write("Nama: ");
            string nama = Console.ReadLine();
            Console.Write("Kelas: ");
            string kelas = Console.ReadLine();
            var siswa = new Siswa { Id = Guid.NewGuid().GetHashCode(), Nama = nama, Kelas = kelas };
            siswaService.TambahSiswa(siswa);
            Console.WriteLine("Siswa berhasil ditambahkan.");
        }

        static void LihatSemuaSiswa()
        {
            var semuaSiswa = siswaService.GetType()
                .GetField("_daftarSiswa", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(siswaService) as List<Siswa>;

            if (semuaSiswa == null || semuaSiswa.Count == 0)
            {
                Console.WriteLine("Belum ada data siswa.");
                return;
            }

            TableRenderer.Render(semuaSiswa, new Dictionary<string, Func<Siswa, string>>
            {
                { "ID", s => s.Id.ToString() },
                { "Nama", s => s.Nama },
                { "Kelas", s => s.Kelas },
                { "Total Poin", s => s.TotalPoin.ToString() }
            });
        }

        static void CariSiswa()
        {
            Console.Write("Masukkan ID Siswa (NIS): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID tidak valid.");
                return;
            }

            var siswa = siswaService.CariSiswa(id);
            if (siswa == null)
            {
                Console.WriteLine("Siswa tidak ditemukan.");
                return;
            }

            Console.WriteLine($"Nama: {siswa.Nama}, Kelas: {siswa.Kelas}, Total Poin: {siswa.TotalPoin}");
        }

        static void TambahPelanggaran()
        {
            Console.Write("ID Siswa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID tidak valid.");
                return;
            }

            var siswa = siswaService.CariSiswa(id);
            if (siswa == null)
            {
                Console.WriteLine("Siswa tidak ditemukan.");
                return;
            }

            Console.Write("Jenis Pelanggaran: ");
            string jenis = Console.ReadLine();

            try
            {
                int poin = TabelPelanggaran.GetPoin(jenis);
                var pelanggaran = new Pelanggaran
                {
                    Id = Guid.NewGuid().GetHashCode(),
                    SiswaId = siswa.Id,
                    Jenis = jenis,
                    Poin = poin,
                    Tanggal = DateTime.Now,
                    Status = StatusPelanggaran.DILAPORKAN
                };

                pelanggaranService.TambahPelanggaran(siswa, pelanggaran);
                Console.WriteLine("Pelanggaran berhasil ditambahkan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void LihatPelanggaran()
        {
            Console.Write("ID Siswa: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID tidak valid.");
                return;
            }

            var siswa = siswaService.CariSiswa(id);
            if (siswa == null)
            {
                Console.WriteLine("Siswa tidak ditemukan.");
                return;
            }

            var riwayat = siswa.RiwayatPelanggaran;
            if (riwayat.Count == 0)
            {
                Console.WriteLine("Tidak ada riwayat pelanggaran.");
                return;
            }

            TableRenderer.Render(riwayat, new Dictionary<string, Func<Pelanggaran, string>>
            {
                { "Tanggal", p => p.Tanggal.ToShortDateString() },
                { "Jenis", p => p.Jenis },
                { "Poin", p => p.Poin.ToString() },
                { "Status", p => p.Status.ToString() }
            });
        }

        static void LihatSemuaPelanggaran()
        {
            pelanggaranService.TampilkanSemuaPelanggaran(); // Harus disiapkan dalam service
        }

        static void UbahStatusPelanggaran()
        {
            Console.WriteLine("Fitur ubah status pelanggaran belum diimplementasikan.");
        }
    }
}
