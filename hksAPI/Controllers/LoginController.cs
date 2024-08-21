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
        public LoginController(ICrudGeneric<LoginRequest> loginRepositoty, IJwtTokenCrud jwtTokenCrud)
        {
            _loginRepository = loginRepositoty;
            _jwtTokenCrud = jwtTokenCrud;
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
        public IActionResult Get()
        {
            
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


