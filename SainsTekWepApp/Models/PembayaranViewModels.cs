using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SainsTekWepApp.Enums;
using System.Web.Mvc;

namespace SainsTekWepApp.Models
{
    public class PaymentConfirmationViewModel
    {
        private readonly List<string> listBankNames;

        private IEnumerable<SelectListItem> listPriceCategory;

        public PaymentConfirmationViewModel()
        {
            listBankNames = new List<string>
            {
                "Bank BRI",
                "Bank BNI",
                "Bank BCA",
                "Bank Mandiri",
                "Bank Danamon",
                "Bank Bukopin",
                "Bank Mega",
                "Bank Permata",
                "Bank NTT",
                "Bank Niaga"
            };
        }

        public IEnumerable<SelectListItem> ListBankNames
        {
            get { return new SelectList(listBankNames.OrderBy(x => x)); }
        }

        //public IEnumerable<System.Web.Mvc.SelectListItem> ListPriceCategory { get; set; }

        public IEnumerable<SelectListItem> ListPriceCategory 
        {
            get { return listPriceCategory; }
            set { listPriceCategory = value; } 
        }

        public int MakalahId { get; set; }

        [Display(Name = "Kategori harga")]
        public int PriceLevelCategory { get; set; }
        //public EPriceLevelCategory PriceLevelCategory { get; set; }

        [Display(Name = "Gambar identitas tambahan")]
        [DataType(DataType.Upload)]
        //[MaxFileSize(1 * 1024 * 1024, ErrorMessage = "Maksimal ukuran file Gambar identitas tambahan adalah {0} bytes (1 MB)")]
        public HttpPostedFileBase AttachmentFileUpload { get; set; }

        [Display(Name = "Nama bank pengirim")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public string BankSenderName { get; set; }

        //[Display(Name = "Nama pengirim")]
        //[Required(ErrorMessage = "{0} harus diisi.")]
        //public string BankAccountName { get; set; }

        //[Display(Name = "No. rekening bank pengirim")]
        //[DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "{0} harus diisi.")]
        //[StringLength(30, ErrorMessage = "{0} minimal {2} angka dan maksimal {1} angka.", MinimumLength = 10)]
        //[RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "{0} tidak valid.")]
        //public string BankAccountNumber { get; set; }

        [Display(Name = "Jumlah transfer (Rp)")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [Range(100000, 350000, ErrorMessage="{0} adalah {1:C2} sampai {2:C2} sesuai jenis dan kategori harga yang dipilih.")]

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        //[StringLength(6, ErrorMessage="{0} harus 6 angka.", MinimumLength=6)]
        //[RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "{0} tidak valid.")]

        public decimal TransferAmount { get; set; }

        [Display(Name = "Gambar bukti transfer")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Belum ada bukti transfer yang diunggah.")]
        //[MaxFileSize(1 * 1024 * 1024, ErrorMessage = "Maksimal ukuran file Gambar bukti transfer adalah {0} bytes (1 MB)")]
        public HttpPostedFileBase TransferSlipFileUpload { get; set; }

        [Display(Name = "Keterangan")]
        [StringLength(500, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string Description { get; set; }

        //ValidationField untuk memberikan batasan nilai min dan maks transfer amount pada pembayaran makalah dan peserta
        //public int MinTransferAmount { get; set; }

        //public int MaxTransferAmount { get; set; }

        //public string StringMinTransferAmount { get; set; }

        //public string StringMaxTransferAmount { get; set; }
    }

}