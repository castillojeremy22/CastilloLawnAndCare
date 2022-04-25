namespace CastilloLawnCare.Models.DataModels
{
    public class dmCustomerReview
    {
        public int ReviewID { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }

        public dmCustomerReview(int reviewID, string description, string userID, DateTime reviewDate, int rating)
        {
            ReviewID = reviewID;
            Description = description;
            UserID = userID;
            ReviewDate = reviewDate;
            Rating = rating;
        }

        public dmCustomerReview()
        {
        }
    }
}
