using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PackInfoController : ControllerBase
    {
        private readonly ICrudGeneric<PackInfo> _packInfoRepository;

        public PackInfoController(ICrudGeneric<PackInfo> packInfoRepository)
        {
            _packInfoRepository = packInfoRepository;
        }

        [HttpGet("getPackInfo")]
        public IActionResult GetPackInfo(string contactInfo)
        {
            try
            {
                PackInfo packInfo = _packInfoRepository.GetByName(contactInfo);
                return Ok(packInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("postPack")]
        public IActionResult PostPack(PackInfo packInfo)
        {
            try
            {
                _packInfoRepository.Update(packInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
