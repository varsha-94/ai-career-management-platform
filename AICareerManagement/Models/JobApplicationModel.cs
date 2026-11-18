using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AICareerManagement.Models
{
    public class JobApplicationModel
    {
        public int ApplicationId { get; set; }

        public int UserId { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string ApplicationStatus { get; set; }

        public DateTime AppliedDate { get; set; }
    }
}