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
    public class SignUpController : Controller
    {
        IUserService _userService;
        IMapper _mapper;
        public SignUpController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.Title = "User Sign Up";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> SignUp(UserMvcModel model)
        {
            if (ModelState.IsValid)
            {
                IUserModel _userToCreate = new UserModel { Username = model.Username, Password = model.Password };
                var status = await _userService.CheckAsync(_userToCreate);
                Guid _userid = status.UserID;
                this.Session["UserID"] = _userid;
                return RedirectToAction("ListChoices", "LoggedIn", new { userid = _userid });
            }

            return View();
        }

        public ActionResult SignIn()
        {
            ViewBag.Title = "User Sign In";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> SignIn(UserMvcModel model)
        {
            IUserModel _userToLogIn = new UserModel { Username = model.Username, Password = model.Password };
            var status = await _userService.GetAsync(_userToLogIn);
            Guid _userid = status.UserID;
            Session["UserID"] = _userid;
            return RedirectToAction("ListChoices", "LoggedIn");
        }
    }
}