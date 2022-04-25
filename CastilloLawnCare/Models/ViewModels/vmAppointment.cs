using Microsoft.AspNetCore.Mvc.Rendering;

namespace CastilloLawnCare.Models
{
    namespace ViewModels
    {
        public class vmAppointment : User, Interfaces.IService, Interfaces.IAppointmentType
        {
            public int AppointmentID { get; set; }
            public DateTime AppointmentDate { get; set; }
            public int AppointmentTypeID { get; set; }
            public int ServiceID { get; set; }
            public string ServiceType { get; set; }
            public string ServiceDescription { get; set; }
            public string AppointmentType { get; set; }
            public List<SelectListItem> ServiceList { get; set; }

            public vmAppointment()
            {
                ServiceList = new List<SelectListItem>();
            }

            public vmAppointment(int appointmentID, DateTime appointmentDate, int appointmentTypeID, int serviceID, string serviceType, 
                                 string serviceDescription, string appointmentType, List<SelectListItem> serviceList)
            {
                AppointmentID = appointmentID;
                AppointmentDate = appointmentDate;
                AppointmentTypeID = appointmentTypeID;
                ServiceID = serviceID;
                ServiceType = serviceType;
                ServiceDescription = serviceDescription;
                AppointmentType = appointmentType;
                ServiceList = serviceList;
            }
        }
    }
   
}
