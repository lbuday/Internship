using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHP.WebAPI.Models
{
    public class CompositeMvcModel
    {
        public List<AlternativeComparisonMvcModel> AltComps;

        public CompositeMvcModel() { this.AltComps = new List<AlternativeComparisonMvcModel>(); }

        void Add(AlternativeComparisonMvcModel a)
        {
            AltComps.Add(a);
        }

        bool Any()
        {
            return AltComps.Any();
        }
    }
}