using SainsTekWepApp.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace SainsTekWepApp.Models
{
    //UserProfileViewModel
    public class UserProfileViewModel
    {
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
        
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [StringLength(40, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string Email { get; set; }

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

        public EParticipantPaymentStatus ParticipantPaymentStatus { get; set; }

        public string ParticipantPaymentConfirmMessage { get; set; }
    }
}