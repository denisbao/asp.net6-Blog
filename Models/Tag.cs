namespace NewBlog.Models
{
  public class Tag
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    // Relationship 1-N:
    public IList<Post> Posts { get; set; }
  }
}