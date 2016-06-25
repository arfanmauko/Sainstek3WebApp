using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SainsTekWepApp.Enums;
using SainsTekWepApp.Entities.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace SainsTekWepApp.Entities
{

    [Table("PembayaranMakalah")]
    public class PembayaranMakalah : Pembayaran
    {
        public int MakalahId { get; set; }

        public virtual Makalah Makalah { get; set; }

    }
}