using Application.Dto.Profile;
using Utility.Core;

namespace Application.Modules.Profile
{
    public interface IProfileValidation
    {
        /// <summary>
        /// Get the details of the user's profile
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        MethodResult<UserProfileDto> ProfileDetails(string userName);

        /// <summary>
        /// Update the user's profile
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        MethodResult<bool> Update(UserProfileDto model, string loggedInUser);
    }
}