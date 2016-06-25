using SainsTekWepApp.Enums;
using System;

namespace SainsTekWepApp.Entities.Interfaces
{
    public interface IMakalah
    {
        int Id { get; set; }

        int CategoryId { get; set; }

        string Title { get; set; }

        string Abstract { get; set; }

        string Description { get; set; }

        DateTime DateSubmitted { get; set; }

        DateTime DateModified { get; set; }

        string AbstractFileName { get; set; }

        EPaperAbstractStatus PaperAbstractStatus { get; set; }

        string PaperAbstractConfirmMessage { get; set; }

        string FullPaperFileName { get; set; }

        DateTime? FullPaperDateSubmitted { get; set; }

        EFullPaperStatus FullPaperStatus { get; set; }

        string FullPaperConfirmMessage { get; set; }
    }
}
