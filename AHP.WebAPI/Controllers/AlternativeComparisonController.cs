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
    public class AlternativeComparisonController : Controller
    {
        IAlternativeComparisonService _alternativeComparisonService;
        IAlternativeService _alternativeService;
        ICriterionService _criterionService;
        IMapper _mapper;
        public AlternativeComparisonController(
            IMapper mapper,
            IAlternativeComparisonService alternativeComparisonService,
            IAlternativeService alternativeService,
            ICriterionService criterionService
          )
        {
            _mapper = mapper;
            _alternativeComparisonService = alternativeComparisonService;
            _alternativeService = alternativeService;
            _criterionService = criterionService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListAlternativeComparisons", "AlternativeComparison", new { page = Session["page"] });
        }

        public async Task<ActionResult> ListAlternativeComparisons(int page)
        {
            ViewBag.Title = "Alternative comparisons";
            if (page < 1) { page = 1; }
            Session["Page"] = page;

            var caltcomps = await _alternativeComparisonService.GetByAlternativeIdAsync((Guid)Session["AlternativeID"], page);
            var _caltcomps = new List<AlternativeComparisonMvcModel>();
            Guid altID = (Guid)Session["AlternativeID"];
            IDictionary<Guid, string> CritNames = (IDictionary<Guid, string>)Session["CriteriaNames"];
            IDictionary<Guid, string> AltNames = (IDictionary<Guid, string>)Session["AlternativesNames"];

            foreach (IAlternativeComparisonModel altcomp in caltcomps)
            {
                Guid altID1 = altcomp.AlternativeID1;
                Guid altID2 = altcomp.AlternativeID2;
                Guid critID = altcomp.CriteriaID;

                if (altID1 == altID)
                {
                    //OVO S NUOOM je samo za test
                    double newaltratio;
                    if (altcomp.AlternativeRatio != 0)
                    {
                        newaltratio = altcomp.AlternativeRatio;
                        //Pretvara u oblik koji je prihvatljiv sliderima
                        if (newaltratio < 1)
                        {
                            newaltratio = -(Math.Round((1 / altcomp.AlternativeRatio)) - 1);
                        }
                        else
                        {
                            newaltratio -= 1;
                        }
                    }
                    else
                    {
                        newaltratio = 0;
                    }
                    AlternativeComparisonMvcModel ac = new AlternativeComparisonMvcModel
                    {
                        CriteriaID = altcomp.CriteriaID,
                        CriteriaName = CritNames[critID],
                        AlternativeID1 = altID1,
                        AlternativeName1 = AltNames[altID1],
                        AlternativeID2 = altID2,
                        AlternativeName2 = AltNames[altID2],
                        Flipped = false,
                        AlternativeRatio = newaltratio
                    };

                    _caltcomps.Add(ac);
                }
                else
                {
                    double newaltratio;
                    if (altcomp.AlternativeRatio != 0)
                    {
                        newaltratio = 1 / altcomp.AlternativeRatio;
                        //OVO SA NULOM POSLJE OBRISAT
                        //Pretvara u oblik koji je prihvatljiv sliderima
                        if (newaltratio < 1)
                        {
                            newaltratio = -(Math.Round(1 / newaltratio) - 1);
                        }
                        else
                        {
                            newaltratio -= 1;
                        }
                    }
                    else
                    {
                        newaltratio = 0;
                    }

                    AlternativeComparisonMvcModel ac = new AlternativeComparisonMvcModel
                    {
                        CriteriaID = altcomp.CriteriaID,
                        CriteriaName = CritNames[critID],
                        AlternativeID1 = altID2,
                        AlternativeName1 = AltNames[altID2],
                        AlternativeID2 = altID1,
                        AlternativeName2 = AltNames[altID1],
                        Flipped = true,
                        AlternativeRatio = newaltratio
                    };

                    _caltcomps.Add(ac);
                }
            }
            if (!_caltcomps.Any() && page > 1)
            {
                Session["Page"] = page - 1;
                return RedirectToAction("ListAlternativeComparisons", "AlternativeComparison", new { page = Session["Page"] });
            }

            return View(_caltcomps);
        }

        [HttpPost]
        public async Task<ActionResult> ListAlternativeComparisons(List<AlternativeComparisonMvcModel> comps)
        {
            var Update = new List<IAlternativeComparisonModel>();
            double newratio;
            foreach(var item in comps)
            {
                if ((bool)item.Flipped) {
                    if (item.AlternativeRatio == 0)
                    {
                        newratio = 1;
                    }
                    else if (item.AlternativeRatio < 0)
                    {
                        newratio = -(item.AlternativeRatio - 1);

                    }
                    else
                    {
                        newratio = item.AlternativeRatio + 1;
                        newratio = 1 / newratio;
                    }
                    IAlternativeComparisonModel _ac = new AlternativeComparisonModel { AlternativeID1 = item.AlternativeID2, AlternativeID2 = item.AlternativeID1, CriteriaID = item.CriteriaID, AlternativeRatio = newratio };
                    Update.Add(_ac);
                }
                else
                {
                    if (item.AlternativeRatio == 0)
                    {
                        newratio = 1;
                    }
                    else if (item.AlternativeRatio < 0)
                    {
                        newratio = -(item.AlternativeRatio - 1);
                        newratio = 1 / newratio;
                    }
                    else
                    {
                        newratio = item.AlternativeRatio + 1;
                    }
                    IAlternativeComparisonModel _ac = new AlternativeComparisonModel { AlternativeID1 = item.AlternativeID1, AlternativeID2 = item.AlternativeID2, CriteriaID = item.CriteriaID, AlternativeRatio = newratio };
                    Update.Add(_ac);
                } 
            }
            _ = await _alternativeComparisonService.UpdateAsync(Update);

            return RedirectToAction("ListAlternativeComparisons", "AlternativeComparison", new { page = Session["page"] });
        }

        public async Task<ActionResult> ListUnfilledAlternativeComparisons()
        {
            ViewBag.Title = "Unfilled alternative comparisons";

            var caltcomps = await _alternativeComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            var _caltcomps = new List<AlternativeComparisonMvcModel>();
            IDictionary<Guid, string> CritNames = (IDictionary<Guid, string>)Session["CriteriaNames"];
            IDictionary<Guid, string> AltNames = (IDictionary<Guid, string>)Session["AlternativesNames"];

            foreach (IAlternativeComparisonModel altcomp in caltcomps)
            {
                Guid altID1 = altcomp.AlternativeID1;
                Guid altID2 = altcomp.AlternativeID2;

                double newaltratio = 0;

                AlternativeComparisonMvcModel ac = new AlternativeComparisonMvcModel
                {
                    CriteriaID = altcomp.CriteriaID,
                    CriteriaName = CritNames[altcomp.CriteriaID],
                    AlternativeID1 = altID1,
                    AlternativeName1 = AltNames[altID1],
                    AlternativeID2 = altID2,
                    AlternativeName2 = AltNames[altID2],
                    Flipped = false,
                    AlternativeRatio = newaltratio
                };

                _caltcomps.Add(ac);
            }
            if (!_caltcomps.Any())
            {
                return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
            }

            return View(_caltcomps);
        }

        [HttpPost]
        public async Task<ActionResult> ListUnfilledAlternativeComparisons(List<AlternativeComparisonMvcModel> comps)
        {
            var Update = new List<IAlternativeComparisonModel>();
            double newratio;
            foreach (var item in comps)
            {

                if (item.AlternativeRatio == 0)
                {
                    newratio = 1;
                }
                else if (item.AlternativeRatio < 0)
                {
                    newratio = -(item.AlternativeRatio - 1);
                    newratio = 1 / newratio;
                }
                else
                {
                    newratio = item.AlternativeRatio + 1;
                }
                IAlternativeComparisonModel _ac = new AlternativeComparisonModel { AlternativeID1 = item.AlternativeID1, AlternativeID2 = item.AlternativeID2, CriteriaID = item.CriteriaID, AlternativeRatio = newratio };
                Update.Add(_ac);  
            }
            _ = await _alternativeComparisonService.UpdateAsync(Update);

            var caltcomps = await _alternativeComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            if (caltcomps.Any())
            {
                return RedirectToAction("ListUnfilledAlternativeComparisons", "AlternativeComparison", new { page = Session["Page"] });
            }
            else
            { 
                Session["AnyUnfilledAlt"] = false;
            }
            return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
        }

        public async Task<ActionResult> Unfilled()
        {
            var alternatives = new List<IAlternativeModel>();
            int page = 1;
            IDictionary<Guid, string> Names = new Dictionary<Guid, string>(20);
            do
            {
                alternatives = await _alternativeService.GetAsync((Guid)Session["ChoiceID"], page);
                page += 1;
                foreach (var alt in alternatives)
                {
                    Names.Add(alt.AlternativeID, alt.AlternativeName);
                }
            } while (alternatives.Count != 0);
            Session["AlternativesNames"] = Names;
            var criteria = new List<ICriterionModel>();
            page = 1;
            IDictionary<Guid, string> CritNames = new Dictionary<Guid, string>(20);
            do
            {
                criteria = await _criterionService.GetAsync((Guid)Session["ChoiceID"], page);
                page++;
                foreach (var crit in criteria)
                {
                    CritNames.Add(crit.CriteriaID, crit.CriteriaName);
                }
            } while (alternatives.Count != 0);
            Session["CriteriaNames"] = CritNames;
            ViewBag.Title = "Edit an Alternative";
            Session["Page"] = 1;
            return RedirectToAction("ListUnfilledAlternativeComparisons", "AlternativeComparison", null);
        }
    }
}
