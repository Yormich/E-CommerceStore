using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace E_CommerceStore.Utilities
{
    public interface IUserClaimsManager
    {
        public abstract bool TryAddClaim(string key, string value);
        public abstract string? TryGetClaimValue(string key);

        public abstract bool TryReplaceClaim(string key, string newValue);

        public abstract IEnumerable<Claim> GetClaims();

        private bool isAuthenthicated(ClaimsPrincipal User)
        {
            return User.Identity?.IsAuthenticated ?? false;
        }
    }
}
