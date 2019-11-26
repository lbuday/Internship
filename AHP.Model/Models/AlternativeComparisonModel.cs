using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class AlternativeComparisonModel : IAlternativeComparisonModel
    {
        public System.Guid CriteriaID { get; set; }
        public System.Guid AlternativeID1 { get; set; }
        public System.Guid AlternativeID2 { get; set; }
        public double AlternativeRatio { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
    }
}
