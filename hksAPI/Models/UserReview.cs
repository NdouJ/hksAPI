namespace hksAPI.Models
{
    public class UserReview
    {
        public int IdUserReview { get; set; }
        public int Grade { get; set; }
        public string Review { get; set; }
        public int UserId { get; set; }
        public int BreederId { get; set; }
    }
}
