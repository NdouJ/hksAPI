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
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("breeders-of-dog")]
        public IActionResult GetBreedersOfDog(string dog)
        {
            try
            {
                IEnumerable<Breeder> breeders = _breederRepository.GetAllByParametar(dog);
                return Ok(breeders.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetBreederbyName(string breederName)
        {
            try
            {
                Breeder breeder = _breederRepository.GetByName(breederName);
                if (breeder == null)
                {
                    return NotFound("Breeder not found");
                }
                return Ok(breeder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("updateBreeder")]
        public IActionResult UpdateBreeder(Breeder breederUpdate)
        {
            try
            {
                _breederRepository.Update(breederUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult PostBreeder(Breeder breeder)
        {
            try
            {
                _breederRepository.Insert(breeder);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteBreeder(int  id)
        {
            try
            {
                _breederRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
