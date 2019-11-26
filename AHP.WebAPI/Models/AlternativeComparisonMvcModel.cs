using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class AlternativeComparisonMvcModel
    {
        [Display(Name = "Criterion")]
        public string CriteriaName { get; set; }
        public Guid CriteriaID { get; set; }
        [Display(Name = "First alternative")]
        public string AlternativeName1 { get; set; }
        public Guid AlternativeID1 { get; set; }
        [Display(Name = "Second alternative")]
        public string AlternativeName2 { get; set; }
        public Guid AlternativeID2 { get; set; }
        [Display(Name = "Ratio")]
        public double AlternativeRatio { get; set; }
        public Nullable<bool> Flipped { get; set; }

    }
}