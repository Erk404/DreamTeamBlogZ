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
    public class CommentController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //Gets all the comments from the database and orders them by date then sorts them into a list
            var CommentsList = db.Comments.OrderByDescending(b => b.Created).ToList();
            //Counts the amount of items in the CommentList
            ViewBag.TotalCommentsCount = CommentsList.Count();                                              

            //Returns the CommentList to a view for actuall Viewpage
            return View(CommentsList);
        }


        public ActionResult ShowCommentsInPost(int postId)
        {
            if (postId == null)
            {
                postId = (int)TempData["PostId"];
                //id = (int)TempData["PostId"];
                return RedirectToAction("Index");

            }

            //Searches the Posts Database and finds the first posts with the same ID as the incoming postId
            var cmntInPost = db.Posts.FirstOrDefault(pid => pid.Id == postId);
            //Searches the Comments Database and Collects all the comments related to the Declared postId and sorts them into dates then into a List
            var CommentList = db.Comments.Where(b => b.PostId == cmntInPost.Id).OrderByDescending(b => b.Created).ToList();

            //Sends the list to a partial view with the CommentList
            return PartialView("_ShowCommentsInPostPartial", CommentList);
        }

        [Authorize]
        public ActionResult CreateNewComment(string createcomment, int postId, int BlogId)
        {
            var comPostId = db.Posts.FirstOrDefault(i => i.Id == postId);
            var currenUserId = User.Identity.GetUserId();
            var author = db.Users.FirstOrDefault(u => u.Id == currenUserId);
            var currentBlogId = db.Posts.Find(BlogId);

            Comment comment = new Comment()
            {
                Author = author,
                Created = DateTime.Now,
                Post = comPostId,
                Body = createcomment

            };
            if (author == null)
            {
                comment.Author = null;
            }
            else
            {
                comment.Author = author;
            }

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["postId"] = comment.PostId;
                TempData["BlogId"] = BlogId;
                return RedirectToAction("ShowBlog", "Blog", BlogId);

            }
            ViewBag.PostId = postId;

            return RedirectToAction("ShowBlog", "Blog");
        }

        //[HttpPost]
        //public ActionResult CreateNewComment(Comment comment,int postId, string BlogId)
        //{
        //    var comPostId = db.Posts.FirstOrDefault(i => i.Id == postId);
        //    var currenUserId = User.Identity.GetUserId();
        //    var author = db.Users.FirstOrDefault(u => u.Id == currenUserId);

        //    comment.Created = DateTime.Now;
        //    comment.PostId = postId;
        //    //blogId = Convert.ToInt32(BlogId);
        //    //(int)TempData["BlogId"] = Convert.ToInt32(BlogId);

        //    if (author == null)
        //    {
        //        comment.Author = null;
        //    }
        //    else
        //    {
        //        comment.Author = author;
        //    }


        //    db.Comments.Add(comment);
        //    db.SaveChanges();

        //    string temp1 = "ShowBlog/";
        //    string temp2 = BlogId;
        //    string temp12 = temp1 + temp2;


        //    return RedirectToAction(temp12, "Blog");
        //    //return RedirectToAction("Index");
        //}

        //[Authorize]
        //public ActionResult CreateNewComment(Post post, Blog blog, int postId)
        //{

        //    //post.Id = postId;
        //    //blogId = blog.Id;
        //    blog.Id = post.BlogId;
        //    post.Id = postId;
        //    //ViewBag.tmpPostTitle = db.Posts.Where(x => x.Id == postId).ToList();
        //    ViewBag.PostTitle = post.Title;
        //    ViewBag.PostId = post.Id;
        //    ViewBag.BlogId = post.BlogId;

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateNewComment(int? postId, string body)
        //{
        //    var comPostId = db.Posts.FirstOrDefault(i => i.Id == postId);
        //    var currenUserId = User.Identity.GetUserId();
        //    var author = db.Users.FirstOrDefault(u => u.Id == currenUserId);
        //    //postId = post.Id;

        //    Comment comment = new Comment()
        //    {
        //        Author = author,
        //        Created = DateTime.Now,
        //        Post = comPostId,
        //        Body = body

        //    };

        //    //ViewBag.BlogId = post.BlogId;
        //    //ViewBag.PostId = post.Id;
        //    //ViewBag.PostTitle = post.Title;


        //    //if (author == null)
        //    //{
        //    //    comment.Author = null;
        //    //}
        //    //else
        //    //{
        //    //    comment.Author = author;
        //    //}


        //    db.Comments.Add(comment);
        //    db.SaveChanges();
        //    //TempData["PostId"] = comment.PostId;

        //    //string temp1 = "ShowBlog/";
        //    //string temp2 = BlogId;
        //    //string temp12 = temp1 + temp2;


        //    return RedirectToAction("ShowBlog", "Blog");
        //    //return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public ActionResult CreateNewComment(Comment comment)
        //{

        //    comment.Created = DateTime.Now;
        //    comment.PostId = postId;
        //    var currentUserId = User.Identity.GetUserId();
        //    var author = db.Users.FirstOrDefault(u => u.Id == currentUserId);

        //    if (author == null)
        //    {
        //        comment.Author = null;
        //    }
        //    else
        //    {
        //        comment.Author = author;
        //    }

        //    db.Comments.Add(comment);
        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}
    }
}