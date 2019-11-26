using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class AlternativeMvcModel
    {
        public Guid AlternativeID { get; set; }
        public Guid ChoiceID { get; set; }
        [Display(Name = "Name")]
        public string AlternativeName { get; set; }
        public Nullable<double> Score { get; set; }
        [Display(Name = "Updated")]
        public DateTime DateUpdated { get; set; }
    }
}