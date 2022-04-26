using CastilloLawnCare.Models.DataModels;
//using Microsoft.Data.SqlClient;
using MySqlConnector;

namespace CastilloLawnCare.Data
{
    public class CastilloLawnCareDBData
    {
        CastilloLawnCareDA da = new CastilloLawnCareDA();
        public void AddReview(dmCustomerReview dmCustomerReview)
        {
            string query = "Insert Into Reviews (Description, UserID, ReviewPostedDate, Rating)" +
                           "Values (@Description, @UserID, @ReviewDate, @Rating);";

            using (MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                cmd.Parameters.AddWithValue("@Description", dmCustomerReview.Description);
                cmd.Parameters.AddWithValue("@UserID", dmCustomerReview.UserID);
                cmd.Parameters.AddWithValue("@ReviewDate", dmCustomerReview.ReviewDate);
                cmd.Parameters.AddWithValue("@Rating", dmCustomerReview.Rating);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
        }

        public List<dmCustomerReview> GetReviews()
        {
            List<dmCustomerReview> reviews = new List<dmCustomerReview>();
            string query = "Select * From Reviews Order By ReviewPostedDate Desc Limit 5;";

            using (MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                try
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                reviews.Add(new dmCustomerReview(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetDateTime(3), dr.GetInt32(4)));
                            }
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            return reviews;
        }

        public void AddAppointment(dmAppointment dmAppointment)
        {
            string query = "Insert Into ClientAppointments (AppointmentDate, AppointmentTypeID, ServiceID, UserID)" +
                           "Values (@AppointmentDate, @AppointmentTypeID, @ServiceID, @UserID);";

            using (MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                cmd.Parameters.AddWithValue("@AppointmentDate", dmAppointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@AppointmentTypeID", dmAppointment.AppointmentTypeID);
                cmd.Parameters.AddWithValue("@ServiceID", dmAppointment.ServiceID);
                cmd.Parameters.AddWithValue("@UserID", dmAppointment.UserID);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }

            }
        }

        public List<dmAppointment> GetAppointmentsByUserID(string userID)
        {
            List<dmAppointment> dmAppointmentList = new List<dmAppointment>();
            string query = "Select * From ClientAppointments Where UserID = @UserID;";
            
            using(MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dmAppointmentList.Add(new dmAppointment(dr.GetInt32(0),
                                                                        dr.GetDateTime(1),
                                                                        dr.GetInt32(2),
                                                                        dr.GetInt32(3),
                                                                        dr.GetString(4)));
                            }
                        }
                    }
                }
                catch(MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            return dmAppointmentList;
        }

        public string GetAppointmentTypeByID(int id)
        {
            string appointmentType = null;
            string query = "Select AppointmentType from AppointmentTypes where AppointmentTypeID = @AppointmentTypeID;";
            using( MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@AppointmentTypeID", id);
                    using(MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                appointmentType = dr.GetString(0);
                            }
                        }
                    }
                }
                catch(MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            return appointmentType;
        }

        public List<dmService> GetServices()
        {
            List<dmService> dmServices = new List<dmService>();
            string query = "Select * From ServiceTypes;";
            using( MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                try
                {
                    using(MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dmServices.Add(new dmService(dr.GetInt32(0), dr.GetString(1), dr.IsDBNull(2) ? null : dr.GetString(2)));
                            }
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            return dmServices;
        }
        
        public string GetServiceTypeByID(int id)
        {
            string serviceType = null;
            string query = "Select ServiceType from ServiceTypes where ServiceID = @ServiceID;";
            using (MySqlCommand cmd = new MySqlCommand(query, da.CreateConnection()))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@ServiceID", id);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                serviceType = dr.GetString(0);
                            }
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            return serviceType;

        }
    }
}
