namespace hksAPI.Models
{
    public class BreederPack
    {
        public BreederPack()
        {
            Breeder = new Breeder(); 
            Pack = new Pack();      
        }
        public Breeder Breeder { get; set; }
        public Pack Pack { get; set; }
    }
}
