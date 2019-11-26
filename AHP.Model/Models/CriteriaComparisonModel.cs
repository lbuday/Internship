using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class CriteriaComparisonModel : ICriteriaComparisonModel
    {
        public System.Guid CriteriaID1 { get; set; }
        public System.Guid CriteriaID2 { get; set; }
        public double CriteriaRatio { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
    }
}
