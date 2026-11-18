
using AICareerManagement.DAL;
using AICareerManagement.DAL.AICareerManagement.DAL;
using AICareerManagement.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AICareerManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardDAL dashboardDAL =
            new DashboardDAL();

        private readonly JobApplicationDAL jobDAL =
            new JobApplicationDAL();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            DashboardViewModel model =
                dashboardDAL.GetDashboardStatistics(userId);

            model.RecentApplications =
                jobDAL.GetApplications(userId);

            if (model.RecentApplications != null)
            {
                model.RecentApplications =
                    model.RecentApplications
                    .OrderByDescending(x => x.AppliedDate)
                    .Take(5)
                    .ToList();
            }

            return View(model);
        }
    }
}
