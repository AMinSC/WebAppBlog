namespace WebAppBlog.Models;

public class Categories
{
    public int Id { get; set; }
    public required string CategoryName { get; set; }
    public ICollection<Post>? Posts { get; set; }
}
