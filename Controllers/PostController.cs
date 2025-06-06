using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class PostController : ControllerBase
  {
    private readonly DataContextDapper _dapper;

    public PostController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
    }

    #region Post CRUD
    [HttpGet("Posts")]
    public IEnumerable<Post> GetPosts()
    {
      string sql = @"SELECT [PostId], 
                            [UserId], 
                            [PostTitle], 
                            [PostContent], 
                            [PostCreated], 
                            [PostUpdated]  FROM TutorialAppSchema.Posts;";
      return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("PostSingle/{postId}")]
    public Post GetSinglePost(int postId)
    {
      string sql = @$"SELECT [PostId], 
                            [UserId], 
                            [PostTitle], 
                            [PostContent], 
                            [PostCreated], 
                            [PostUpdated]  FROM TutorialAppSchema.Posts WHERE PostId = {postId};";
      return _dapper.LoadDataSingle<Post>(sql);
    }

    [HttpGet("PostByUser/{userId}")]
    public IEnumerable<Post> GetPostsByUser(int userId)
    {
      string sql = @$"SELECT [PostId], 
                            [UserId], 
                            [PostTitle], 
                            [PostContent], 
                            [PostCreated], 
                            [PostUpdated]  FROM TutorialAppSchema.Posts WHERE UserId = {userId};";

      return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> GetMyPosts()
    {
      string sql = @$"SELECT [PostId], 
                            [UserId], 
                            [PostTitle], 
                            [PostContent], 
                            [PostCreated], 
                            [PostUpdated]  FROM TutorialAppSchema.Posts WHERE UserId = {this.User.FindFirst("userId")?.Value};";
      return _dapper.LoadData<Post>(sql);
    }

    [HttpPost("Post")]
    public IActionResult AddPost(PostToAddDto postToAddDto)
    {
      string sql = @$"
                      INSERT INTO TutorialAppSchema.Posts( 
                      [UserId], 
                      [PostTitle], 
                      [PostContent], 
                      [PostCreated], 
                      [PostUpdated]) 
                      
                      VALUES (
                      {this.User.FindFirst("userId")?.Value},
                      '{postToAddDto.PostTitle}',
                      '{postToAddDto.PostContent}',
                      GETDATE(),
                      GETDATE()
                      );";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to add post");
    }

    [HttpPut("Post")]
    public IActionResult EditPost(PostToEditDto postToEditDto)
    {
      string sql = @$"UPDATE TutorialAppSchema.Posts
                      SET PostContent = '{postToEditDto.PostContent}', PostTitle = '{postToEditDto.PostTitle}', PostUpdated = GETDATE()
                      WHERE PostId = {postToEditDto.PostId} AND UserId = {this.User.FindFirst("userId")?.Value}";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to edit the post");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
      string sql = @$"
      DELETE FROM TutorialAppSchema.Posts WHERE PostId = {postId} AND UserId = {this.User.FindFirst("userId")?.Value};";
      if (_dapper.ExecuteSqlBool(sql))
      {
        return Ok();
      }
      throw new Exception("Failed to delete the post");
    }

    [HttpGet("PostBySearch/{searchParam}")]
    public IEnumerable<Post> GetPostsBySearch(string searchParam)
    {
      string sql = @$"SELECT * FROM TutorialAppSchema.Posts
                      WHERE PostTitle LIKE '%{searchParam}%' OR PostContent LIKE '%{searchParam}%';";
      return _dapper.LoadData<Post>(sql);
    }


    #endregion



  }
}