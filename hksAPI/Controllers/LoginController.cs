using hksAPI.Data.Repositories;
using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        ICrudGeneric<LoginRequest> _loginRepository;
        public LoginController(ICrudGeneric<LoginRequest> loginRepositoty)
        {
            _loginRepository = loginRepositoty;
        }

        [HttpPost("getToken")]
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

        [HttpPost("saveUser")]
        public IActionResult InsertUser([FromBody] LoginRequest loginRequest)
        {
            try
            {
             _loginRepository.Insert(loginRequest);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message); 
            }


            return Ok(loginRequest.Username);
        }


    }




}


