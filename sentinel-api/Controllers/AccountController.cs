using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sentinel_api.Models;

namespace sentinel_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
                        SignInManager<User> signInManager, 
                        UserManager<User> userManager, 
                        RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var result = await signInManager
                .PasswordSignInAsync(login.Email, 
                                     login.Password, 
                                     login.RememberMe, 
                                     lockoutOnFailure: false);
            return Ok(result);
        }
    }
}
