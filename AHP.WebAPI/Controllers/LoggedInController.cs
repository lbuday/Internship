using AHP.Model;
using AHP.Model.Common;
using AHP.Service;
using AHP.Service.Common;
using AHP.WebAPI.Models;
using AutoMapper;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AHP.WebAPI.Controllers
{
    public class LoggedInController : Controller
    {
        IChoiceService _choiceService;
        IAlternativeComparisonService _alternativeComparisonService;
        ICriteriaComparisonService _criteriaComparisonService;
        IMapper _mapper;

        public LoggedInController(IMapper mapper, ICriteriaComparisonService criteriaComparisonService, IAlternativeComparisonService alternativeComparisonService,  IChoiceService choiceService)
        {
            _mapper = mapper;
            _choiceService = choiceService;
            _alternativeComparisonService = alternativeComparisonService;
            _criteriaComparisonService = criteriaComparisonService;
        }
        public ActionResult Index()
        {
            return RedirectToAction("ListChoices", "LoggedIn");
        }

        public async Task<ActionResult> ListChoices(int page = 1)
        {
            if (page < 1) { page = 1; }
            Session["Page"] = page;

            Guid userID = (Guid)this.Session["UserID"];
            var choices = await _choiceService.GetAsync(userID, page);
            var _choices = new List<ChoiceMvcModel>();
            foreach (IChoiceModel choice in choices)
            {
                _choices.Add(new ChoiceMvcModel { ChoiceID = choice.ChoiceID, Name = choice.ChoiceName, DateUpdated = (DateTime)choice.DateUpdated, UserID = choice.UserID });
            }
            if (!_choices.Any() && page > 1)
            {
                Session["Page"] = page - 1;
                return RedirectToAction("ListChoices", "LoggedIn", new { page = Session["page"] });
            }
            return View(_choices);
        }

        public ActionResult CreateChoice()
        {
            ViewBag.Title = "Create a Choice";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateChoice(ChoiceMvcModel model)
        {
            if (ModelState.IsValid)
            {
                IChoiceModel _choice = new ChoiceModel { ChoiceName = model.Name, UserID = (Guid)Session["UserID"] };
                var status = await _choiceService.CreateAsync(_choice);
                Guid _userid = status.UserID;
                return RedirectToAction("ListChoices", "LoggedIn", new { page = Session["page"] });
            }
            return View();
        }

        public async Task<ActionResult> DeleteChoice(Guid choiceID)
        {
            var choice = await _choiceService.GetByIdAsync(choiceID);
            bool b = await _choiceService.DeleteAsync(choice);
            return RedirectToAction("ListChoices", "LoggedIn", new { page = Session["page"] });
        }

        public async Task<ActionResult> EditChoice(Guid choiceID)
        {

            Session["ChoiceID"] = choiceID;
            var caltcomps = await _alternativeComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            if (caltcomps.Any()) { Session["AnyUnfilledAlt"] = true; } else
            {
                Session["AnyUnfilledAlt"] = false;
            }
            var critcomps = await _criteriaComparisonService.GetUnfilledAsync((Guid)Session["ChoiceID"]);
            if (critcomps.Any()) { Session["AnyUnfilledCrit"] = true; } else
            {
                Session["AnyUnfilledCrit"] = false;
            }
            Session["Page"] = 1;
            ViewBag.Title = "List Alternatives";
            var choice = await _choiceService.GetByIdAsync(choiceID);
            Session["ChoiceName"] = choice.ChoiceName;
            return RedirectToAction("ListAlternatives", "Alternative", new { page = Session["page"] });
        }

        public ActionResult BackToChoices()
        {
            Session["AnyUnfilledAlt"] = false;
            Session["AnyUnfilledCrit"] = false;
            Session["AlternativesNames"] = null;
            Session["CriteriaNames"] = null;
            return RedirectToAction("ListChoices", "LoggedIn");
        }
    }


}