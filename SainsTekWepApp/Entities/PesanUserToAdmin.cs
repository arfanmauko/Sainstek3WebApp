using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("PesanUserToAdmin")]
    public class PesanUserToAdmin : IPesan
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string MessageTitle { get; set; }    
            
        public string MessageContent { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateRead { get; set; }

        public string AdminReaderId { get; set; }

        public virtual ApplicationUser ApplicationUserByUserId { get; set; }

        public virtual ApplicationUser ApplicationUserByAdminReaderId { get; set; }
    }
}