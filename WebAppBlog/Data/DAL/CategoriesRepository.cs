using WebAppBlog.Models;

namespace WebAppBlog.Data.DAL
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly WebAppBlogContext _context;

        public CategoriesRepository(WebAppBlogContext context)
        {
            _context = context;
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        
        public Categories? GetCategoryById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public void InsertCategory(Categories category)
        {
            _context.Categories.Add(category);
            Save();
        }

        public void UpdateCategory(Categories category)
        {
            _context.Categories.Update(category);
            Save();
        }

        public void DeleteCategory(Categories category)
        {
            _context.Categories.Remove(category);
            Save();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
