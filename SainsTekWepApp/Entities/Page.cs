using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("Page")]
    public class Page : IPage
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Content { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}