using Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositFund;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.WithdrawnFund;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Banking.Account.Command.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BankAccountOperationsAccountController : ControllerBase
    {

        private IMediator _mediator;

        public BankAccountOperationsAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("openAccount", Name="OpenAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> OpenAccount([FromBody] OpenAccountCommand command)
    {
            var id=Guid.NewGuid().ToString();
            command.Id = id;
            var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("deposit/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DepositFund(string id,[FromBody] DepositFundCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("closeAccount/{id}", Name ="CloseAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CloseAccount(string id)
        {
            var command = new CloseAccountCommand { id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("WithdrawnFund/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> WithdrawFund(string id, [FromBody] WithdrawFundsCommands command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
