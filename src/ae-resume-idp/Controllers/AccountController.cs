using IdentityModel;
using Duende.IdentityServer.Services;
using aeresumeidp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using aeresumeidp.Database;

namespace aeresumeidp.Core.Controllers
{
    [AllowAnonymous]
    [Route("Identity/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IConfiguration Configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _configuration = Configuration;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberLogin, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (context != null)
                    {
                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid credentials");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
            }

            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            return Redirect(_configuration.GetValue<string>("ApplicationRedirect"));
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Role = "admin"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Errors.First().Description);
                return View(model);
            }

            //result = await _userManager.AddClaimsAsync(user, new Claim[]{
            //                new Claim(JwtClaimTypes.Email, model.Email)
            //            });

            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError(string.Empty, result.Errors.First().Description);

            //}

            return View("RegistrationSuccess");
        }
    }
}
