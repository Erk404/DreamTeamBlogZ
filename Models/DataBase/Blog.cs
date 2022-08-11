using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;




namespace DreamTeamBlogZ.Models.Database
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Title Name")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        public string ImgUrl { get; set; }

        public DateTime Created { get; set; }

        //public virtual string UserID { get; set; }

        // RELATIONS:
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}