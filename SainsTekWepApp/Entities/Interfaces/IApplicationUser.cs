using SainsTekWepApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IApplicationUser
    {
        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        ESexType Sex { get; set; }

        string FrontTitle { get; set; }

        string BehindTitle { get; set; }

        string MailAddress { get; set; }

        string Description { get; set; }
    }
}