using FinTrackBack.Authentication.Application.Features.Authentication.Login;
using FinTrackBack.Authentication.Application.Features.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackBack.Authentication.Interfaces.REST.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly ISender _mediator; 

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")] 
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        var newUserId = await _mediator.Send(command);
        return CreatedAtAction(null, new { id = newUserId });
    }
    
    [HttpPost("login")] 
    public async Task<IActionResult> LoginUser([FromBody] LoginUserQuery query)
    {
        try
        {
            var response = await _mediator.Send(query);
            return Ok(response); 
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message }); 
        }
    }
}