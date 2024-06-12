using Microsoft.AspNetCore.Mvc;
using MyPersonelBlog.Data;
using MyPersonelBlog.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;

namespace MyPersonelBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(BlogContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var blogPosts = _context.BlogPosts.ToList();
            return View(blogPosts);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Blog oluşturmak için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogPost blogPost)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Blog oluşturmak için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                blogPost.CreatedAt = DateTime.Now;
                _context.BlogPosts.Add(blogPost);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        public IActionResult Details(int id)
        {
            var blogPost = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var comments = _context.Comments.Where(c => c.BlogPostId == id).ToList();
            ViewBag.Comments = comments;

            return View(blogPost);
        }

        [HttpPost]
        public IActionResult AddComment(int blogPostId, string content)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Yorum yapmak için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var comment = new Comment
            {
                BlogPostId = blogPostId,
                Content = content,
                Author = User.Identity.Name,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = blogPostId });
        }
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Author != User.Identity.Name)
            {
                TempData["ErrorMessage"] = "Sadece kendi yorumlarınızı silebilirsiniz.";
                return RedirectToAction("Details", new { id = comment.BlogPostId });
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = comment.BlogPostId });
        }
    }
}
