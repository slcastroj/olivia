using System;
using System.ComponentModel.DataAnnotations;

namespace Olivia.Web.Models.Validation
{
    public class UserLogin
    {
        public LoginSignIn SignInForm { get; set; }
        public LoginSignUp SignUpForm { get; set; }
    }
}