using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebSoftMast_02.Controller
{

    [ApiController]
    [Route("/api/account")]
    public class ApiAccountController : ControllerBase
    {
        private SignInManager<IdentityUser> signinManager;

        private UserManager<IdentityUser> userManager;


        public ApiAccountController(SignInManager<IdentityUser> mgr, UserManager<IdentityUser> userManager)
        {
            signinManager = mgr;

            this.userManager = userManager;
        }


        [Authorize]
        [HttpGet("list")]
        public List<DataUsers> Get_users()
        {
            var ls = new List<DataUsers>();

            foreach (var user in userManager.Users)
            {
                ls.Add(
                    new DataUsers { Id = user.Id, Username = user.UserName, 
                    Email = user.Email});

            }

            return ls; 

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials creds)
        {
            Microsoft.AspNetCore.Identity.SignInResult result
                = await signinManager.PasswordSignInAsync(creds.Username,
                    creds.Password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return Ok();
        }


        public class Credentials
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }

        public class DataUsers
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }

        }

    }
}
