using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        ICrudGeneric<Seller> _sellerRepository;
        public SellerController(ICrudGeneric<Seller> sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        [HttpGet]
        public IActionResult GetSellerbyId(int id)
        {

            Seller seller = _sellerRepository.GetById(id);

            return Ok(seller);
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {



            return Ok(_sellerRepository.GetAll());
        }
        [HttpPost("updateSeller")]
        public IActionResult UpdateSellee(Seller sellerUpdate)
        {



            return Ok();
        }
        [HttpPost]
        public IActionResult PostSeller(Seller seller)
        {

            try
            {
                _sellerRepository.Insert(seller);

            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key violation error number
                {
                    return BadRequest("Seller must be registrated as a breeder with HKS");
                }
                else
                {

                }
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteSeller(int id)
        {

            _sellerRepository.Delete(id);

            return Ok();
        }

    }
}
