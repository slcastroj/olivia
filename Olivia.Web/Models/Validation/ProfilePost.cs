using System;
using System.ComponentModel.DataAnnotations;

namespace Olivia.Web.Models.Validation
{
    public class ProfilePost
    {
        [Required(ErrorMessage = "Shared_Required")]
        [MinLength(6, ErrorMessage = "Shared_MinLength")]
        [MaxLength(320, ErrorMessage = "Shared_MaxLength")]
        public String Content { get; set; }
    }
}