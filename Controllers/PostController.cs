using DreamTeamBlogZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DreamTeamBlogZ.Models.Database;


namespace DreamTeamBlogZ.Controllers
{
    public class PostController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        public ActionResult Index()
        {
            //var PostList = db.Posts.OrderByDescending(b => b.Created).ToList().ToPagedList(page ?? 1, 5);
            var PostList = db.Posts.OrderByDescending(b => b.Created).ToList();
            ViewBag.TotalPostsCount = PostList.Count();

            return View(PostList);
        }

        //static int blogId = 0;


        [Authorize]
        public ActionResult CreateNewPost(Blog blog)
        {
            int blogId = blog.Id;
            var cBlog = db.Blogs.FirstOrDefault(b => b.Id == blogId);

            ViewBag.BlogTitle = cBlog.Title;
            ViewBag.BlogId = cBlog.Id;

            return View();
        }

        [HttpPost]
        public ActionResult CreateNewPost(Post post, int BlogId)
        {
            post.Created = DateTime.Now;
            post.Views = 0;
            post.BlogId = BlogId;

            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
            }
            TempData["BlogId"] = BlogId;
            return RedirectToAction("ShowBlog", "Blog");
            //return RedirectToAction("Index");

        }
    }
}