using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserEFController : ControllerBase
  {
    // DataContextEF _dataContextEF;
    IMapper _mapper;
    IUserRepository _userRepository;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
      _userRepository = userRepository;
      // _dataContextEF = new DataContextEF(config);
      _mapper = new Mapper(new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<UserToAddDto, User>();
        cfg.CreateMap<UserJobInfoToAddDto, UserJobInfo>();
        cfg.CreateMap<UserSalaryToAddDto, UserSalary>();
      }));
    }

    #region User CRUD
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
      // return _dataContextEF.Users.ToList();
      return _userRepository.GetUsers();
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
      // User? user = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
      // if (user != null)
      // {
      //   return user;
      // }
      // throw new Exception("User not found");
      return _userRepository.GetSingleUser(userId);
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
      // if (user == null) throw new Exception("User is null");
      // User userDb = new User();
      // userDb.FirstName = user.FirstName;
      // userDb.LastName = user.LastName;
      // userDb.Email = user.Email;
      // userDb.Active = user.Active;
      // userDb.Gender = user.Gender;
      User userDb = _mapper.Map<User>(user);

      _userRepository.AddEntity<User>(userDb);
      // _dataContextEF.Add(userDb);
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
      throw new Exception("User not added");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
      // User? userToEdit = _dataContextEF.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();

      User? userToEdit = _userRepository.GetSingleUser(user.UserId);

      if (userToEdit != null)
      {
        userToEdit.FirstName = user.FirstName;
        userToEdit.LastName = user.LastName;
        userToEdit.Email = user.Email;
        userToEdit.Gender = user.Gender;
        userToEdit.Active = user.Active;

        if (_userRepository.SaveChanges())
        {
          return Ok();

        }
        throw new Exception("User not updated");


      }
      throw new Exception("User not found");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
      // User? user = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
      User? user = _userRepository.GetSingleUser(userId);
      if (user != null)
      {
        _userRepository.RemoveEntity<User>(user);
        // _dataContextEF.Users.Remove(user);
        if (_userRepository.SaveChanges()) return Ok();
        throw new Exception("User not deleted");
      }
      throw new Exception("User not found");

    }
    #endregion

    #region UserJobInfo CRUD
    // UserJobInfo API Endpoints
    // CRUD 
    // Create - Add (POST)
    // Read - Get (GET)
    // Update - Edit - PUT
    // Delete - Delete - DELETE
    [HttpPost("AddJobInfo")]
    public IActionResult AddJobInfo(UserJobInfoToAddDto userJobInfo)
    {
      UserJobInfo userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfo);
      // _dataContextEF.UserJobInfos.Add(userJobInfoDb);
      _userRepository.AddEntity<UserJobInfo>(userJobInfoDb);
      if (_userRepository.SaveChanges()) return Ok();
      throw new Exception("User JobInfo could not be added");
    }

    [HttpGet("GetJobInfo")]
    public IEnumerable<UserJobInfo> GetJobInfos()
    {
      // return _dataContextEF.UserJobInfos.ToList();
      return _userRepository.GetUserJobInfos();
    }
    [HttpGet("GetSingleJobInfo/{userId}")]
    public UserJobInfo GetSingleJobInfo(int userId)
    {
      // UserJobInfo? userJobInfo = _dataContextEF.UserJobInfos.Where(u => u.UserId == userId).FirstOrDefault();
      UserJobInfo? userJobInfo = _userRepository.GetSingleUserJobInfo(userId);
      if (userJobInfo != null) return userJobInfo;
      throw new Exception("Could not find the job info at the given user id");
    }

    [HttpPut("EditJobInfo")]
    public IActionResult EditJobInfo(UserJobInfo userJobInfo)
    {
      // UserJobInfo? userJobInfoToEdit = _dataContextEF.UserJobInfos.Where(u => u.UserId == userJobInfo.UserId).FirstOrDefault();
      UserJobInfo? userJobInfoToEdit = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId);

      if (userJobInfoToEdit != null)
      {
        userJobInfoToEdit.JobTitle = userJobInfo.JobTitle;
        userJobInfoToEdit.Department = userJobInfo.Department;
        if (_userRepository.SaveChanges()) return Ok();
        throw new Exception("User JobInfo not edited");
      }
      throw new Exception("User JobInfo not found");
    }

    [HttpDelete("DeleteUserJobInfo")]
    public IActionResult DeleteUserJobInfo(int userId)
    {

      // UserJobInfo? UserJobInfoToDelete = _dataContextEF.UserJobInfos.Where(u => u.UserId == userId).FirstOrDefault();
      UserJobInfo? UserJobInfoToDelete = _userRepository.GetSingleUserJobInfo(userId);
      if (UserJobInfoToDelete != null)
      {
        // _dataContextEF.UserJobInfos.Remove(UserJobInfoToDelete);
        _userRepository.RemoveEntity<UserJobInfo>(UserJobInfoToDelete);
        if (_userRepository.SaveChanges())
        {
          return Ok();
        }
        throw new Exception("User JobInfo not deleted");
      }
      throw new Exception("User JobInfo not found");
    }

    #endregion


    #region UserSalary CRUD
    // GetUserSalaries
    [HttpGet("GetUserSalaries")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
      // return _dataContextEF.UserSalaries.ToList();
      return _userRepository.GetUserSalaries();
    }
    // GetUserSalary
    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
      // UserSalary? userSalary = _dataContextEF.UserSalaries.Where(u => u.UserId == userId).FirstOrDefault();
      UserSalary? userSalary = _userRepository.GetSingleUserSalary(userId);

      if (userSalary != null)
      {
        return userSalary;
      }
      throw new Exception("Failed to get the UserSalary");
    }
    // AddUserSalary
    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalaryToAddDto userSalary)
    {
      UserSalary userSalaryDb = _mapper.Map<UserSalary>(userSalary);
      // _dataContextEF.UserSalaries.Add(userSalaryDb);
      _userRepository.AddEntity<UserSalary>(userSalaryDb);
      if (_userRepository.SaveChanges()) return Ok();
      throw new Exception("User Salary could not be added");
    }
    // EditUserSalary
    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
      // UserSalary? userSalaryToEdit = _dataContextEF.UserSalaries.Where(u => u.UserId == userSalary.UserId).FirstOrDefault();
      UserSalary? userSalaryToEdit = _userRepository.GetSingleUserSalary(userSalary.UserId);
      if (userSalaryToEdit != null)
      {
        userSalaryToEdit.Salary = userSalary.Salary;
        if (_userRepository.SaveChanges())
        {
          return Ok();
        }
        throw new Exception("User Salary not edited");
      }
      throw new Exception("User Salary not found");
    }

    // DeleteUserSalary
    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
      // UserSalary? userSalaryToDelete = _dataContextEF.UserSalaries.Where(u => u.UserId == userId).FirstOrDefault();
      UserSalary? userSalaryToDelete = _userRepository.GetSingleUserSalary(userId);

      if (userSalaryToDelete != null)
      {
        _userRepository.RemoveEntity<UserSalary>(userSalaryToDelete);
        // _dataContextEF.UserSalaries.Remove(userSalaryToDelete);
        if (_userRepository.SaveChanges())
        {
          return Ok();
        }
        throw new Exception("UserSalary not deleted");
      }
      throw new Exception("UserSalary not found");
    }
    #endregion
  }
}