using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByHolder;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountWithBalance;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAccounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Account.Query.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingAccountQueryController : ControllerBase
    {

        private readonly IMediator mediator;

        public BankingAccountQueryController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("account/holder/{holder}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> FindAccountByHolder(string holder)
        {
           var FindAccountQuery= new FindAccountByHolderQuery { AccountHolder = holder };
            var response = await mediator.Send(FindAccountQuery);
            return Ok(response);
        }


        [HttpGet("account/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> FindAccountById(string id)
        {
            var FindAccountQuery = new FindAccountByIdQuery { Identifier = id };
            var response = await mediator.Send(FindAccountQuery);
            return Ok(response);
        }


        [HttpGet("account/{balance}/{equalityType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> FindAccountWithBalance(double balance, string equalityType)
        {
            var FindAccountQuery = new FindAccountWithBalanceQuery { Balance = balance, EqualityType = equalityType };
            var response = await mediator.Send(FindAccountQuery);
            return Ok(response);
        }


        [HttpGet("accounts/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllAccounts()
        {
            var FindAccountQuery = new FindAllAccountsQuery();
            var response = await mediator.Send(FindAccountQuery);
            return Ok(response);
        }



    }
}
