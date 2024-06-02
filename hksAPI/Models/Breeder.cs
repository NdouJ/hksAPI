using Newtonsoft.Json;
using System;
using System.Linq;

namespace hksAPI.Models
{
    public class Breeder
    {
        [JsonIgnore]
        public string BreederInfo { get; set; }

        private string _breederName;
        private string _breederContact;

        public int IdBreeder { get; set; }
        public string OIB { get; set; }

        public string BreederName
        {
            get
            {
                if (_breederName != null)
                {
                    return _breederName;
                }
                if (!string.IsNullOrEmpty(BreederInfo))
                {
                    string breederName = new string(BreederInfo.TakeWhile(c => !char.IsDigit(c)).ToArray()).Trim();
                    _breederName = breederName.ToUpper().Replace('Č', 'C').Replace('Ć', 'C').Replace('Đ', 'D').Replace('Ž', 'Z');

                    return _breederName;
                }

                return "";
            }

            set
            {
                _breederName = value.ToUpper().Replace('Č', 'C').Replace('Ć', 'C').Replace('Đ','D').Replace('Ž','Z'); 
            }

        }

        public string BreederContact
        {
            get
            {
                if (_breederContact != null)
                {
                    return _breederContact;
                }
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
            set
            {
                _breederContact = value;
            }
        }
    }
}
