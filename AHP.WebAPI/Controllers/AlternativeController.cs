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
    public class AlternativeController : Controller
    {
        IAlternativeService _alternativeService;
        ICriterionService _criterionService;
        IAlternativeComparisonService _alternativeComparisonService;
        IMapper _mapper;
        public AlternativeController(
            IMapper mapper,
            IAlternativeService alternativeService,
            IAlternativeComparisonService alternativeComparisonService,
            ICriterionService criterionService
          )
        {
            _mapper = mapper;
            _alternativeService = alternativeService;
            _criterionService = criterionService;
            _alternativeComparisonService = alternativeComparisonService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
        }

        public async Task<ActionResult> ListAlternatives(int page)
        {
            ViewBag.Title = "Alterntatives";
            if (page < 1)
            {
                page = 1;
            }
            Session["Page"] = page;

            var alternatives = await _alternativeService.GetAsync((Guid)Session["ChoiceID"], page);
            var _alternatives = new List<AlternativeMvcModel>();
            foreach (IAlternativeModel alternative in alternatives)
            {
                double? newscore;
                if (alternative.AlternativeScore != null) {
                    double num = alternative.AlternativeScore ?? 0;
                    newscore = Math.Round(num, 4); }
                else { newscore = null; }
                _alternatives.Add(new AlternativeMvcModel { AlternativeID = alternative.AlternativeID, ChoiceID = alternative.ChoiceID, AlternativeName = alternative.AlternativeName, DateUpdated = (DateTime)alternative.DateUpdated , Score = newscore });
            }
            if (!_alternatives.Any() && page > 1)
            {
                Session["Page"] = page - 1;
                return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
            }

            return View(_alternatives);
        }

        public ActionResult CreateAlternative()
        {
            ViewBag.Title = "Create an Alternative";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAlternative(AlternativeMvcModel model)
        {
            if (ModelState.IsValid)
            {
                IAlternativeModel _alternative = new AlternativeModel { ChoiceID = (Guid)Session["ChoiceID"], AlternativeName = model.AlternativeName };
                var status = await _alternativeService.AddAsync(_alternative);
                var caltcomps = await _alternativeComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
                if (caltcomps.Any()) { Session["AnyUnfilledAlt"] = true; }
                else
                {
                    Session["AnyUnfilledAlt"] = false;
                }
                return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
            }
            return View();
        }

        public async Task<ActionResult> DeleteAlternative(Guid alternativeID)
        {
            var alternative = await _alternativeService.GetByIdAsync(alternativeID);
            bool b = await _alternativeService.DeleteAsync(alternative);
            var caltcomps = await _alternativeComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            if (caltcomps.Any()) { Session["AnyUnfilledAlt"] = true; }
            else
            {
                Session["AnyUnfilledAlt"] = false;
            }
            return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["Page"] });
        }

        public async Task<ActionResult> EditAlternative(Guid alternativeid)
        {
            var alternatives = new List<IAlternativeModel>();
            int page = 1;
            IDictionary<Guid, string> Names = new Dictionary<Guid, string>(10);
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
            IDictionary<Guid, string> CritNames = new Dictionary<Guid, string>(10);
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
            ViewBag.Title = "Edit an Alternative";
            var alternative = await _alternativeService.GetByIdAsync(alternativeid);
            Session["AlternativeName"] = alternative.AlternativeName;
            Session["AlternativeID"] = alternativeid;
            Session["Page"] = 1;
            return RedirectToAction("ListAlternativeComparisons", "AlternativeComparison", new { page = Session["page"] });
        }
    }
}