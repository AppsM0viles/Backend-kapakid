using MediatR;
using Microsoft.AspNetCore.Mvc;
using FinTrackBack.Wallet.Application.Features.PaymentCards;
using FinTrackBack.Wallet.Application.Features.TransportCards;

namespace FinTrackBack.Wallet.Interfaces.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly ISender _mediator;

    public WalletController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("payment-cards")]
    public async Task<IActionResult> CreatePaymentCard([FromBody] CreatePaymentCardCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }

    [HttpGet("payment-cards/user/{userId:guid}")]
    public async Task<IActionResult> GetPaymentCards(Guid userId)
    {
        var result = await _mediator.Send(new GetPaymentCardsQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPut("payment-cards/update-balance")]
    public async Task<IActionResult> UpdatePaymentBalance([FromBody] UpdatePaymentBalanceCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }

    [HttpPut("payment-cards/pay-debt")]
    public async Task<IActionResult> PayDebt([FromBody] PayDebtCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return BadRequest(new { success = false, message = "Saldo insuficiente o tarjeta no encontrada" });
        return Ok(new { success = true });
    }

    [HttpPost("transport-cards")]
    public async Task<IActionResult> CreateTransportCard([FromBody] CreateTransportCardCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }

    [HttpGet("transport-cards/user/{userId:guid}")]
    public async Task<IActionResult> GetTransportCards(Guid userId)
    {
        var result = await _mediator.Send(new GetTransportCardsQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPut("transport-cards/update-balance")]
    public async Task<IActionResult> UpdateTransportBalance([FromBody] UpdateTransportBalanceCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return Ok(new { success = true });
    }
}