using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class ChoiceMvcModel
    {
        public Guid ChoiceID { get; set; }
        public Guid UserID { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "You need a name")]
        public string Name { get; set; }
        [Display(Name = "Updated")]
        public DateTime DateUpdated { get; set; }
    }
}