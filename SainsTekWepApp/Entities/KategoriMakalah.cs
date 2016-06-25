using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("KategoriMakalah")]
    public class KategoriMakalah : IKategoriMakalah
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Makalah> ListMakalah { get; set; }
    }
}