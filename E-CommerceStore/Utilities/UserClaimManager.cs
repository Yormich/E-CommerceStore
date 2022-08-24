using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace E_CommerceStore.Utilities
{
    public class UserClaimManager : IUserClaimsManager
    {
        private readonly ClaimsIdentity? identity;
        public UserClaimManager(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;
            identity = context?.User.Identity as ClaimsIdentity;
            Console.WriteLine("Successfully injected transient claimsManager!)");
        }

        public IEnumerable<Claim> GetClaims()
        {
            return identity?.Claims ?? Enumerable.Empty<Claim>();
        }

        public bool TryAddClaim(string type, string value)
        {
            if (identity == null)
                return false;

            Claim claimToAdd = new Claim(type, value);

            if (identity.HasClaim(c=> c.Type == type))
                return false;

            identity.AddClaim(claimToAdd);
            return true;
        }



        public string? TryGetClaimValue(string type)
        {
            if (identity == null)
                return null;

            foreach(Claim claim in identity.Claims)
            {
                if(claim.Type == type)
                {
                    return claim.Value;
                }
            }

            return null;
        }

        public bool TryReplaceClaim(string type, string newValue)
        {
            if (identity == null)
                return false;

            foreach(Claim claim in identity.Claims)
            {
                if(claim.Type == type)
                {
                    Claim replacement = new Claim(type, newValue);
                    identity.RemoveClaim(claim);
                    identity.AddClaim(replacement);
                    return true;
                }
            }

            return false;
        }
    }
}
