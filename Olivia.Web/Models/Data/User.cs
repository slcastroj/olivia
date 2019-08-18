using System;
using System.Collections.Generic;

namespace Olivia.Web.Models.Data
{
    public partial class User
    {
        public User()
        {
            Post = new HashSet<Post>();
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
