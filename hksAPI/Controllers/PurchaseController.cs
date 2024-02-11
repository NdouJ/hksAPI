using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
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
        public IEnumerable<Purchase> Get()
        {
            return _purchaseRepository.GetAll();
        }

        [HttpGet("get-all-purchase-of-a-user")]
        public IEnumerable<Purchase> GetAllFromUser( string id)
        {
            return _purchaseRepository.GetAllByParametar(id);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Purchase value)
        {
            _purchaseRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = value.IdPack }, value);

        }
    }
}
