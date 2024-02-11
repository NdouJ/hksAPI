namespace hksAPI.Models
{
    public class Purchase
    {
        public int UserID { get; set; }
        public int IdPack { get; set; }
        public int Quantity { get; set; }
        public int SellerID { get; set; }

        public string BreedName { get; set; }
        public string BreederName { get; set; }
        public decimal Price { get; set; }
    }
}
