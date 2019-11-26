using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class CriteriaComparisonMvcModel
    {
        [Display(Name = "First criterion")]
        public string CriterionName1 { get; set; }
        public Guid CriterionID1 { get; set; }
        [Display(Name = "Second criterion")]
        public string CriterionName2 { get; set; }
        public Guid CriterionID2 { get; set; }
        [Display(Name = "Ratio")]
        public double CriteriaRatio { get; set; }
        public Nullable<bool> Flipped { get; set; }

    }
}