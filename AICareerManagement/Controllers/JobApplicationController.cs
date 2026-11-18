using System;
using System.Linq;
using System.Web.Mvc;
using AICareerManagement.DAL;
using AICareerManagement.Models;

namespace AICareerManagement.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly JobApplicationDAL dal = new JobApplicationDAL();

        private int? LoggedInUserId
        {
            get
            {
                if (Session["UserId"] == null)
                    return null;

                return Convert.ToInt32(Session["UserId"]);
            }
        }

        // GET: JobApplication

        public ActionResult Index(string searchTerm, string applicationStatus)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            // Allow only supported application statuses
            string[] validStatuses =
            {
        "Applied", "Interview", "Selected", "Rejected", "On Hold"
    };

            if (!string.IsNullOrWhiteSpace(applicationStatus) &&
                !validStatuses.Contains(applicationStatus))
            {
                applicationStatus = null;
            }

            var applications = dal.SearchApplications(
                userId,
                searchTerm,
                applicationStatus
            );

            ViewBag.SearchTerm = searchTerm;
            ViewBag.ApplicationStatus = applicationStatus;

            return View(applications);
        }

        // GET: JobApplication/Create
        public ActionResult Create()
        {
            if (!LoggedInUserId.HasValue)
                return RedirectToAction("Login", "Account");

            return View(new JobApplicationModel());
        }

        // POST: JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobApplicationModel model)
        {
            if (!LoggedInUserId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(model.CompanyName))
                ModelState.AddModelError("CompanyName", "Company name is required.");

            if (string.IsNullOrWhiteSpace(model.JobTitle))
                ModelState.AddModelError("JobTitle", "Job title is required.");

            string[] statuses =
                { "Applied", "Interview", "Selected", "Rejected", "On Hold" };

            if (!statuses.Contains(model.ApplicationStatus))
                ModelState.AddModelError("ApplicationStatus", "Select a valid status.");

            if (!ModelState.IsValid)
                return View(model);

            model.UserId = LoggedInUserId.Value;

            dal.AddApplication(model);

            TempData["Success"] = "Job application added successfully.";

            return RedirectToAction("Index");
        }

        // GET: JobApplication/Edit/5
        public ActionResult Edit(int id)
        {
            if (!LoggedInUserId.HasValue)
                return RedirectToAction("Login", "Account");

            var application =
                dal.GetApplicationById(id, LoggedInUserId.Value);

            if (application == null)
                return HttpNotFound();

            return View(application);
        }

        // POST: JobApplication/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobApplicationModel model)
        {
            if (!LoggedInUserId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(model.CompanyName))
                ModelState.AddModelError("CompanyName", "Company name is required.");

            if (string.IsNullOrWhiteSpace(model.JobTitle))
                ModelState.AddModelError("JobTitle", "Job title is required.");

            string[] statuses =
                { "Applied", "Interview", "Selected", "Rejected", "On Hold" };

            if (!statuses.Contains(model.ApplicationStatus))
                ModelState.AddModelError("ApplicationStatus", "Select a valid status.");

            if (!ModelState.IsValid)
                return View(model);

            model.UserId = LoggedInUserId.Value;

            bool updated = dal.UpdateApplication(model);

            if (!updated)
                return HttpNotFound();

            TempData["Success"] = "Job application updated successfully.";

            return RedirectToAction("Index");
        }

        // POST: JobApplication/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!LoggedInUserId.HasValue)
                return RedirectToAction("Login", "Account");

            bool deleted =
                dal.DeleteApplication(id, LoggedInUserId.Value);

            TempData["Success"] = deleted
                ? "Job application deleted successfully."
                : "Application was not found.";

            return RedirectToAction("Index");
        }
    }
}