using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SainsTekWepApp.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SainsTekWepApp.Entities
{
    [Table("Makalah")]
    public class Makalah : IMakalah
    {
        public int Id { get; set; }

        public string userId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Abstract { get; set; }

        public DateTime DateSubmitted { get; set; }

        public DateTime DateModified { get; set; }

        public string Description { get; set; }

        public string AbstractFileName { get; set; }

        public EPaperAbstractStatus PaperAbstractStatus { get; set; }

        public string PaperAbstractConfirmMessage { get; set; }

        public string FullPaperFileName { get; set; }

        public DateTime? FullPaperDateSubmitted { get; set; }

        public EFullPaperStatus FullPaperStatus { get; set; }

        public string FullPaperConfirmMessage { get; set; }

        public EPaperPaymentStatus PaperPaymentStatus { get; set; }

        public string PaperPaymentConfirmMessage { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual KategoriMakalah KategoriMakalah { get; set; }

        public virtual ICollection<PembayaranMakalah> ListPembayaranMakalah { get; set; }

        public Makalah()
        {
            DateSubmitted = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }
    }
}