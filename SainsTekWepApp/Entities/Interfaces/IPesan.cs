using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IPesan
    {
        int Id { get; set; }

        string MessageTitle { get; set; }

        string MessageContent { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DateRead { get; set; }
    }
}
