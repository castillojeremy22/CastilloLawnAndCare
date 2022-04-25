namespace CastilloLawnCare.Models.DataModels
{
    public class dmService : Interfaces.IService
    {
        public int ServiceID { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDescription { get; set; }

        public dmService()
        {
        }

        public dmService(int serviceID, string serviceType, string serviceDescription)
        {
            ServiceID = serviceID;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
        }
    }
}
