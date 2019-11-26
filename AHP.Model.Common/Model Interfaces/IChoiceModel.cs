using System;
using System.Collections.Generic;

namespace AHP.Model.Common
{
    public interface IChoiceModel
    {
        ICollection<IAlternativeModel> Alternatives { get; set; }
        Guid ChoiceID { get; set; }
        string ChoiceName { get; set; }
        ICollection<ICriterionModel> Criteria { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        Guid UserID { get; set; }
    }
}