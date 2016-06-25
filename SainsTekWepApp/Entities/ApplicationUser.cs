using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SainsTekWepApp.Entities.Interfaces;
using SainsTekWepApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SainsTekWepApp.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public ESexType Sex { get; set; }

        public string FrontTitle { get; set; }

        public string BehindTitle { get; set; }

        public string MailAddress { get; set; }

        public string Description { get; set; }

        public EParticipantPaymentStatus ParticipantPaymentStatus { get; set; }

        public string ParticipantPaymentConfirmMessage { get; set; }

        public DateTime RegisterDate { get; set; }

        public virtual UserImage UserImage { get; set; }

        public virtual ICollection<Makalah> ListMakalah { get; set; }

        public virtual ICollection<PembayaranPeserta> ListPembayaranPeserta { get; set; }

        public virtual ICollection<Pengumuman> ListPengumuman { get; set; }

        public virtual ICollection<Page> ListPage { get; set; }

        public virtual ICollection<PesanPublikToAdmin> ListPesanPublikToAdmin { get; set; }

        public virtual ICollection<PesanUserToAdmin> ListPesanUserToAdminByUserId { get; set; }

        public virtual ICollection<PesanUserToAdmin> ListPesanUserToAdminByAdminReaderId { get; set; }

        public virtual ICollection<PesanAdminToUser> ListPesanAdminToUserByAdminId { get; set; }

        public virtual ICollection<PesanAdminToUser> ListPesanAdminToUserByUserId { get; set; }

        public ApplicationUser()
        {
            ParticipantPaymentStatus = EParticipantPaymentStatus.Baru;
            ParticipantPaymentConfirmMessage = string.Empty;
            RegisterDate = DateTime.UtcNow;
        }
    }
}