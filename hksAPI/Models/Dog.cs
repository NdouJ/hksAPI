namespace hksAPI.Models
{
    public class Dog
    {
        private string _breedName; 
        public int IdDog { get; set; }
        public string BreedName {
            get
            {
                return _breedName;
            }

            set
            {
                _breedName = value.ToUpper().Replace('Č', 'C').Replace('Ć', 'C');
            }
        }
        public int AvgWeightFemale { get; set; }
        public int AvgWeightMale { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
