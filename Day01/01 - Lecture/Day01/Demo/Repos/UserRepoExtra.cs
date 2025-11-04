using Microsoft.AspNetCore.Identity;
using ModelsLayer;

namespace Demo.Repos
{
    
    public class UserRepoExtra
    {
        UserManager<ApplicationUser> userManager;

        public UserRepoExtra(UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<bool> IsEmailExistAsync(string email, string id)
        {
            // When adding a new user (Id == 0)
            if (id == null)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (!(user != null))
                    return false;
                return true;
            }

            // When editing an existing user
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
                return false; // Email doesn't exist in DB → valid

            // Email exists but belongs to the same user being edited → valid
            if (existingUser.Id == id)
                return false;

            // Email exists and belongs to another user → invalid
            return true;
        }
    }
}
