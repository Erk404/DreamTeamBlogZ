using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 

/// <summary>
/// Models Database
/// </summary>
namespace DreamTeamBlogZ.Models.Database
{
    public class Comment
    {

        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }

        [StringLength(1000)]
        public string Body { get; set; }

        public DateTime Created { get; set; }

        public virtual ApplicationUser Author { get; set; }

        // RELATIONS:
        public virtual Post Post { get; set; }

    }
}