using AICareerManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AICareerManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalApplications { get; set; }

        public int AppliedCount { get; set; }

        public int InterviewCount { get; set; }

        public int SelectedCount { get; set; }

        public int RejectedCount { get; set; }

        public int OnHoldCount { get; set; }

        public List<JobApplicationModel> RecentApplications
        {
            get;
            set;
        }
    }
}