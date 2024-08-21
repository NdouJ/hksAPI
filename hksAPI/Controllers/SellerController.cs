using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace hksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ICrudGeneric<Seller> _sellerRepository;

        public SellerController(ICrudGeneric<Seller> sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetSellerById(int id)
        {
            try
            {
                Seller seller = _sellerRepository.GetById(id);
                if (seller == null)
                {
                    return NotFound("Seller not found");
                }
                return Ok(seller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-all")]
        public IActionResult GetAllSellers()
        {
            try
            {
                return Ok(_sellerRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("updateSeller")]
        public IActionResult UpdateSeller(Seller sellerUpdate)
        {
            try
            {
                _sellerRepository.Update(sellerUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostSeller(Seller seller)
        {
            try
            {
                _sellerRepository.Insert(seller);
                return Ok();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key violation error number
                {
                    return BadRequest("Seller must be registered as a breeder with HKS");
                }
                else
                {
                    return StatusCode(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeller(int id)
        {
            try
            {
                _sellerRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
