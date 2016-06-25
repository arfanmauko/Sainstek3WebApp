using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SainsTekWepApp.Enums;
using SainsTekWepApp.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SainsTekWepApp
{
    public static class GlobalData
    {
        public static int TitleMaxWordsCount
        {
            get { return 15; }
        }

        public static int AbstractMaxWordsCount
        {
            get { return 300; }
        }

        public static string AbstractFileSavePath
        {
            get { return @"~/Uploads/Makalah/Abstract"; }
        }

        public static string PaperFileSavePath
        {
            get { return @"~/Uploads/Makalah/FullPaper"; }
        }

        public static string PaperPaymentConfirmationImageFileSavePath
        {
            get { return @"~/Uploads/Images/Konfirmasi/Makalah"; }
        }

        public static string IdentityPaymentConfirmationImageFileSavePath
        {
            get { return @"~/Uploads/Images/Konfirmasi/Identitas"; }
        }

        public static string ParticipantPaymentConfirmationImageFileSavePath
        {
            get { return @"~/Uploads/Images/Konfirmasi/Peserta"; }
        }

        public static string DownloadFilePath
        {
            get { return @"~/Data/Downloads/Files"; }
        }

        public static string DownloadImagePath
        {
            get { return @"~/Data/Downloads/Images"; }
        }




        public static string GetPaperAbstractStatusMessage(EPaperAbstractStatus paperAbstractStatus)
        {
            switch (paperAbstractStatus)
            {
                case EPaperAbstractStatus.Baru:
                    return "Menunggu proses konfirmasi oleh panitia.";
                case EPaperAbstractStatus.Edit:
                    return "Menunggu proses konfirmasi hasil perubahan oleh panitia.";
                case EPaperAbstractStatus.Ditolak:
                    return "Mohon maaf abstrak paper anda belum diterima. Silahkan diperiksa lagi.";
                case EPaperAbstractStatus.Diterima:
                    return "Abstrak disetujui.";
                default:
                    return string.Empty;
            }
        }

        public static string GetFullPaperStatusMessage(EFullPaperStatus fullPaperStatus)
        {
            switch (fullPaperStatus)
            {
                case EFullPaperStatus.Kosong:
                    return "Masih kosong.";
                case EFullPaperStatus.Baru:
                    return "Menunggu proses konfirmasi oleh panitia.";
                case EFullPaperStatus.Edit:
                    return "Menunggu proses konfirmasi hasil perubahan oleh panitia.";
                case EFullPaperStatus.Ditolak:
                    return "Mohon maaf Full paper anda belum diterima. Silahkan diperiksa lagi.";
                case EFullPaperStatus.Diterima:
                    return "Full paper disetujui.";
                default:
                    return string.Empty;
            }
        }

        public static string GetPaperPaymentStatusMessage(EPaperPaymentStatus paperPaymentStatus)
        {
            switch (paperPaymentStatus)
            {
                case EPaperPaymentStatus.BelumBayar:
                    return "Belum bayar.";
                case EPaperPaymentStatus.KonfirmasiPembayaran:
                    return "Menunggu proses konfirmasi pembayaran oleh panitia.";
                case EPaperPaymentStatus.Ditolak:
                    return "Mohon maaf pembayaran anda belum valid. Pastikan informasi proses konfirmasi anda sudah benar dengan melakukan proses konfirmasi pembayaran lagi.";
                case EPaperPaymentStatus.Diterima:
                    return "Lunas.";
                default:
                    return string.Empty;
            }
        }

        public static string GetParticipantPaymentStatusMessage(EParticipantPaymentStatus paperPaymentStatus)
        {
            switch (paperPaymentStatus)
            {
                case EParticipantPaymentStatus.Baru:
                    return "Silahkan melakukan pembayaran untuk mendaftarkan diri anda sebagai peserta seminar nasional SainsTek III.";
                case EParticipantPaymentStatus.KonfirmasiPembayaran:
                    return "Proses pembayaran anda dalam tahap menunggu konfirmasi dari kami.";
                case EParticipantPaymentStatus.Ditolak:
                    return "Mohon maaf pembayaran anda belum valid. Pastikan informasi proses konfirmasi anda sudah benar dengan melakukan proses konfirmasi pembayaran lagi.";
                case EParticipantPaymentStatus.Diterima:
                    return "Terima kasih telah bergabung dengan kami sebagai peserta pada seminar nasional SainsTek III.";
                default:
                    return string.Empty;
            }
        }

        public static decimal GetHargaMakalah(EPriceLevelCategory priceCategory, bool moreThanOne)
        {
            decimal result = 350000;

            if (moreThanOne)
            {
                return 250000;
            }

            switch (priceCategory)
            {
                case EPriceLevelCategory.Dosen:
                    result = 350000;
                    break;
                case EPriceLevelCategory.MahasiswaPascaSarjana:
                    result = 300000;
                    break;
                default: break;
            }

            return result;
        }

        public static decimal GetHargaPeserta(EPriceLevelCategory priceCategory)
        {
            decimal result = 250000;

            switch (priceCategory)
            {
                case EPriceLevelCategory.Dosen:
                    result = 250000;
                    break;
                case EPriceLevelCategory.MahasiswaPascaSarjana:
                    result = 200000;
                    break;
                case EPriceLevelCategory.MahasiswaS1:
                    result = 100000;
                    break;
                default: break;
            }

            return result;
        }

        public static List<PriceCategory> ListPriceCategory
        {
            get
            {
                List<PriceCategory> listPriceCategory = new List<PriceCategory>();
                listPriceCategory.Add(new PriceCategory
                {
                    Id = 0,
                    Name = "Dosen/Peneliti/Profesional"
                });
                listPriceCategory.Add(new PriceCategory
                {
                    Id = 1,
                    Name = "Mahasiswa Pasca Sarjana"
                });

                listPriceCategory.Add(new PriceCategory
                {
                    Id = 2,
                    Name = "Mahasiswa Sarjana (S1)"
                });

                return listPriceCategory;
            }
        }

        public class PriceCategory
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}