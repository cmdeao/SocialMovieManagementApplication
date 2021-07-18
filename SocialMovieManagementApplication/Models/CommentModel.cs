using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Models
{
    public class CommentModel
    {
        public int commentID { get; set; }
        public int postID { get; set; }
        public string commentText { get; set; }
        public int postedBy { get; set; }
        public string postedUsername { get; set; }

        public DateTime postedDate { get; set; }
    }
}