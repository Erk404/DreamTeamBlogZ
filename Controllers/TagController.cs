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
    public class TagController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tag
        public ActionResult Index(Tag tag, int? postId)
        {
            var random = from t in db.Tags select t;
            //var ar = db.Tags.Where(t => t.Posts.Where(x=> x.Id == postId);
            return View();
        }

        // GET: Tags/Create
        static int postId = 0;
        public ActionResult AddTag([Bind(Include = "Id")] Post post)
        {
            postId = post.Id;

            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTag([Bind(Include = "Id,Name")] Tag tag)
        {
            var post = db.Posts.First(p => p.Id == postId);

            if (tag == null)
            {
                return RedirectToAction("ListMyPosts", "Posts");
            }
            else
            {
                var tagExists = db.Tags.FirstOrDefault(t => t.Name == tag.Name);

                if (tagExists == null)
                {
                    if (ModelState.IsValid)
                    {
                        tag.Posts.Add(post);
                        //db.Tags.Add(tag);
                        //db.SaveChanges();
                        //tagId = tag.Id;
                        //var newTag = db.Tags.FirstOrDefault(nt => nt.Id == tagId);
                        //newTag.Posts.Add(post);
                        db.Tags.Add(tag);
                        db.SaveChanges();
                    }
                }
                else
                {
                    tagExists.Posts.Add(post);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ListMyPosts", "Posts");
        }


    }
}

        //public ActionResult CreateTag(Post post)
        //{
        //    ViewBag.postId = post.Id;
            
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateTag(Tag tag, int postId)
        //{
        //    Post post = db.Posts.Find(postId);
        //    string[] collectionOfTags = null;
        //    if (tag.Name == null)
        //    {
        //        collectionOfTags = tag.Name.Split(',');

        //    }

        //    foreach (var item in collectionOfTags)
        //    {
        //        Tag tags = db.Tags.FirstOrDefault(c => c.Name == item);
        //        if (tags != null)
        //        {
        //            post.Tags.Add(tags);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //           Tag newtags = new Tag() { Name = item };
        //            db.Tags.Add(newtags);
        //            //post.Tags.Add(newtags);
        //            newtags.Posts.Add(post);
        //            db.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction("ShowBlog", "Blog", new { id = post.BlogId });
        //}