using AHP.Model;
using AHP.Model.Common;
using AHP.Service.Common;
using AHP.WebAPI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AHP.WebAPI.Controllers
{
    public class CriteriaComparisonController : Controller
    {
        ICriteriaComparisonService _criteriaComparisonService;
        ICriterionService _criterionService;
        IMapper _mapper;
        public CriteriaComparisonController(
            IMapper mapper,
            ICriteriaComparisonService criteriaComparisonService,
            ICriterionService criterionService
          )
        {
            _mapper = mapper;
            _criteriaComparisonService = criteriaComparisonService;
            _criterionService = criterionService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListCriteriaComparisons", "CriteriaComparison", new { page = Session["page"] });
        }

        public async Task<ActionResult> ListCriteriaComparisons(int page)
        {
            ViewBag.Title = "Criteria comparisons";
            if (page < 1) { page = 1; }
            Session["Page"] = page;

            var critcomps = await _criteriaComparisonService.GetAsync((Guid)Session["CriterionID"], page);
            var _critcomps = new List<CriteriaComparisonMvcModel>();
            Guid critID = (Guid)Session["CriterionID"];
            IDictionary<Guid, string> CritNames = (IDictionary<Guid, string>)Session["CriteriaNames"];

            foreach (ICriteriaComparisonModel critcomp in critcomps)
            {
                Guid critID1 = critcomp.CriteriaID1;
                Guid critID2 = critcomp.CriteriaID2;

                if (critID1 == critID)
                {
                    //OVO S NUOOM je samo za test
                    double newcritratio;
                    if (critcomp.CriteriaRatio != 0)
                    {
                        newcritratio = critcomp.CriteriaRatio;
                        //Pretvara u oblik koji je prihvatljiv sliderima
                        if (newcritratio < 1)
                        {
                            newcritratio = -(Math.Round(1 / critcomp.CriteriaRatio) - 1);
                        }
                        else
                        {
                            newcritratio -= 1;
                        }
                    }
                    else
                    {
                        newcritratio = 0;
                    }
                    CriteriaComparisonMvcModel cc = new CriteriaComparisonMvcModel
                    {
                        CriterionName1 = CritNames[critID1],
                        CriterionID1 = critID1,
                        CriterionID2 = critID2,
                        CriterionName2 = CritNames[critID2],
                        Flipped = false,
                        CriteriaRatio = newcritratio
                    };

                    _critcomps.Add(cc);
                }
                else
                {
                    double newcritratio;
                    if (critcomp.CriteriaRatio != 0)
                    {
                        newcritratio = 1 / critcomp.CriteriaRatio;
                        //OVO SA NULOM POSLJE OBRISAT
                        //Pretvara u oblik koji je prihvatljiv sliderima
                        if (newcritratio < 1)
                        {
                            newcritratio = -(Math.Round(1 / newcritratio) - 1);
                        }
                        else
                        {
                            newcritratio -= 1;
                        }
                    }
                    else
                    {
                        newcritratio = 0;
                    }

                    CriteriaComparisonMvcModel cc = new CriteriaComparisonMvcModel
                    {
                        CriterionName1 = CritNames[critID2],
                        CriterionID1 = critID2,
                        CriterionID2 = critID1,
                        CriterionName2 = CritNames[critID1],
                        Flipped = true,
                        CriteriaRatio = newcritratio
                    };

                    _critcomps.Add(cc);
                }
            }
            if (!_critcomps.Any() && page > 1)
            {
                Session["Page"] = page - 1;
                return RedirectToAction("ListCriteriaComparisons", "CriteriaComparison", new { page = Session["Page"] });
            }

            return View(_critcomps);
        }

        [HttpPost]
        public async Task<ActionResult> ListCriteriaComparisons(List<CriteriaComparisonMvcModel> comps)
        {
            var Update = new List<ICriteriaComparisonModel>();
            double newratio;
            foreach (var item in comps)
            {
                if ((bool)item.Flipped)
                {
                    if (item.CriteriaRatio == 0)
                    {
                        newratio = 1;
                    }
                    else if (item.CriteriaRatio < 0)
                    {
                        newratio = -(item.CriteriaRatio - 1);
                    }
                    else
                    {
                        newratio = item.CriteriaRatio + 1;
                        newratio = 1 / newratio;
                    }
                    ICriteriaComparisonModel _cc = new CriteriaComparisonModel { CriteriaID1 = item.CriterionID2, CriteriaID2 = item.CriterionID1, CriteriaRatio = newratio };
                    Update.Add(_cc);
                }
                else
                {
                    if (item.CriteriaRatio == 0)
                    {
                        newratio = 1;
                    }
                    else if (item.CriteriaRatio < 0)
                    {
                        newratio = -(item.CriteriaRatio - 1);
                        newratio = 1 / newratio;
                    }
                    else
                    {
                        newratio = item.CriteriaRatio + 1;
                    }
                    ICriteriaComparisonModel _cc = new CriteriaComparisonModel { CriteriaID2 = item.CriterionID2, CriteriaID1 = item.CriterionID1, CriteriaRatio = newratio };
                    Update.Add(_cc);
                }
            }
            _ = await _criteriaComparisonService.UpdateAsync(Update);

            return RedirectToAction("ListCriteriaComparisons", "CriteriaComparison", new { page = Session["page"] });
        }

        public async Task<ActionResult> ListUnfilledCriteriaComparisons()
        {
            ViewBag.Title = "Unfilled criteria comparisons";

            var critcomps = await _criteriaComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            var _critcomps = new List<CriteriaComparisonMvcModel>();
            IDictionary<Guid, string> CritNames = (IDictionary<Guid, string>)Session["CriteriaNames"];

            foreach (ICriteriaComparisonModel critcomp in critcomps)
            {
                Guid critID1 = critcomp.CriteriaID1;
                Guid critID2 = critcomp.CriteriaID2;

                double newcritratio = 0;

                CriteriaComparisonMvcModel cc = new CriteriaComparisonMvcModel
                {
                    CriterionName1 = CritNames[critID1],
                    CriterionID1 = critID1,
                    CriterionID2 = critID2,
                    CriterionName2 = CritNames[critID2],
                    Flipped = false,
                    CriteriaRatio = newcritratio
                };

                _critcomps.Add(cc);
            }
            if (!_critcomps.Any())
            {
                return RedirectToAction("ListCriteria", "Criterion", new { page = Session["Page"] });
            }

            return View(_critcomps);
        }

        [HttpPost]
        public async Task<ActionResult> ListUnfilledCriteriaComparisons(List<CriteriaComparisonMvcModel> comps)
        {
            var Update = new List<ICriteriaComparisonModel>();
            double newratio;
            foreach (var item in comps)
            {

                if (item.CriteriaRatio == 0)
                {
                    newratio = 1;
                }
                else if (item.CriteriaRatio < 0)
                {
                    newratio = -(item.CriteriaRatio - 1);
                    newratio = 1 / newratio;
                }
                else
                {
                    newratio = item.CriteriaRatio + 1;
                }
                ICriteriaComparisonModel _cc = new CriteriaComparisonModel { CriteriaID1 = item.CriterionID1, CriteriaID2 = item.CriterionID2,CriteriaRatio = newratio };
                Update.Add(_cc);
            }
            _ = await _criteriaComparisonService.UpdateAsync(Update);

            var critcomps = await _criteriaComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            if (critcomps.Any())
            {
                return RedirectToAction("ListUnfilledCriteriaComparisons", "CriteriaComparison", null);
            }
            else
            {
                Session["AnyUnfilledCrit"] = false;
            }


            return RedirectToAction("ListCriteria", "Criterion", new { page = Session["Page"] });
        }

        public async Task<ActionResult> Unfilled()
        {
            var criteria = new List<ICriterionModel>();
            int page = 1;
            IDictionary<Guid, string> CritNames = new Dictionary<Guid, string>(20);
            do
            {
                criteria = await _criterionService.GetAsync((Guid)Session["ChoiceID"], page);
                page++;
                foreach (var crit in criteria)
                {
                    CritNames.Add(crit.CriteriaID, crit.CriteriaName);
                }
            } while (criteria.Count != 0);
            Session["CriteriaNames"] = CritNames;
            ViewBag.Title = "Edit a criterion";
            Session["Page"] = 1;
            return RedirectToAction("ListUnfilledCriteriaComparisons", "CriteriaComparison", null);
        }
    }
}