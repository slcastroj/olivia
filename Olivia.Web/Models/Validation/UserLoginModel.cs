using System;
using System.ComponentModel.DataAnnotations;

namespace Olivia.Web.Models.Validation
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Shared_Required")]
        [Display(Name = "Nombre de usuario")]
        [MinLength(6, ErrorMessage = "Shared_MinLength")]
        [MaxLength(32, ErrorMessage = "Shared_MaxLength")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Shared_Required")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "Shared_MinLength")]
        [MaxLength(64, ErrorMessage = "Shared_MaxLength")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}