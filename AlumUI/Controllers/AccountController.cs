using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.Owin.Security;
using Microsoft.Owin;

using System.Net.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using DomainLib;
using AlumUI.Models;

namespace AlumUI.Controllers
{
    public class AccountController : Controller
    {
   //
        // GET: /Account/
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        List<string> Loginbox = new List<string>();

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login details)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.Name,
details.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password.");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, ident);
                    return Redirect("/");
                }
            }
            return View(details);
        }
        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Main", "Home");
        }
        public ActionResult Register()
        {

            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Captcha answer cannot be empty.");
                return View(model);
            }

            RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();

            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("", "Incorrect captcha answer.");
            }

            if (ModelState.IsValid)
            {
                

                AppUser user = new AppUser { UserName = model.Username, Email = model.Email, PhoneNumber=model.Phoneno };
                IdentityResult result = await UserManager.CreateAsync(user,
                    model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Main","Home");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public PartialViewResult LoginBox()
        {
            if(HttpContext.User.Identity.IsAuthenticated==true)
            { 
            Loginbox.Add(HttpContext.User.Identity.Name);
            Loginbox.Add(HttpContext.User.Identity.IsAuthenticated.ToString());
            }
                
            return PartialView (Loginbox);
        }
    }
	}
