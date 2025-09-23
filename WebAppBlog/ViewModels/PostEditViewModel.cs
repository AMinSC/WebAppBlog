using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebAppBlog.ViewModels
{
    public class PostEditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string UrlSlug { get; set; } = string.Empty;
        [Display(Name = "기존 카테고리 선택")]
        public int? CategoryId { get; set; }
        [ValidateNever]
        public SelectList? Categories { get; set; }
        [Display(Name = "새 카테고리 이름")]
        public string? NewCategoryName { get; set; }
    }
}
