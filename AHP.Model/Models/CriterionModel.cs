using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class CriterionModel : ICriterionModel
    {
        public System.Guid CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public Nullable<double> CriteriaScore { get; set; }
        public System.Guid ChoiceID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
    }
}
