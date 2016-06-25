using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("UserImage")]
    public class UserImage
    {
        public string UserId { get; set; }

        public string PhotoFileName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}