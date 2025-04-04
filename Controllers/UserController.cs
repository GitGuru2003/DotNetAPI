using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);

    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
      return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    #region User CRUD
    // Get all of the users
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
      string sql = @"SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
        FROM  TutorialAppSchema.Users";
      IEnumerable<User> users = _dapper.LoadData<User>(sql);
      return users;
    }

    // Get a single user
    [HttpGet("GetUsers/{userId}")]
    public User GetSingleUser(int userId)
    {
      string sql = @$"SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
          FROM  TutorialAppSchema.Users
          WHERE [UserId] = {userId};";
      User user = _dapper.LoadDataSingle<User>(sql);
      return user;
    }


    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
      string sql = @$"
      UPDATE TutorialAppSchema.Users
      SET [FirstName] = '{user.FirstName}',
          [LastName] = '{user.LastName}',
          [Email] = '{user.Email}',
          [Gender] = '{user.Gender}',
          [Active] = '{user.Active}'
      WHERE UserId = {user.UserId};
      ";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to update the user.");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
      int isActive = user.Active ? 1 : 0;
      string sql = @$"
      INSERT INTO TutorialAppSchema.Users
        (FirstName, LastName, Email, Gender, Active)
      VALUES 
        ('{user.FirstName}',
         '{user.LastName}',
         '{user.Email}',
         '{user.Gender}',
          {isActive}); ";

      if (_dapper.ExecuteSqlBool(sql)) return Ok();
      throw new Exception("Failed to add the user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
      string sql = @$"DELETE FROM TutorialAppSchema.Users WHERE UserId = {userId}";

      if (_dapper.ExecuteSqlBool(sql)) return Ok();
      throw new Exception("Failed to delete the user");
    }

    #endregion


    #region UserJobInfo CRUD
    [HttpGet("GetJobInfos")]
    public IEnumerable<UserJobInfo> GetJobInfos()
    {
      string sql = @"SELECT  [UserId]
        , [JobTitle]
        , [Department]
        FROM  TutorialAppSchema.UserJobInfo;";
      return _dapper.LoadData<UserJobInfo>(sql);
    }
    [HttpGet("GetSingleJobInfo/{userId}")]
    public UserJobInfo GetSingleJobInfo(int userId)
    {
      string sql = @$"SELECT  [UserId]
        , [JobTitle]
        , [Department]
        FROM  TutorialAppSchema.UserJobInfo WHERE UserId = {userId};";
      return _dapper.LoadDataSingle<UserJobInfo>(sql);
    }

    [HttpPost("AddUserJob")]
    public IActionResult AddUserJob(UserJobInfoToAddDto userJobInfo)
    {
      string sql = @$"INSERT INTO TutorialAppSchema.UserJobInfo
       (JobTitle, Department) VALUES ('{userJobInfo.JobTitle}', '{userJobInfo.Department}');";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to add the userJobInfo.");

    }

    [HttpPut("EditJobInfo")]
    public IActionResult EditJobInfo(UserJobInfo userJobInfo)
    {
      string sql = @$"UPDATE TutorialAppSchema.UserJobInfo
                      SET [JobTitle] = '{userJobInfo.JobTitle}',
                          [Department] = '{userJobInfo.Department}'
                           WHERE UserId = {userJobInfo.UserId};";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to update the UserJobInfo");
    }
    [HttpDelete("DeleteJobInfo/{userId}")]
    public IActionResult DeleteJobInfo(int userId)
    {
      string sql = @$"DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = {userId}";

      if (_dapper.ExecuteSqlBool(sql)) return Ok();
      throw new Exception("Failed to delete the userJobInfo");
    }
    #endregion

    #region UserSalary CRUD
    [HttpGet("GetUserSalary")]
    public IEnumerable<UserSalary> GetUserSalary()
    {
      string sql = @"SELECT  [UserId]
                            , [Salary]
                      FROM  TutorialAppSchema.UserSalary;";

      return _dapper.LoadData<UserSalary>(sql);
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
      string sql = @$"SELECT  [UserId]
        , [Salary]
        FROM  TutorialAppSchema.UserSalary WHERE UserId = {userId};";
      return _dapper.LoadDataSingle<UserSalary>(sql);
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalaryToAddDto userSalary)
    {
      string sql = @$"INSERT INTO TutorialAppSchema.UserSalary
       (Salary) VALUES ({userSalary.Salary});";

      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to add the UserSalary.");

    }

    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
      string sql = @$"UPDATE TutorialAppSchema.UserSalary
                      SET [Salary] = {userSalary.Salary}
                           WHERE UserId = {userSalary.UserId};";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to update the UserSalary");
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
      string sql = @$"DELETE FROM TutorialAppSchema.UserSalary WHERE UserId = {userId}";

      if (_dapper.ExecuteSqlBool(sql)) return Ok();
      throw new Exception("Failed to delete the DeleteUserSalary");
    }
    #endregion

  }
}