using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMovieManagementApplication.Models
{
    public class FriendModel
    {
        public int friendID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
    }
}