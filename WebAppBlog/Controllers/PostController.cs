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

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Content,UrlSlug,CreatedAt,UpdatedAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.UtcNow;
                _postRepository.InsertPost(post);
                _postRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
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
