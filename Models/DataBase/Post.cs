using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamTeamBlogZ.Models.Database
{
    public class Post
    {

        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Title Name")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        public DateTime Created { get; set; }
        public int Views { get; set; }
        
        // RELATIONS:
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }


    }
}