namespace WebAppBlog.Models;

public class Categories
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Post> Posts { get; set; }
}
