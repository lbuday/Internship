using System;
using System.Collections.Generic;

namespace AHP.Model.Common
{
    public interface IAlternativeModel
    {
        ICollection<IAlternativeComparisonModel> AlternativeComparisons { get; set; }
        ICollection<IAlternativeComparisonModel> AlternativeComparisons1 { get; set; }
        Guid AlternativeID { get; set; }
        string AlternativeName { get; set; }
        double? AlternativeScore { get; set; }
        Guid ChoiceID { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}