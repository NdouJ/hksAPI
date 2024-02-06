using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreederController : ControllerBase
    {

        ICrudGeneric<Breeder> _breederRepository;
        public BreederController(ICrudGeneric<Breeder> breederRepository)
        {
            _breederRepository = breederRepository; 
        }
        [HttpGet("CheckOib")]
        public IActionResult CheckOib(string oib)
        {
            if (string.IsNullOrEmpty(oib) || !Regex.IsMatch(oib, "^[0-9]{11}$"))
                return BadRequest("Invalid OIB format.");

            var oibSpan = oib.AsSpan();
            var a = 10;
            for (var i = 0; i < 10; i++)
            {
                if (!int.TryParse(oibSpan.Slice(i, 1), out int number))
                    return BadRequest("Invalid OIB format.");

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

            return isValid ? Ok("OIB is valid.") : BadRequest("Invalid OIB.");
        }


        [HttpGet("breeders-of-dog")]
        public IActionResult GetBreedersOfDog(string dog)
        {
            IEnumerable<Breeder> breeders = _breederRepository.GetAllByParametar(dog);

            return Ok(breeders.ToList()); 
        }

        [HttpGet]
        public IActionResult GetBreederbyName(string breederName)
        {

            Breeder breeder = _breederRepository.GetByName(breederName); 

            return Ok(breeder);
        }

        [HttpPost("updateBreeder")]
        public IActionResult UpdateBreeder(Breeder breederUpdate)
        {

         _breederRepository.Update(breederUpdate);

            return Ok();
        }
        [HttpPost]
        public IActionResult PostBreeder(Breeder breeder)
        {

            _breederRepository.Insert(breeder); 

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteBreeder(int  id)
        {

            _breederRepository.Delete(id);

            return Ok(); 
        }

    }
}
