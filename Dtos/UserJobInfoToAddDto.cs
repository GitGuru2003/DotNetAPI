namespace DotnetAPI.Dtos
{
  public partial class UserJobInfoToAddDto
  {
    public string JobTitle { get; set; } = "";
    public string Department { get; set; } = "";

    public UserJobInfoToAddDto()
    {
      if (JobTitle == null) JobTitle = "";
      if (Department == null) Department = "";
    }

  }
}