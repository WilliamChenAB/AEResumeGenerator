using aeresumeidp.Database;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace aeresumeidp
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject;
            if (subject == null) throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null) throw new ArgumentException("Invalid subject identifier");

            //context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, user.Role));

            var claims = await _userManager.GetClaimsAsync(user);
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var subject = context.Subject;
            if (subject == null) throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }

    }
}