using System;
using System.Collections.Generic;

namespace Olivia.Web.Models.Data
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        public virtual User UsernameNavigation { get; set; }
    }
}
