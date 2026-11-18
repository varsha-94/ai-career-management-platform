using System;
using System.Web.Mvc;
using AICareerManagement.DAL;
using AICareerManagement.Services;
using AICareerManagement.ViewModels;

namespace AICareerManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDAL userDAL = new UserDAL();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool result = userDAL.RegisterUser(
                    model.FullName.Trim(),
                    model.Email.Trim().ToLowerInvariant(),
                    model.Password
                );

                if (result)
                {
                    TempData["Success"] =
                        "Registration successful. Please login.";

                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(
                    "Email", "Email already registered.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "", "Unable to complete registration. Please try again.");
            }

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = userDAL.GetUserByEmail(
                    model.Email.Trim().ToLowerInvariant());

                if (user != null &&
                    PasswordService.VerifyPassword(
                        model.Password, user.PasswordHash))
                {
                    Session["UserId"] = user.UserId;
                    Session["FullName"] = user.FullName;
                    Session["Email"] = user.Email;

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(
                    "", "Invalid email or password.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "", "Login could not be completed. Please try again.");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");
        }
    }
}