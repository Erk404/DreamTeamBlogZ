using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamTeamBlogZ.Models;
using Microsoft.AspNet.Identity;


namespace DreamTeamBlogZ.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }

        public ActionResult ShowBlogsOnFrontpage()
        {
            var BlogList = db.Blogs.OrderByDescending(b => b.Created).ToList().Take(8);

            var query = (from b in db.Blogs
                         orderby b.Title
                         select b).ToList();

            return View(BlogList);
        }

        public ActionResult ShowPostsOnFrontpage()
        {
            var PostList = db.Posts.OrderByDescending(b => b.Created).ToList().Take(10);

            return View(PostList);
        }
    }
}