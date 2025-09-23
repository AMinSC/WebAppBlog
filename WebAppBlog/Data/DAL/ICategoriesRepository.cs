using WebAppBlog.Models;

namespace WebAppBlog.Data.DAL
{
    public interface ICategoriesRepository
    {
        IEnumerable<Categories> GetAllCategories();
        Categories? GetCategoryById(int categoryId);
        void InsertCategory(Categories category);
        void UpdateCategory(Categories category);
        void DeleteCategory(Categories category);
        void Save();
    }
}
