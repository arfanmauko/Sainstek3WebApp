using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SainsTekWepApp.Entities.Interfaces
{
    interface IUserImage
    {
        string USerId { get; set; }

        string PhotoFileName { get; set; }
    }
}
