using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class CriterionMvcModel
    {
        public Guid CriteriaID { get; set; }
        public Guid ChoiceID { get; set; }
        [Display(Name = "Name")]
        public string CriteriaName { get; set; }
        public Nullable<double> CriteriaScore { get; set; }
        [Display(Name = "Updated")]
        public DateTime DateUpdated { get; set; }
    }
}