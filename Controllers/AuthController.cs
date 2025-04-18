using System.Data;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotNetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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


          // string? passwordSaltString = _config.GetSection("AppSettings:PasswordKey").Value
          //                               + Convert.ToBase64String(passwordSalt);
          // byte[] passwordHash = KeyDerivation.Pbkdf2(
          //   password: userForRegistration.Password,
          //   salt: Encoding.ASCII.GetBytes(passwordSaltString),
          //   prf: KeyDerivationPrf.HMACSHA256,
          //   iterationCount: 100000,
          //   numBytesRequested: 256 / 8
          // );

          byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

          string sqlAddAuth = @$" INSERT INTO TutorialAppSchema.Auth ([Email], [PasswordHash], [PasswordSalt])
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
            return Ok();
          }
          throw new Exception("Failed to register user");
        }
        throw new Exception("User with this email already exists");
      }
      throw new Exception("Passwords do not match");
    }


    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {

      string sqlForHashAndSalt = @$"SELECT [PasswordHash], [PasswordSalt] FROM TutorialAppSchema.Auth
                                 WHERE Email = '{userForLogin.Email}';";
      UserForLoginConfirmationDto userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

      byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

      for (int i = 0; i < passwordHash.Length; i++)
      {
        if (passwordHash[i] != userForConfirmation.PasswordHash[i])
        {
          return StatusCode(401, "Invalid Password");
        }
      }
      return Ok();

      // if (passwordHash.SequenceEqual(userForConfirmation.PasswordHash))
      // {
      // }
      // throw new Exception("Invalid password");
    }

    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
      string? passwordSaltString = _config.GetSection("AppSettings:PasswordKey").Value
                                        + Convert.ToBase64String(passwordSalt);
      byte[] passwordHash = KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.ASCII.GetBytes(passwordSaltString),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8
      );

      return passwordHash;
    }


  }
}