using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AICareerManagement.Models;

namespace AICareerManagement.DAL
{
    public class JobApplicationDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["CareerDB"]
            .ConnectionString;

        public List<JobApplicationModel> GetApplications(int userId)
        {
            List<JobApplicationModel> applications =
                new List<JobApplicationModel>();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand("SP_GetJobApplications", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        applications.Add(new JobApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(
                                dr["ApplicationId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            CompanyName = dr["CompanyName"].ToString(),
                            JobTitle = dr["JobTitle"].ToString(),
                            JobDescription =
                                dr["JobDescription"].ToString(),
                            ApplicationStatus =
                                dr["ApplicationStatus"].ToString(),
                            AppliedDate = Convert.ToDateTime(
                                dr["AppliedDate"])
                        });
                    }
                }
            }

            return applications;
        }


        public int AddApplication(JobApplicationModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddJobApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = model.UserId;
                cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 150).Value = model.CompanyName;
                cmd.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 150).Value = model.JobTitle;
                cmd.Parameters.Add("@JobDescription", SqlDbType.NVarChar, -1).Value =
                    (object)model.JobDescription ?? DBNull.Value;
                cmd.Parameters.Add("@ApplicationStatus", SqlDbType.NVarChar, 50).Value =
                    model.ApplicationStatus;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public JobApplicationModel GetApplicationById(int applicationId, int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetJobApplicationById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ApplicationId", SqlDbType.Int).Value = applicationId;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new JobApplicationModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            CompanyName = dr["CompanyName"].ToString(),
                            JobTitle = dr["JobTitle"].ToString(),
                            JobDescription = dr["JobDescription"].ToString(),
                            ApplicationStatus = dr["ApplicationStatus"].ToString(),
                            AppliedDate = Convert.ToDateTime(dr["AppliedDate"])
                        };
                    }
                }
            }

            return null;
        }

        public bool UpdateApplication(JobApplicationModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateJobApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ApplicationId", SqlDbType.Int).Value = model.ApplicationId;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = model.UserId;
                cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 150).Value = model.CompanyName;
                cmd.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 150).Value = model.JobTitle;
                cmd.Parameters.Add("@JobDescription", SqlDbType.NVarChar, -1).Value =
                    (object)model.JobDescription ?? DBNull.Value;
                cmd.Parameters.Add("@ApplicationStatus", SqlDbType.NVarChar, 50).Value =
                    model.ApplicationStatus;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool DeleteApplication(int applicationId, int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteJobApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ApplicationId", SqlDbType.Int).Value = applicationId;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }


    }
}