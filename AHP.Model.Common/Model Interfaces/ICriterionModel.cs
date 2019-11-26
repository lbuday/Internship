using System;

namespace AHP.Model.Common
{
    public interface ICriterionModel
    {
        Guid ChoiceID { get; set; }
        Guid CriteriaID { get; set; }
        string CriteriaName { get; set; }
        double? CriteriaScore { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}