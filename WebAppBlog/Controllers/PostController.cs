using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppBlog.Data;
using WebAppBlog.Models;
using WebAppBlog.ViewModels;

namespace WebAppBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly WebAppBlogContext _context;

        public PostController(WebAppBlogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _context.Post
                .Include(p => p.Category)
                .FirstOrDefault(m => m.Id == id);
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
                Categories = new SelectList(_context.Categories, "Id", "CategoryName")
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
                    _context.Categories.Add(newCategory);
                    _context.SaveChanges();
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
                    Category = _context.Categories.Find(categoryId),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Post.Add(post);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            viewModel.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _context.Post.Find(id);
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
                    _context.Update(post);
                    _context.SaveChanges();
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

            var post = _context.Post
                .Include(p => p.Category)
                .FirstOrDefault(m => m.Id == id);
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
            var post = _context.Post.Find(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
