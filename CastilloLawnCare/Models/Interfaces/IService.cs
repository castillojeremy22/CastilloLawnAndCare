namespace CastilloLawnCare.Models.Interfaces
{
    public interface IService
    {
        int ServiceID { get; set; }
        string ServiceType { get; set; }
        string ServiceDescription { get; set; }
    }
}
