using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.WithdrawnFund
{
    public class WithdrawnFundCommandHandler:IRequestHandler<WithdrawFundsCommands,bool>
    {

        private IEventSourcingHandler<AccountAggregate> eventSourcingHandler;
        public WithdrawnFundCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
                {
                    this.eventSourcingHandler = eventSourcingHandler;
                }

        public async Task<bool> Handle(WithdrawFundsCommands request, CancellationToken cancellationToken)
        {
            var aggregate = await eventSourcingHandler.GetById(request.Id);
            aggregate.WithDrawFunds(request.Amount);
            await eventSourcingHandler.Save(aggregate);
            return true;


        }

        
    }
}
