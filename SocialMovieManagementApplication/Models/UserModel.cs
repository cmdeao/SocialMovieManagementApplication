using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/* Social Movie Management Applicaiton
 * Version 0.1
 * Cameron Deao
 * 6/25/2021
 * The UserModel establishes a model for creation when
 * users pass registration credentials into the application. Appropriate
 * requirements and length headers exist to showcase appropriate error messages.
 */

namespace SocialMovieManagementApplication.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "The First Name field is required.")]
        [StringLength(50, ErrorMessage = "First name exceeded 50 characters.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [StringLength(50, ErrorMessage = "Last name exceeded 50 characters.")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "The Email Address field is required.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Email address exceeded 100 characters.")]
        public string emailAddress { get; set; }
        [Required(ErrorMessage = "The Username field is required.")]
        [StringLength(50, ErrorMessage = "Username exceeded 50 characters.")]
        public string username { get; set; }
        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password exceeded 50 characters.")]
        public string password { get; set; }
        
    }
}