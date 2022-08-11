using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamTeamBlogZ.Models.Database;
using DreamTeamBlogZ.Models;
using System.ComponentModel.DataAnnotations;

namespace DreamTeamBlogZ.ViewModels
{
    public class BlogIndexViewModels
    {
        public Blog Blogs { get; set; }
        public List<Blog> blogsList { get; set; }
        public Post post { get; set; }
        public List<Post> postsList { get; set; }
    }

    public class PostIndexViewModels
    {
        public Post posts { get; set; }
        public Comment comments { get; set; }
        public List<Comment> commentsList { get; set; }
        public Tag tag { get; set; }
        public List<Tag> tagsList { get; set; }

    }

    public class SearchIndexViewModels
    {
        public List<Blog> blogsByTitle { get; set; }
        public List<Blog> blogsByUsers { get; set; }
        public List<Blog> blogsList { get; set; }
        public List<Post> postsByTitle { get; set; }
        public List<Post> postsByUser { get; set; }
        public List<Post> postsList { get; set; }
        public List<Tag> tagsList { get; set; }
    }

    public class EditUserViewModels
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public List<String> Users { get; set; }
        public List<String> Roles { get; set; }

    }    

}