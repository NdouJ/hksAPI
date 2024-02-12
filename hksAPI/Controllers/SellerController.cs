using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController  : ControllerBase
    {
        ICrudGeneric<Seller> _sellerRepository;
        public SellerController(ICrudGeneric<Seller> sellerRepository)
        {
            _sellerRepository = sellerRepository; 
        }

        [HttpGet]
        public IActionResult GetSellerbyName(string sellerName)
        {

            Seller seller = _sellerRepository.GetByName(sellerName);

            return Ok(seller);
        }

        [HttpPost("updateSeller")]
        public IActionResult UpdateSellee(Seller sellerUpdate)
        {

            _sellerRepository.Update(sellerUpdate);

            return Ok();
        }
        [HttpPost]
        public IActionResult PostSeller(Seller seller)
        {

            _sellerRepository.Insert(seller);

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
