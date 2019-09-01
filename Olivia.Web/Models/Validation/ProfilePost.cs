using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Olivia.Web.Models.Validation
{
    public class ProfilePost
    {
        [Required(ErrorMessage = "Shared_Required")]
        [MinLength(6, ErrorMessage = "Shared_MinLength")]
        [MaxLength(320, ErrorMessage = "Shared_MaxLength")]
        public String Content { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}