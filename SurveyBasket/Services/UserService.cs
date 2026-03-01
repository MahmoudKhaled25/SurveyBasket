using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users.Where(x => x.Id == userId)
                                            .ProjectToType<UserProfileResponse>()
                                            .SingleAsync();
        return Result.Success(user);
    }
}
