using DotnetAPI.Data;
using DotNetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers
{
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;
    public AuthController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
      _config = config;
    }

    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForResgistration)
    {
      return Ok();
    }


    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
      return Ok();
    }


  }
}