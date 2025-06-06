namespace DotNetAPI.Dtos
{
  public partial class UserForLoginConfirmationDto
  {
    public byte[] PasswordHash { get; set; } // Password hash is a byte array here and password hash is the result 
                                             // of passing the password + the salt through the hashing function.
    public byte[] PasswordSalt { get; set; } // Password salt is a byte array here and 
                                             // password salt is a random byte array that is appended to the 
                                             // password before hashing.
    public UserForLoginConfirmationDto()
    {
      PasswordHash ??= new byte[0];
      PasswordSalt ??= new byte[0];
    }
  }
}