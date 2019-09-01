using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olivia.Web.Models.Data
{
    [Table("Post", Schema = "sheeminc_olivia")]
    public partial class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(32)]
        public string Username { get; set; }
        [Url]
        public string ImageUrl { get; set; }

        [ForeignKey("Username")]
        public virtual User User { get; set; }
    }
}
