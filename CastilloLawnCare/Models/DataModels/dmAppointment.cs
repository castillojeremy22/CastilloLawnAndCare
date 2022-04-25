namespace CastilloLawnCare.Models.DataModels
{
    public class dmAppointment
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int AppointmentTypeID { get; set; }
        public int ServiceID { get; set; }
        public string UserID { get; set; }

        public dmAppointment()
        {
        }

        public dmAppointment(int appointmentID, DateTime appointmentDate, int appointmentTypeID, int serviceID, string userID)
        {
            AppointmentID = appointmentID;
            AppointmentDate = appointmentDate;
            AppointmentTypeID = appointmentTypeID;
            ServiceID = serviceID;
            UserID = userID;
        }
    }
}
