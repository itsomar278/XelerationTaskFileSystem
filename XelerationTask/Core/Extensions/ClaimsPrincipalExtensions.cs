using System.Security.Claims;
using XelerationTask.Core.Exceptions;

namespace XelerationTask.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(value, out var id)
                ? id
                : throw new NotAuthorized("Try to login again");
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            return !string.IsNullOrEmpty(email)
                ? email
                : throw new NotAuthorized("Try to login again");
        }
    }
}
