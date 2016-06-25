using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities.Interfaces
{
    public class IPembayaran
    {
        int Id { get; set; }

        string UserId { get; set; }

        string BankSenderName { get; set; }

        string BankAccountName { get; set; }

        string BankAccountNumber { get; set; }

        decimal TransferAmount { get; set; }

        string FileName { get; set; }

        bool IsAdminConfirmed { get; set; }

        bool ConfirmationResult { get; set; }

        string ConfirmationMessage { get; set; }

        DateTime UserConfirmationDate { get; set; }

        DateTime? AdminConfirmationDate { get; set; }

        string Description { get; set; }
    }
}