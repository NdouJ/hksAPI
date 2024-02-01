using System;
using System.Linq;

namespace hksAPI.Models
{
    public class Breeder
    {
        public string BreederInfo { get; set; }

        public string BreederName
        {
            get
            {
                if (!string.IsNullOrEmpty(BreederInfo))
                {
                    string breederName = new string(BreederInfo.TakeWhile(c => !char.IsDigit(c)).ToArray()).Trim();

                    return breederName;
                }

                return "";
            }
        }

        public string BreederContact
        {
            get
            {
                if (!string.IsNullOrEmpty(BreederInfo))
                {
                    // Skip characters until the first digit is encountered
                    int firstDigitIndex = BreederInfo.IndexOfAny("0123456789".ToCharArray());

                    if (firstDigitIndex >= 0)
                    {
                        string breederContact = BreederInfo.Substring(firstDigitIndex).Trim();
                        return breederContact;
                    }
                }

                return "";
            }
        }
    }
}
