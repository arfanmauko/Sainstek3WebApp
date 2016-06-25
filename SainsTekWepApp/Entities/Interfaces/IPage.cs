using System;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IPage
    {
        int Id { get; set; }

        string Name { get; set; }

        string Content { get; set; }

         DateTime DateCreated { get; set; }

         string UserId { get; set; }
    }
}
