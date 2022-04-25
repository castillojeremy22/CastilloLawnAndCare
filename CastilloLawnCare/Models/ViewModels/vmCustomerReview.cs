namespace CastilloLawnCare.Models
{
    namespace ViewModels
    {
        public class vmCustomerReview : User
        {
            public int ReviewID { get; set; }
            public string Description { get; set; }
            public DateTime ReviewDate { get; set; }
            public int Rating { get; set; }

            public vmCustomerReview()
            {
               
            }

            public vmCustomerReview(int reviewID, string description, DateTime reviewDate, int rating)
            {
                ReviewID = reviewID;
                Description = description;
                ReviewDate = reviewDate;
                Rating = rating;
            }
        }
    }
    
}
