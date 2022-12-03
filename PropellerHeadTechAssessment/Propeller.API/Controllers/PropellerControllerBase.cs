using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Propeller.API.Controllers
{
    public class PropellerControllerBase : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //internal bool ValidateAdmin() // TODO: SHould this be protected?
        //{
        //    Claim? profileClaim = User.Claims.FirstOrDefault(x => x.Type == Constants.ProfileClaim);

        //    if (profileClaim == null)
        //    {
        //        return false;
        //    }

        //    if (!Enum.TryParse<UserProfile>(profileClaim.Value, out UserProfile cprof))
        //    {
        //        return false;
        //    }

        //    if (cprof != UserProfile.Admin)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
