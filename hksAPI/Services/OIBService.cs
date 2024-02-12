using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace hksAPI.Services
{
    public class OIBService
    {
       

        public string CheckOib(string oib)
        {
            if (string.IsNullOrEmpty(oib) || !Regex.IsMatch(oib, "^[0-9]{11}$"))
                return "Invalid OIB format.";

            var oibSpan = oib.AsSpan();
            var a = 10;
            for (var i = 0; i < 10; i++)
            {
                if (!int.TryParse(oibSpan.Slice(i, 1), out int number))
                    return "Invalid OIB format.";

                a += number;
                a %= 10;

                if (a == 0)
                    a = 10;

                a *= 2;
                a %= 11;
            }

            var kontrolni = 11 - a;

            if (kontrolni == 10)
                kontrolni = 0;

            bool isValid = kontrolni == int.Parse(oibSpan.Slice(10, 1));

            return isValid ? "OIB is valid." : "Invalid OIB.";
        }

    }
}
