namespace hksAPI.Models
{
    public class Donation
    {
        public int IdDonation { get; set; }  

        public DateTime DonationTime { get; set; } = DateTime.Now; 

        public decimal Amount { get; set; }  

        public int UserId { get; set; }  

    }
}
