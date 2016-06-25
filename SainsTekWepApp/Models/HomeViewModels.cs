using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Models
{
    public class MailMessageViewModel
    {
        [Display(Name = "Nama")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(20, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 1)]
        public string name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [StringLength(40, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string email { get; set; }

        [Display(Name = "Pesan")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(200, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 1)]
        public string message { get; set; }
    }
}