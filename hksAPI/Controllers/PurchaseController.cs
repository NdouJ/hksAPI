using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        ICrudGeneric<Purchase> _purchaseRepository;

        public PurchaseController(ICrudGeneric<Purchase> purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        [HttpGet("get-all-purchase")]
        public IActionResult GetAllPurchases()
        {
            try
            {
                var purchases = _purchaseRepository.GetAll();
                return Ok(purchases); // Return 200 OK with the list of purchases
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get-all-purchase-of-a-user")]
        public IActionResult GetAllPurchasesFromUser(string id)
        {
            try
            {
                var purchases = _purchaseRepository.GetAllByParametar(id);

                if (purchases == null || !purchases.Any())
                {
                    return NotFound(); 
                }
                return Ok(purchases); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreatePurchase([FromBody] Purchase value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(); // Return 400 Bad Request if the request body is null
                }

                _purchaseRepository.Insert(value);

                
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
