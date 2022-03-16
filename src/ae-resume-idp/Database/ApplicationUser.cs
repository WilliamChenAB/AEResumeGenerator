using Microsoft.AspNetCore.Identity;

namespace aeresumeidp.Database
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}