using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olivia.Web.Models.Data
{
    [Table("User", Schema = "sheeminc_olivia")]
    public partial class User
    {
        public User()
        {
            Post = new HashSet<Post>();
        }

        [Key]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        public string Email { get; set; }

        [Required]
        public Byte IsEmailConfirmed { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
