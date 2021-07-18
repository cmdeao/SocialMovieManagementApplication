using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Models
{
    public class SocialPostModel
    {
        public int postID { get; set; }
        public int postedBy { get; set; }
        public string postedUsername { get; set; }
        public string postContent { get; set; }
        public DateTime datePosted { get; set; }
    }
}