using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IPublikPesanToAdmin
    {
        int Id { get; set; }

        string SenderName { get; set; }

        string SenderEmail { get; set; }

        string MessageContent { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DateAdminRead { get; set; }

        string ReplyMessage { get; set; }

        DateTime? DateReply { get; set; }

        string ReplyAdminId { get; set; }
    }
}
