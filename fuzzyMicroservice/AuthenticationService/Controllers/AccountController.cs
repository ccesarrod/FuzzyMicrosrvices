using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.DataCore;
using DataCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(IOptions<AppSettings> appSettings, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
           
            _appSettings = appSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;           
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            var theUser = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, lockoutOnFailure: false);


            if (theUser.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.UserName);

                return Ok(new
                {
                    email = user.Email,
                    userName = user.UserName,
                   
                });
            }

            return NotFound();
        }
        // POST: api/Account
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
