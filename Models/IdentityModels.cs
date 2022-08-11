using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using DreamTeamBlogZ.Models.Database;
using System;

namespace DreamTeamBlogZ.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }                                          // Extra
        public string LastName { get; set; }                                           // Extra
        public DateTime MemberSince { get; set; }                                      // Extra

        public virtual ICollection<Blog> Blogs { get; set; }                           // Extra
        public virtual ICollection<Comment> Comments { get; set; }                     // Extra

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var dbContext = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(dbContext);
            var userManager = new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "Kalle",
                Email = "KalleAnka@Ankeborg.se"
            };
            
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            var roleStore = new RoleStore<IdentityRole>(dbContext);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            roleManager.Create(new IdentityRole("Administrator"));
            ApplicationUser dbUser = userManager.FindByEmail("KalleAnka@Ankeborg.se");
            userManager.AddToRole(dbUser.Id, "Administrator");
            // Add custom user claims here
            return userIdentity;
            
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Blog> Blogs { get; set; }                                        // Extra
        public DbSet<Post> Posts { get; set; }                                        // Extra
        public DbSet<Comment> Comments { get; set; }                                  // Extra
        public DbSet<Tag> Tags { get; set; }                                          // Extra

        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}