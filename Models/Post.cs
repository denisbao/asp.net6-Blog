namespace NewBlog.Models
{
  public class Post
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
    public string Slug { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

    // Relationships 1-N:
    public Category Category { get; set; }
    public User Author { get; set; }

    // Relationship N-N:
    public List<Tag> Tags { get; set; }
  }
}