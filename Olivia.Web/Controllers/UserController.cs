using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Olivia.Web.Models;
using Olivia.Web.Models.Data;
using Olivia.Web.Models.Identity;
using Olivia.Web.Models.Validation;

namespace Olivia.Web.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> Manager { get; }
        private SignInManager<User> Users { get; }
        private OliviaContext Database { get; }
        private IEmailSender EmailSender { get; }
        private IHostingEnvironment Environment { get; }

        public UserController(SignInManager<User> users, UserManager<User> manager, OliviaContext database, IEmailSender sender, IHostingEnvironment env)
        {
            Users = users;
            Database = database;
            Manager = manager;
            EmailSender = sender;
            Environment = env;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Profile"); }
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var rs = await Users.PasswordSignInAsync(
                login.SignInForm.Username,
                login.SignInForm.Password,
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

        [Authorize]
        [HttpGet("/Profile")]
        public async Task<IActionResult> Profile()
        {
            var u = await Users.UserManager.GetUserAsync(HttpContext.User);
            var posts = Database.Post.Where(x => x.Username == u.Username);
            var up = new UserProfile { User = u, Posts = posts };
            return View(up);
        }

        [HttpGet("/Profile/{username}")]
        public async Task<IActionResult> PublicProfile(string username)
        {
            var u = await Database.User.FindAsync(username);
            if (u.Username.ToLowerInvariant() == User.Identity.Name.ToLowerInvariant())
            {
                return RedirectToAction("Profile");
            }
            var posts = Database.Post.Where(x => x.Username == u.Username);
            var up = new UserProfile { User = u, Posts = posts };
            return View(up);
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await Users.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost("/Post")]
        public async Task<IActionResult> Post(UserProfile up)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var file = up.PostForm.File;
            var guid = Guid.Empty;
            var ext = String.Empty;
            if (up.PostForm.File != null)
            {
                ext = Path.GetExtension(file.FileName);
                var dir = Path.Combine(Environment.WebRootPath, "user-data", User.Identity.Name);
                if (!Directory.Exists(dir))
                {
                    var di = Directory.CreateDirectory(dir);
                    if (file.Length > 0)
                    {
                        var filename = String.Empty;
                        do
                        {
                            guid = Guid.NewGuid();
                            filename = Path.Combine(dir, guid.ToString() + ext);
                        } while (System.IO.File.Exists(filename));
                        var path = Path.Combine(dir, filename);
                        using (var stream = new FileStream(filename, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
            }

            var u = await Users.UserManager.GetUserAsync(HttpContext.User);
            var p = new Post
            {
                Content = up.PostForm.Content,
                Username = u.Username,
                Date = DateTime.Now,
                ImageUrl = up.PostForm.File is null ? "" : Path.Combine("~", "user-data", u.Username, guid + ext)
            };

            var rs = await Database.Post.AddAsync(p);
            if (rs is null)
            {
                return BadRequest();
            }
            await Database.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        [Authorize]
        [HttpGet("/Post/Delete/{id}")]
        public async Task<IActionResult> DeletePost(Int32 id)
        {
            var u = await Users.UserManager.GetUserAsync(HttpContext.User);
            var p = await Database.Post.Where(x => x.Username == u.Username && x.Id == id).FirstOrDefaultAsync();
            if (p is null) { return Unauthorized(); }

            var rs = Database.Post.Remove(p);
            if (rs is null) { return BadRequest(); }
            await Database.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        [HttpPost("/SignUp")]
        public async Task<IActionResult> SignUp(UserLogin login)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = new User
            {
                Username = login.SignUpForm.Username,
                Email = login.SignUpForm.Email
            };

            var rs = await Manager.CreateAsync(user, login.SignUpForm.Password);
            if (rs.Succeeded)
            {
                var token = await Manager.GenerateEmailConfirmationTokenAsync(user);
                var bytes = Encoding.UTF8.GetBytes(token);
                var enc = WebEncoders.Base64UrlEncode(bytes);
                var url = Url.Action(
                    "VerifyEmail",
                    "User",
                    new { username = user.Username, token = enc },
                    Request.Scheme);

                await EmailSender.SendEmailAsync(
                    user.Email,
                    "Confirm your email",
                    $"Confirm your email by clicking <a href='{HtmlEncoder.Default.Encode(url)}'>here</a>"
                );
            }

            return RedirectToAction("Login");
        }

        [HttpGet("/User/Verify/{username}/{token}")]
        public async Task<IActionResult> VerifyEmail(String username, String token)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var bytes = WebEncoders.Base64UrlDecode(token);
            var dec = Encoding.UTF8.GetString(bytes);

            var user = await Database.User.FindAsync(username);
            var rs = await Manager.ConfirmEmailAsync(user, dec);
            if (!rs.Succeeded) { return Unauthorized(); }

            await Users.SignInAsync(user, true);

            return RedirectToAction("Profile");
        }
    }
}