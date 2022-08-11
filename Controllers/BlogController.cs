using DreamTeamBlogZ.Models;
using DreamTeamBlogZ.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using DreamTeamBlogZ.ViewModels;


namespace DreamTeamBlogZ.Controllers
{
    public class BlogController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //static int blogId = 0;

        public ActionResult Index(int? page)
        {
            var BlogList = db.Blogs.OrderBy(b => b.Title).ToList().ToPagedList(page ?? 1, 5);

            return RedirectToAction("ShowAllBlogsInList");
        }

        public ActionResult ShowBlog(int? id)
        {
            if (id == null)
            {
                id = (int)TempData["BlogId"];
                Blog blogs = db.Blogs.Find(id);
                var tmpBlogOwnerUserNames = blogs.User.UserName;
                ViewBag.temp_BlogOwnerUserName = tmpBlogOwnerUserNames;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return View(blogs);

            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            var tmpBlogOwnerUserName = blog.User.UserName;
            ViewBag.temp_BlogOwnerUserName = tmpBlogOwnerUserName;

            return View(blog);
        }

        public ActionResult ShowAllBlogsInList(int? page)
        {
            var BlogList = db.Blogs.OrderBy(b => b.Title).ToList().ToPagedList(page ?? 1, 5);

            var TotalBlogList = db.Blogs.OrderBy(b => b.Title).ToList();
            ViewBag.TotalBlogsCount = TotalBlogList.Count();

            var query = (from b in db.Blogs
                         orderby b.Title
                         select b).ToList();

            return View(BlogList);
        }

        public ActionResult ShowPostsOnBlogpage([Bind(Include = "BlogId")]int? id)
        {

            if (id == null)
            {
                id = (int)TempData["BlogId"];
                var blogs = db.Blogs.FirstOrDefault(bid => bid.Id == id);
                var PostLists = db.Posts.Where(b => b.BlogId == blogs.Id).OrderByDescending(b => b.Created).ToList();
                ViewBag.CountView = PostLists.Count;
                ViewBag.PostsOnBlogPageCounter = PostLists.Count;

                return PartialView("_ShowPostsOnBlogpage", PostLists);

            }


            var blog = db.Blogs.FirstOrDefault(bid => bid.Id == id);
            var PostList = db.Posts.Where(b => b.BlogId == blog.Id).OrderByDescending(b => b.Created).ToList();
            ViewBag.PostsOnBlogPageCounter = PostList.Count;
            ViewBag.CountView = PostList.Count;

            return PartialView("_ShowPostsOnBlogpage", PostList);
        }

        public ActionResult ShowMyBlogsInList(int? page)
        {
            var CurrentUserId = User.Identity.GetUserId();

            var BlogList = db.Blogs.Where(b => b.User.Id == CurrentUserId).OrderBy(b => b.Title).ToList();  

            var TotalBlogList = db.Blogs.OrderBy(b => b.Title).ToList();                                    
            ViewBag.TotalBlogsCount = TotalBlogList.Count();                                                
            ViewBag.MyBlogsCount = BlogList.Count();                                                        

            var query = (from b in db.Blogs
                         orderby b.Title
                         select b).ToList();

            return View(BlogList);
        }  

        [Authorize]
        public ActionResult CreateNewBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewBlog(Blog blog)
        {
            var currentUserId = User.Identity.GetUserId();

            blog.User = db.Users.FirstOrDefault(u => u.Id == currentUserId);
            blog.Created = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Remove Blog
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Remove Blog
        public ActionResult DeleteBlogConfirmed(int id)
        {
            Blog remove = db.Blogs.Find(id);
            db.Blogs.Remove(remove);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult EditBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blogs = db.Blogs.Find(id);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            return View(blogs);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditBlog([Bind(Include = "Id, Title, Body")]Blog obj)
        {
            //Variabler som definerar Andvändare BlogID och sedan hämtar dem för _IF_ satsen under.
            var currentUserId = User.Identity.GetUserId();
            var blog = db.Blogs.FirstOrDefault(b => b.Id == obj.Id);
            var bloguserId = blog.User.Id;

            //Validerar att ifall du är administrator eller blog skapare för att tillåta dig åtkomst till funktionen
            if ((bloguserId == currentUserId) || User.IsInRole("Administrator"))
            {
                //Sparar blogen I en temporär string som slängs efter
                if (TryUpdateModel(blog, "", new string[] {"Title", "Body"}))
                {
                    //Sparar stringen i db innan den slängs
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }            
            return View();
        }

        public ActionResult DetailsBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        public ActionResult SearchIndex(string searchString)
        {
            SearchIndexViewModels obj = new SearchIndexViewModels();
            List<Blog> BlogListByTitle = new List<Blog>();
            List<Blog> BlogListByUserName = new List<Blog>();
            List<Post> PostListByTitle = new List<Post>();
            List<Post> PostListByUserName = new List<Post>();
            List<Tag> TagsListByTags = new List<Tag>();

            var SearchBlog = from b in db.Blogs select b;
            var SearchUserName = from b in db.Blogs select b;
            var SearchPost = from p in db.Posts select p;
            //var SearchPostUserName = from u in db.Blogs join p in db.Posts on u.User.UserName equals p.Id select p;
            var SearchPostUserName = from p in db.Posts select p;
            var SearchTag = from t in db.Tags select t;
            
            if (searchString == null)
            {
                return RedirectToAction("Index");
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                BlogListByTitle = SearchBlog.Where(t => t.Title.Contains(searchString)).ToList();
                PostListByTitle = SearchPost.Where(p => p.Title.Contains(searchString)).ToList();
                BlogListByUserName = SearchBlog.Where(u => u.User.UserName.Contains(searchString)).ToList();
                PostListByUserName = SearchPostUserName.Where(p => p.Blog.User.UserName.Contains(searchString)).ToList();
                TagsListByTags = SearchTag.Where(t => t.Name.Contains(searchString)).ToList();

            }

            obj.blogsByTitle = BlogListByTitle;
            obj.blogsByUsers = BlogListByUserName;
            obj.postsByTitle = PostListByTitle;
            obj.postsByUser = PostListByUserName;
            obj.tagsList = TagsListByTags;
            List<Post> postList = new List<Post>();
            List<Blog> blogsList = new List<Blog>();
            List<Tag> tagsList = new List<Tag>();
            blogsList = BlogListByTitle.Concat(BlogListByTitle.Distinct()).ToList();
            postList = PostListByTitle.Concat(PostListByTitle.Distinct()).ToList();
            tagsList = TagsListByTags.Concat(TagsListByTags.Distinct()).ToList();

            @ViewBag.BlogTotalSearchCount = BlogListByTitle.Count + BlogListByUserName.Count;
            @ViewBag.PostTotalSearchCount = PostListByTitle.Count + PostListByUserName.Count;
            return View(obj);
        }

        //public ActionResult SearchIndex(string userString, string searchString)
        //{
        //    var BlogListByUser = new List<string>();

        //    var BlogQryByUser = from d in db.Blogs
        //                   orderby d.User
        //                   select d.User;

        //    BlogListByUser.AddRange(BlogListByUser.Distinct());
        //    ViewBag.userString = new SelectList(BlogListByUser);

        //    var BlogUser = from m in db.Blogs
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        BlogUser = BlogUser.Where(s => s.Title.Contains(searchString));
        //    }

        //    if (!String.IsNullOrEmpty(userString))
        //    {
        //        BlogUser = BlogUser.Where(x => x.User.UserName.Contains(userString));
        //    }

        //    return View(BlogUser);
        //}
    }
}