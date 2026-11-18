using AICareerManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AICareerManagement.ViewModels;
namespace AICareerManagement.DAL
{

    
    namespace AICareerManagement.DAL
    {
        public class DashboardDAL
        {
            private readonly string connectionString =
                ConfigurationManager.ConnectionStrings["CareerDB"]
                .ConnectionString;

            public DashboardViewModel GetDashboardStatistics(int userId)
            {
                DashboardViewModel dashboard = new DashboardViewModel();

                using (SqlConnection con =
                    new SqlConnection(connectionString))
                using (SqlCommand cmd =
                    new SqlCommand("SP_GetDashboardStatistics", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            dashboard.TotalApplications =
                                Convert.ToInt32(dr["TotalApplications"]);

                            dashboard.AppliedCount =
                                Convert.ToInt32(dr["AppliedCount"]);

                            dashboard.InterviewCount =
                                Convert.ToInt32(dr["InterviewCount"]);

                            dashboard.SelectedCount =
                                Convert.ToInt32(dr["SelectedCount"]);

                            dashboard.RejectedCount =
                                Convert.ToInt32(dr["RejectedCount"]);

                            dashboard.OnHoldCount =
                                Convert.ToInt32(dr["OnHoldCount"]);
                        }
                    }
                }

                return dashboard;
            }
        }
    }

}