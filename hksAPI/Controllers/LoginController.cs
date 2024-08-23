using hksAPI.Data.Repositories;
using hksAPI.Interfaces;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        ICrudGeneric<LoginRequest> _loginRepository;
        IJwtTokenCrud _jwtTokenCrud; 
        IConfiguration _configuration;
        public LoginController(ICrudGeneric<LoginRequest> loginRepositoty, IJwtTokenCrud jwtTokenCrud, IConfiguration configuration)
        {
            _loginRepository = loginRepositoty;
            _jwtTokenCrud = jwtTokenCrud;
            _configuration = configuration;
        }

        [HttpPost("getTokenForUser")]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
            var token = "";

            try
            {
                token = _loginRepository.CheckEntity(loginRequest);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok(token);

        }

        [HttpGet("getToken")]
        public IActionResult Get(string apiKey)
        {
            if (apiKey == null || !apiKey.Equals(_configuration["apiKey"]))
            {
                return Forbid();
            }
            
            var token = "";

            try
            {
                token = _jwtTokenCrud.CreateJwtToken(); 
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

            return Ok(token);

        }

    }



}


