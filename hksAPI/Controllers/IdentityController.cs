using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    public class IdentityController : ControllerBase
    {
        private const string TokenSecret = "SecuryThisINeedToPassMyFinalExam";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);



        [HttpPost("token")]
        public IActionResult GenrateToken([FromBody]TokenGenerationRequest request)
        {
            return Redirect("home"); 
        }
    }
}
