using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Entities
{
    [Table("Pengumuman")]
    public class Pengumuman : IPengumuman
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DatePublished { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}