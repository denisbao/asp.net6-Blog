using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
  [HttpGet("v1/posts")]
  public async Task<IActionResult> GetAsync (
    [FromServices] BlogDataContext context,
    [FromQuery] int page = 0,
    [FromQuery] int pageSize = 10
  )
  {
    try
    {
      var countPosts = await context.Posts.AsNoTracking().CountAsync();
      var posts = await context
      .Posts
      .AsNoTracking()
      .Include(x => x.Category)
      .Include(x => x.Author)
      .Select(x => new ListPostsViewModel
      {
        Id = x.Id,
        Title = x.Title,
        Slug = x.Slug,
        LastUpdateDate = x.LastUpdateDate,
        Category = x.Category.Name,
        Author = $"{x.Author.Name} ({x.Author.Email})"
      })
      .Skip(page * pageSize)
      .Take(pageSize)
      .ToListAsync();

      return Ok(new ResultViewModel<dynamic>(new 
      {
        total = countPosts,
        page,
        pageSize,
        posts
      }));
    }
    catch (Exception ex)
    {
       return StatusCode(500, new ResultViewModel<List<Post>>("COD038: Falha interna no servidor"));
    }
  }
}