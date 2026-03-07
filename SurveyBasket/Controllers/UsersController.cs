using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstractions.Consts;
using SurveyBasket.Authentication.Filters;
using System.Threading;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) => Ok(await _userService.GetAllUsersAsync(cancellationToken));




}
