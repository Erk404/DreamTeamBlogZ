using DreamTeamBlogZ.Models;
using DreamTeamBlogZ.Models.Database;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using DreamTeamBlogZ.ViewModels;



namespace DreamTeamBlogZ.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index(string searchResult)
        {
            var checkifsearchresulthasavaluie = searchResult;
            // gets Asp.Net Framework  logged in ApplicationUser ID and puts in in currentuserId variable
            var currentuserId = User.Identity.GetUserId();
            // finds Users in the database and and matches the current logged in user with that user
            var user = db.Users.FirstOrDefault(u => u.Id == currentuserId);
            if (User.IsInRole("Administrator") || User.IsInRole("User"))
            {
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListUsers(string searchUserString)
        {
            //What ever value you put in the input field on the View gets sent here
            if (!String.IsNullOrEmpty(searchUserString))
            {
                //Create a new List of ASP.Net Framework ApplicationUser
                List<ApplicationUser> searchResult = new List<ApplicationUser>();
                //Selects all user in the database
                var searchUser = from u in db.Users select u;
                //Puts all users in the database into a list
                var showUser = db.Users.ToList();
                //If the value you typed is similar to a Username in the database it gets added to a list here
                searchResult = searchUser.Where(su => su.UserName.Contains(searchUserString)).ToList();
                //Counts the amount of users it has found with the keyword in your value
                ViewBag.UsersCount = searchResult.Count;

                return PartialView("_ListUsersPartial", searchResult);    

            }
            //Finds all the Users in the database but only keeps 10
            var showUsers = db.Users.Take(10).ToList();
            //Finds all the Users in the database and adds them to a list
            var UsersListCount = db.Users.ToList();
            //Counts how many Users it has found
            ViewBag.UsersCount = UsersListCount.Count;

            //Returns the list of users and sends them to the Partial View for Viewing
            return PartialView("_ListUsersPartial", showUsers);
        }

        public ActionResult createNewRoles(string roleName)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            return PartialView();
        }

        public ActionResult addRoleToUser()
        {
            var roles = db.Roles.Select(rnm => rnm.Name).ToList();
            var users = db.Users.Select(u => u.UserName).ToList();
            EditUserViewModels usrRole = new EditUserViewModels();

            usrRole.Roles = roles;
            usrRole.Users = users;

            return PartialView("_addRoleToUser", usrRole);
        }

        [HttpPost]
        public ActionResult addRoleToUser(string User, string Role)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var thisUser = userManager.FindByName(User);

            if (thisUser != null)
            {
                userManager.AddToRole(thisUser.Id, Role);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult editUser()
        {
            var userId = User.Identity.GetUserId();
            var currentUserId = db.Users.FirstOrDefault(uid => uid.Id == userId);

            //var user = db.Users.Find(userId);

            //var userStore = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var currentuserId = User.Identity.GetUserId();
            if (currentUserId.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(currentUserId);
        }

        [HttpPost]
        public ActionResult editUser([Bind(Include ="Id, UserName, LastName, Firstname, PhoneNumber, Email")]ApplicationUser user)
        {
            //var userStore = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var currentUserId = userStore.Users.FirstOrDefault(uid => uid.Id == userId);

            var currentuserId = User.Identity.GetUserId();
            var matchUser = db.Users.FirstOrDefault(uid => uid.Id == currentuserId);
            matchUser.Email = user.Email;
            matchUser.FirstName = user.FirstName;
            matchUser.LastName = user.LastName;
            matchUser.PhoneNumber = user.PhoneNumber;
            matchUser.UserName = user.UserName;
            

            if (ModelState.IsValid)
            {
                db.Entry(matchUser).State = EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(matchUser);
        }

        public ActionResult DeleteUser(string id)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            //finds the user in the database and compares in to the incoming string of userid
            var deleteUser = userManager.FindById(id);
            
            if (ModelState.IsValid)
            {
                //deletes user from the database
                userManager.Delete(deleteUser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}    



//public PartialViewResult SearchUserResult(string searchUserString)
        //{
        //    //Create a new List of ASP.Net Framework ApplicationUser
        //    List<ApplicationUser> searchResult = new List<ApplicationUser>();

        //    //Selects all user in the database
        //    var searchUser = from u in db.Users select u;
        //    //Puts all users in the database into a list
        //    var showUser = db.Users.ToList();

        //    if (searchUserString == null)
        //    {
        //        ViewBag.UsersCount = showUser.Count;
        //        return PartialView("_ListUsersPartial", showUser);
        //    }
        //    //What ever value you put in the input field on the View gets sent here
        //    else if (!String.IsNullOrEmpty(searchUserString))
        //    {
        //        //If the value you typed is similar to a Username in the database it gets added to a list here
        //        searchResult = searchUser.Where(su => su.UserName.Contains(searchUserString)).ToList();
        //        //Counts the amount of users it has found with the keyword in your value
        //        ViewBag.UsersCount = searchResult.Count;
        //    }

        //    //Sends a list of the keyword value Username into a list
        //    return PartialView("_ListUsersPartial", searchUserString);
        //}