using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Olivia.Web.Models;
using Olivia.Web.Models.Data;

namespace Olivia.Web.Controllers
{
    public class UserController : Controller
    {
        private SignInManager<User> Users { get; }

        public UserController(SignInManager<User> users)
        {
            Users = users;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Profile"); }
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(User model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var rs = await Users.PasswordSignInAsync(
                model.Username,
                model.Password,
                true,
                false
            );

            if (!rs.Succeeded)
            {
                ViewBag.ErrorMessage = "Nombre de usuario o contraseña incorrectos";
                return View();
            }

            return RedirectToAction("Profile");
        }

        [HttpGet("/Profile")]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); }
            var u = await Users.UserManager.GetUserAsync(HttpContext.User);
            return View(u);
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await Users.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}