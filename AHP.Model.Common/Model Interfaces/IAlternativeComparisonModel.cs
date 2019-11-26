using System;

namespace AHP.Model.Common
{
    public interface IAlternativeComparisonModel
    {
        Guid AlternativeID1 { get; set; }
        Guid AlternativeID2 { get; set; }
        double AlternativeRatio { get; set; }
        Guid CriteriaID { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}