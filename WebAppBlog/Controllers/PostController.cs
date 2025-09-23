using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppBlog.Data;
using WebAppBlog.Data.DAL;
using WebAppBlog.Models;
using WebAppBlog.ViewModels;

namespace WebAppBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public PostController(IPostRepository postRepository, ICategoriesRepository categoriesRepository)
        {
            _postRepository = postRepository;
            _categoriesRepository = categoriesRepository;
        }

        public IActionResult Index()
        {
            return View(_postRepository.GetAllPosts());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.GetPostById(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            var htmlContent = Markdown.ToHtml(post.Content ?? "**There is no content.**");

            var viewModel = new PostDetailViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = htmlContent,
                UrlSlug = post.UrlSlug,
                CategoryId = post.CategoryId,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new PostEditViewModel
            {
                Categories = new SelectList(_categoriesRepository.GetAllCategories().ToList(), "Id", "Name")
            };  
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostEditViewModel viewModel)
        {
            if (viewModel.CategoryId == null && string.IsNullOrWhiteSpace(viewModel.NewCategoryName))
            {
                ModelState.AddModelError("", "기존 카테고리를 선택하거나 새 카테고리 이름을 입력해야 합니다.");
            }

            if (ModelState.IsValid)
            {
                int categoryId;

                if (!string.IsNullOrWhiteSpace(Request.Form["NewCategoryName"]))
                {
                    var newCategory = new Categories
                    {
                        CategoryName = Request.Form["NewCategoryName"]!
                    };
                    _categoriesRepository.InsertCategory(newCategory);
                    _categoriesRepository.Save();
                    categoryId = newCategory.Id;
                }
                else if (int.TryParse(Request.Form["CategoryId"], out int selectedCategoryId))
                {
                    categoryId = selectedCategoryId;
                }
                else
                {
                    ModelState.AddModelError("CategoryId", "Please select a category or enter a new category name.");
                    return View(viewModel);
                }

                Post post = new Post()
                {
                    Title = viewModel.Title,
                    Content = viewModel.Content,
                    UrlSlug = viewModel.UrlSlug,
                    CategoryId = categoryId,
                    Category = _categoriesRepository.GetCategoryById(categoryId),
                    CreatedAt = DateTime.UtcNow
                };

                _postRepository.InsertPost(post);
                _postRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            viewModel.Categories = new SelectList(_categoriesRepository.GetAllCategories().ToList(), "Id", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Content,UrlSlug,CreatedAt,UpdatedAt")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.UpdatedAt = DateTime.UtcNow;
                    _postRepository.UpdatePost(post);
                    _postRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _postRepository.GetPostById(id);
            if (post != null)
            {
                _postRepository.DeletePost(post);
            }

            _postRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
