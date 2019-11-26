using AHP.DAL;
using AHP.Model;
using AHP.Model.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<User, IUserModel>();
            CreateMap<IUserModel, User>();

            CreateMap<Choice, IChoiceModel>();
            CreateMap<IChoiceModel, Choice>();

            CreateMap<Criterion, ICriterionModel>();
            CreateMap<ICriterionModel, Criterion>();

            CreateMap<Alternative, IAlternativeModel>();
            CreateMap<IAlternativeModel, Alternative>();

            CreateMap<CriteriaComparison, ICriteriaComparisonModel>();
            CreateMap<ICriteriaComparisonModel, CriteriaComparison>();

            CreateMap<AlternativeComparison, IAlternativeComparisonModel>();
            CreateMap<IAlternativeComparisonModel, AlternativeComparison>();

        }
    }
}

