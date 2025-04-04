using DotnetAPI.Models;

namespace DotnetAPI.Data
{
  public class UserRepository : IUserRepository
  {
    DataContextEF _dataContextEF;
    public UserRepository(IConfiguration config)
    {
      _dataContextEF = new DataContextEF(config);
    }

    public bool SaveChanges()
    {
      return _dataContextEF.SaveChanges() > 0;
    }

    public void AddEntity<T>(T entityToAdd)
    {
      if (entityToAdd != null)
      {
        _dataContextEF.Add(entityToAdd);
      }
    }
    public void RemoveEntity<T>(T entityToRemove)
    {
      if (entityToRemove != null)
      {
        _dataContextEF.Remove(entityToRemove);
      }
    }

    public IEnumerable<User> GetUsers()
    {
      return _dataContextEF.Users.ToList();
    }
    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
      return _dataContextEF.UserJobInfos.ToList();
    }
    public IEnumerable<UserSalary> GetUserSalaries()
    {
      return _dataContextEF.UserSalaries.ToList();
    }

    public User GetSingleUser(int userId)
    {
      User? user = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
      if (user != null)
      {
        return user;
      }
      throw new Exception("User not found");
    }
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
      UserJobInfo? userJobInfo = _dataContextEF.UserJobInfos.Where(u => u.UserId == userId).FirstOrDefault();
      if (userJobInfo != null)
      {
        return userJobInfo;
      }
      throw new Exception("User Job Info not found");
    }
    public UserSalary GetSingleUserSalary(int userId)
    {
      UserSalary? userSalary = _dataContextEF.UserSalaries.Where(u => u.UserId == userId).FirstOrDefault();
      if (userSalary != null)
      {
        return userSalary;
      }
      throw new Exception("User Salary not found");
    }

  }
}