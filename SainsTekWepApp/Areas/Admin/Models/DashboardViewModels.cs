using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Areas.Admin.Models
{
    public class DashboardViewModels
    {
        //Menyatakan Jumlah Pendaftar Baru
        public int NewParticipantCount { get; set; }

        //Menyatakan Jumlah Konfirmasi Peserta Baru
        public int NewParticipantPaymentConfirmationCount { get; set; }

        //Menyatakan Jumlah Abstrak Baru
        public int NewPaperAbstractCount { get; set; }

        //Menyatakan Jumlah Abstrak Edited Baru
        public int NewPaperAbstractEditCount { get; set; }

        //Menyatakan Jumlah Full Papper Baru
        public int NewFullPaperCount { get; set; }

        //Menyatakan Jumlah Full Papper Edited Baru
        public int NewFullPaperEditCount { get; set; }

        //Menyatakan Jumlah Konfirmasi Pembayaran Paper  Baru
        public int NewPaperPaymentConfirmationCount { get; set; }

        public int NewMessagesCount { get; set; }


    }
}

