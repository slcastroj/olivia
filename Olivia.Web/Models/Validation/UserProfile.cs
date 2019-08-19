using System.Collections.Generic;
using System.Linq;
using Olivia.Web.Models.Data;

namespace Olivia.Web.Models.Validation
{
    public class UserProfile
    {
        public User User { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public ProfilePost PostForm { get; set; }
    }
}