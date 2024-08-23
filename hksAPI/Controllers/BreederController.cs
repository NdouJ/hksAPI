using hksAPI.Data.Repositories;
using hksAPI.Models;
using hksAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
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
        [Authorize]
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


        //[Authorize]
        [HttpGet("getBreederByName")]
        public IActionResult GetBreederByName(string name)
        {
            {
                try
                {
                    var breeder = _breederRepository.GetByName(name);

                    return Ok(breeder);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [Authorize]
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


        [Authorize]
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

        [Authorize]
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


        [Authorize]
        [HttpGet("getAllBreeders")]
        public IActionResult GetBreeders() {

            try
            {
                IEnumerable<Breeder> breeders = _breederRepository.GetAll();
                return Ok(breeders.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
