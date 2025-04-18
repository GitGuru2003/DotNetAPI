using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
  public class DataContextDapper
  {
    private readonly IConfiguration _config;
    private string _connectionString;

    public DataContextDapper(IConfiguration config)
    {
      _config = config;
      _connectionString = _config.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection");
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.QuerySingle<T>(sql);
    }
    public bool ExecuteSqlBool(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteSqlWithRowCount(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
    {
      SqlCommand sqlCommand = new SqlCommand(sql);

      foreach (SqlParameter parameter in parameters)
      {
        sqlCommand.Parameters.Add(parameter);
      }

      SqlConnection dbConnection = new SqlConnection(_connectionString);
      dbConnection.Open();
      sqlCommand.Connection = dbConnection;

      int rowsAffected = sqlCommand.ExecuteNonQuery();
      dbConnection.Close();

      return rowsAffected > 0;
    }
  }
}