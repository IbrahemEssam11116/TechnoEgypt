using System.Security.Claims;
using System.Security.Principal;

namespace TechnoEgypt
{
    public static class UserHelper
    {
        public static string GetUserId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            return (identity as ClaimsIdentity)?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}
