﻿using SainsTekWepApp.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SainsTekWepApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "{0} harus diisi.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "{0} harus diisi.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "{0} harus diisi.")]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        private readonly List<string> _listKategoriUser;

        public RegisterViewModel()
        {
            _listKategoriUser = new List<string> { "Penulis", "Peserta" };
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> ListKategoriUser
        {
            get { return new System.Web.Mvc.SelectList(_listKategoriUser); }
        }

        [Display(Name = "Nama depan")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(20, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Nama tengah")]
        [StringLength(20, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string MiddleName { get; set; }

        [Display(Name = "Nama belakang")]
        [StringLength(20, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string LastName { get; set; }

        [Display(Name = "Email (Username anda)")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [StringLength(40, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(100, ErrorMessage = "{0} minimal {2} karakter.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi password")]
        [Compare("Password", ErrorMessage = "{0} dan {1} tidak sama.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Jenis kelamin")]
        public ESexType Sex { get; set; }

        [Display(Name = "Gelar depan")]
        [StringLength(10, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string FrontTitle { get; set; }

        [Display(Name = "Gelar belakang")]
        [StringLength(20, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string BehindTitle { get; set; }

        [Display(Name = "No Telepon")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(14, ErrorMessage = "{0} minimal {2} angka dan maksimal {1} angka.", MinimumLength = 10)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "{0} tidak valid.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Alamat surat")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(100, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 20)]
        public string MailAddress { get; set; }

        [Display(Name = "Deskripsi diri")]
        [StringLength(100, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string Description { get; set; }

        [Display(Name = "Daftar sebagai")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        public string UserCategory { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(100, ErrorMessage = "{0} minimal {2} karakter.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi password")]
        [Compare("Password", ErrorMessage = "{0} dan {1} tidak sama.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
