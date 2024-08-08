﻿using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositFund
{
    public class DepositFundCommandHandler : IRequestHandler<DepositFundCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandler;

        public DepositFundCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(DepositFundCommand request, CancellationToken cancellationToken)
        {
         var aggregate = await _eventSourcingHandler.GetById(request.Id);
            aggregate.DepositFunds(request.Amount);
            await _eventSourcingHandler.Save(aggregate);
            return true;

        }
    }
}
