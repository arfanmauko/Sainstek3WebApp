using SainsTekWepApp.Entities.Interfaces;
using SainsTekWepApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities.Abstracts
{
    public abstract class Pembayaran : IPembayaran
    {
        public int Id { get; set; }

        public EPriceLevelCategory PriceLevelCategory { get; set; }

        public string AttachmentFileName { get; set; }

        public string BankSenderName { get; set; }

        public decimal TransferAmount { get; set; }

        public string SenderMessage { get; set; }

        public string SlipFileName { get; set; }

        public bool IsAdminConfirmed { get; set; }

        public bool ConfirmationResult { get; set; }

        public string ConfirmationMessage { get; set; }

        public DateTime UserConfirmationDate { get; set; }

        public DateTime? AdminConfirmationDate { get; set; }

        public string Description { get; set; }
    }
}