using Microsoft.AspNetCore.Authorization;

namespace ae_resume_api.Authorization
{
    public class AccessRequirement : IAuthorizationRequirement
    {
        public Access Access { get; private set; }

        public AccessRequirement(Access role)
        {
            Access = role;
        }
    }
}
