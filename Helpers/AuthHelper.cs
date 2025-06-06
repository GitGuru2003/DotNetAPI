using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
  public class AuthHelper
  {
    private readonly IConfiguration _config;
    public AuthHelper(IConfiguration config)
    {
      _config = config;
    }
    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
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

    public string CreateToken(int userId)
    {
      Claim[] claims = new Claim[]{
        new Claim("userId", userId.ToString()) // "userId" is the identifier for the claim and userId.ToString() is the value of the claim.
      };

      string? tokenKey = _config.GetSection("AppSettings:TokenKey").Value;

      SymmetricSecurityKey symmetrickey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(tokenKey)
      );

      SigningCredentials credentials = new SigningCredentials(
        symmetrickey, SecurityAlgorithms.HmacSha512
      );

      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
      {
        Subject = new ClaimsIdentity(claims),
        SigningCredentials = credentials,
        Expires = DateTime.Now.AddDays(1)
      };

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      string tokenString = tokenHandler.WriteToken(token);

      return tokenString;
    }

  }
}