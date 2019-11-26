using System;

namespace AHP.Model.Common
{
    public interface ICriteriaComparisonModel
    {
        Guid CriteriaID1 { get; set; }
        Guid CriteriaID2 { get; set; }
        double CriteriaRatio { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}