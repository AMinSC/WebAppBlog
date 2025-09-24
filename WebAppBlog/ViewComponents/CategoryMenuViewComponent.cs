using Microsoft.AspNetCore.Mvc;
using WebAppBlog.Data;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly WebAppBlogContext _context;

    public CategoryMenuViewComponent(WebAppBlogContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }
}