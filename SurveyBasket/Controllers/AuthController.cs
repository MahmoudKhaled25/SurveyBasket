using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Authentication;
using SurveyBasket.Contracts.Authentication;
using LoginRequest = SurveyBasket.Contracts.Authentication.LoginRequest;

namespace SurveyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email,request.Password,cancellationToken);
        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return authResult.IsFailure ? authResult.ToProblem(StatusCodes.Status400BadRequest) : Ok(authResult.Value);
    }
    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
    }
}
