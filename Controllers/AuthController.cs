using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Helpers;
using DotNetAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotNetAPI.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    // private readonly IConfiguration _config; // readonly means that it can be assigned value only once.
    private readonly DataContextDapper _dapper;
    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
      // _config = config;
      _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
      if (userForRegistration.Password == userForRegistration.PasswordConfirm)
      {
        string sqlCheckUserExists = @$"SELECT [Email] FROM TutorialAppSchema.Auth WHERE Email = '{userForRegistration.Email}';";
        IEnumerable<string> userExists = _dapper.LoadData<string>(sqlCheckUserExists);

        if (userExists.Count() == 0)
        {
          byte[] passwordSalt = new byte[128 / 8];
          using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
          {
            rng.GetNonZeroBytes(passwordSalt);
          }

          byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

          string sqlAddAuth = @$"INSERT INTO TutorialAppSchema.Auth ([Email], [PasswordHash], [PasswordSalt])
                                   VALUES ('{userForRegistration.Email}', @PasswordHash, @PasswordSalt);";

          List<SqlParameter> sqlParameters = new List<SqlParameter>();

          SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
          passwordSaltParameter.Value = passwordSalt;

          SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
          passwordHashParameter.Value = passwordHash;

          sqlParameters.Add(passwordSaltParameter);
          sqlParameters.Add(passwordHashParameter);

          if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
          {
            string sqlAddUser = @$"INSERT INTO TutorialAppSchema.Users
                                  (FirstName, LastName, Email, Gender, Active)
                                  VALUES ('{userForRegistration.FirstName}',
                                         '{userForRegistration.LastName}',
                                         '{userForRegistration.Email}',
                                         '{userForRegistration.Gender}',
                                           1); ";

            if (_dapper.ExecuteSqlBool(sqlAddUser))
            {
              return Ok();
            }
            throw new Exception("Failed to add user");
          }
          throw new Exception("Failed to register user");
        }
        throw new Exception("User with this email already exists");
      }
      throw new Exception("Passwords do not match");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {

      string sqlForHashAndSalt = @$"SELECT [PasswordHash], [PasswordSalt] FROM TutorialAppSchema.Auth
                                 WHERE Email = '{userForLogin.Email}';";
      UserForLoginConfirmationDto userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

      byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

      for (int i = 0; i < passwordHash.Length; i++)
      {
        if (passwordHash[i] != userForConfirmation.PasswordHash[i])
        {
          return StatusCode(401, "Invalid Password");
        }
      }

      string userIdSql = @$"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '{userForLogin.Email}'";

      int userId = _dapper.LoadDataSingle<int>(userIdSql);


      return Ok(new Dictionary<string, string>(){
        {"token", _authHelper.CreateToken(userId)}
      });
    }

    [HttpGet("RefreshToken")]
    public IActionResult RefreshToken()
    {
      string userId = User.FindFirst("userId")?.Value + "";
      string sqlForUserId = @$"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = {userId}";

      int userIdFromDb = _dapper.LoadDataSingle<int>(sqlForUserId);

      return Ok(new Dictionary<string, string>(){
        {"token", _authHelper.CreateToken(userIdFromDb)}
      });
    }
  }
}