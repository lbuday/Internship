using AHP.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class AlternativeModel : IAlternativeModel
    {
        public ICollection<Common.IAlternativeComparisonModel> AlternativeComparisons { get; set; }
        public ICollection<Common.IAlternativeComparisonModel> AlternativeComparisons1 { get; set; }
        public System.Guid AlternativeID { get; set; }
        public string AlternativeName { get; set; }
        public Nullable<double> AlternativeScore { get; set; }
        public System.Guid ChoiceID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }

    }
}
