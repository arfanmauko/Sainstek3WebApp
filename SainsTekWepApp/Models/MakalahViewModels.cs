using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using SainsTekWepApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Models
{
    public abstract class BaseMakalahViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly List<KategoriMakalah> _listKategoriJurnal;

        public BaseMakalahViewModel()
        {
            _listKategoriJurnal = db.KategoriMakalah.ToList();
        }
        public IEnumerable<SelectListItem> ListKategoriJurnal
        {
            get { return new SelectList(_listKategoriJurnal, "Id", "Name"); }
        }

        public int Id { get; set; }

        [Display(Name = "Kategori Makalah")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public int CategoryId { get; set; }

        [Display(Name = "Judul")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(100, ErrorMessage = "{0} maksimal {1} karakter.")]
        [AllowHtml]
        public string Title { get; set; }

        [Display(Name = "Abstrak")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(2000, ErrorMessage = "{0} maksimal {1} karakter.")]
        [AllowHtml]
        public string Abstract { get; set; }

        [Display(Name = "Keterangan")]
        [StringLength(300, ErrorMessage = "{0} maksimal {1} karakter.")]
        [AllowHtml]
        public string Description { get; set; }
    }

    public class ListMakalahViewModel : BaseMakalahViewModel
    {
        [Display(Name = "Nama File Abstrak")]
        public string AbstractFileName { get; set; }

        [Display(Name = "Tanggal Submit")]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Terakhir Update")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Status Abstrak")]
        public EPaperAbstractStatus PaperAbstractStatus { get; set; }

        [Display(Name = "Informasi Abstrak")]
        public string PaperAbstractConfirmMessage { get; set; }

         [Display(Name = "Nama File Full Paper")]
        public string FullPaperFileName { get; set; }

        [Display(Name = "Tanggal Submit Full Paper")]
        public DateTime? FullPaperDateSubmitted { get; set; }

        [Display(Name = "Status Full Paper")]
        public EFullPaperStatus FullPaperStatus { get; set; }

        [Display(Name = "Informasi Full Paper")]
        public string FullPaperConfirmMessage { get; set; }

        [Display(Name = "Status Pembayaran")]
        public EPaperPaymentStatus PaperPaymentStatus { get; set; }

        [Display(Name = "Informasi Status Pembayaran")]
        public string PaperPaymentConfirmMessage { get; set; }
    }

    public class CreateMakalahViewModel : BaseMakalahViewModel
    {
        [Display(Name = "File Abstrak (pdf)")]
        [Required(ErrorMessage = "Belum ada file abstrak yang diunggah.")]
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = "Maksimal ukuran file abstrak adalah {0} bytes (1 MB)")]
        public HttpPostedFileBase AbstractFileUpload { get; set; }

        [Display(Name = "File Full Paper (pdf)")]
        public HttpPostedFileBase FullPaperFileUpload { get; set; }
    }

    public class EditMakalahViewModel : BaseMakalahViewModel
    {
        [Display(Name = "Nama File Abstrak")]
        public string AbstractFileName { get; set; }

        [Display(Name = "File Abstrak baru (pdf)")]
        public HttpPostedFileBase AbstractFileUpload { get; set; }

        [Display(Name = "Nama File Full Paper")]
        public string FullPaperFileName { get; set; }

        [Display(Name = "File Full Paper baru (pdf)")]
        public HttpPostedFileBase FullPaperFileUpload { get; set; }
    }

    public class DeleteMakalahViewModel : BaseMakalahViewModel
    {
        [Display(Name = "Kategori")]
        public string CategoryName { get; set; }

        [Display(Name = "Tanggal Submit")]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Nama File Abstrak")]
        public string AbstractFileName { get; set; }

        [Display(Name = "Nama File Full Paper")]
        public string FullPaperFileName { get; set; }

         [Display(Name = "Status Pembayaran")]
        public EPaperPaymentStatus PaperPaymentStatus { get; set; }

        [Display(Name = "Informasi Status Pembayaran")]
        public string PaperPaymentConfirmMessage { get; set; }
    }

    public class DetailMakalahViewModel : BaseMakalahViewModel
    {
        [Display(Name = "Kategori")]
        public string CategoryName { get; set; }

        [Display(Name = "Nama File Abstrak")]
        public string AbstractFileName { get; set; }

        [Display(Name = "Tanggal Submit")]
        public DateTime DateSubmitted { get; set; }

        [Display(Name = "Terakhir Update")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Status Abstrak")]
        public EPaperAbstractStatus PaperAbstractStatus { get; set; }

        [Display(Name = "Informasi Abstrak")]
        public string PaperAbstractConfirmMessage { get; set; }

        [Display(Name = "Nama File Full Paper")]
        public string FullPaperFileName { get; set; }

        [Display(Name = "Tanggal Submit Full Paper")]
        public DateTime? FullPaperDateSubmitted { get; set; }

        [Display(Name = "Status Full Paper")]
        public EFullPaperStatus FullPaperStatus { get; set; }

        [Display(Name = "Informasi Full Paper")]
        public string FullPaperConfirmMessage { get; set; }

        [Display(Name = "Status Pembayaran")]
        public EPaperPaymentStatus PaperPaymentStatus { get; set; }

        [Display(Name = "Informasi Status Pembayaran")]
        public string PaperPaymentConfirmMessage { get; set; }
    }
}