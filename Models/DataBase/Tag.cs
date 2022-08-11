using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamTeamBlogZ.Models.Database
{
    public class Tag
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // RELATIONS:
        public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            Posts = new List<Post>();
        }
    }
}