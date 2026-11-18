using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AICareerManagement.Models;
using AICareerManagement.Services;

namespace AICareerManagement.DAL
{
    public class UserDAL
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["CareerDB"]
            .ConnectionString;

        public bool RegisterUser(
            string fullName,
            string email,
            string password)
        {
            string passwordHash =
                PasswordService.HashPassword(password);

            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand("SP_RegisterUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@FullName", SqlDbType.NVarChar, 100
                ).Value = fullName;

                cmd.Parameters.Add(
                    "@Email", SqlDbType.NVarChar, 150
                ).Value = email;

                cmd.Parameters.Add(
                    "@PasswordHash", SqlDbType.NVarChar, 500
                ).Value = passwordHash;

                con.Open();

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                return result == 1;
            }
        }

        public UserModel GetUserByEmail(string email)
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand("SP_LoginUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Email", SqlDbType.NVarChar, 150
                ).Value = email;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserModel
                        {
                            UserId = Convert.ToInt32(
                                reader["UserId"]),

                            FullName = reader["FullName"].ToString(),

                            Email = reader["Email"].ToString(),

                            PasswordHash =
                                reader["PasswordHash"].ToString()
                        };
                    }
                }
            }

            return null;
        }
    }
}