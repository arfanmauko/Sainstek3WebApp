using System;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IPengumuman
    {
        int Id { get; set; }

        string Title { get; set; }

        string Content { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DatePublished { get; set; }

        string UserId { get; set; }

    }
}
