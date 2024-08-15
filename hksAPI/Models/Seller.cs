namespace hksAPI.Models
{
    public class Seller
    {


        public string TempPassword { get; set; }

        public int IdSeller { get; set; }
        public string BreederName { get; set; }
        public string ContactInfo { get; set; }
        public string OIB { get; set; }

        private string GeneratePassword()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var password = new char[4];

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = characters[random.Next(characters.Length)];
            }

            return new string(password);
        }

    }
}
