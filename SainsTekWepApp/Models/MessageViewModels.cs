using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Models
{
    public class MessageViewModels
    {
        public int Id { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class SendMessageViewModel
    {
        [Display(Name = "Judul")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(20, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 1)]
        public string MessageTitle { get; set; }

        [Display(Name = "Pesan")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [StringLength(500, ErrorMessage = "{0} minimal {2} karakter dan maksimal {1} karakter.", MinimumLength = 10)]
        public string MessageContent { get; set; }
        
    }
}