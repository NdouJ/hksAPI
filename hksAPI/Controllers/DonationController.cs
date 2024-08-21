using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly ICrudGeneric<Donation> _donationRepository;

        public DonationController(ICrudGeneric<Donation> donationRepository)
        {
            _donationRepository = donationRepository;
        }


        [HttpPost]
        public IActionResult InsertDonation([FromBody] Donation value)
        {
            if (value == null)
            {
                return BadRequest("Donation data is required.");
            }
            try
            {
                 _donationRepository.Insert(value);
                return Created(value.ToString(),value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
