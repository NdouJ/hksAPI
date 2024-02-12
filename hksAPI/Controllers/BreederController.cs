using hksAPI.Data.Repositories;
using hksAPI.Models;
using hksAPI.Services;
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

        [HttpGet("check-oib")]
        public IActionResult CheckOib(string OIB)
        {
            OIBService oIBService = new OIBService();
            string res = oIBService.CheckOib(OIB);

            if (res.Contains("valid"))
            {
                return Ok(res);
            }

            else
            {
                return BadRequest(res); 
            }
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
