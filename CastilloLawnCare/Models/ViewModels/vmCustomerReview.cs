namespace CastilloLawnCare.Models
{
    namespace ViewModels
    {
        public class vmCustomerReview : User
        {
            public int ReviewID { get; set; }
            public string Description { get; set; }

            public vmCustomerReview()
            {
            }

            public vmCustomerReview(int reviewID, string description)
            {
                ReviewID = reviewID;
                Description = description;
            }
        }
    }
    
}
