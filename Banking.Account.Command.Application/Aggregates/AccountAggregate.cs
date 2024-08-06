using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Aggregates
{
    public class AccountAggregate : AggregateRoot
    {
       
        
        public bool Active { get; set; }
        public double Balance { get; set; }

   
        public AccountAggregate()
        {
            
        }
        //ctor q se encarga de la inicializacion de un evento de tipo cuenta bancaria
        //creariamos un evento que representa la apertura de una cuenta bancaria
        public AccountAggregate(OpenAccountCommand openAccountCommand)
        {
            //este evento sera el que se encargue de la apertura de la cuenta bancaria 
            //se guardara en la db y se enviara a kafka
            var accountOpenedEvent = new AccountOpenedEvent(openAccountCommand.AccountHolder,
                openAccountCommand.AccountType, DateTime.Now
                ,openAccountCommand.OpeningBalance,
                openAccountCommand.Id);

            RaiseEvent(accountOpenedEvent);

        }


        public void Apply(AccountOpenedEvent @event)
        {
            Id = @event.Id;
            Active = true;
            Balance = @event.OpeningBalance;
        }


        //dispara el evento de deposito de fondos
        public void DepositFunds(double amount )
        {
            if (!Active)
            {
                throw new InvalidOperationException("Account is not active");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Deposit amount must be greater than 0");
            }

            var depositFundsEvent = new FundsDepositedEvent( amount,Id);
            RaiseEvent(depositFundsEvent);
        }

        public void Apply(FundsDepositedEvent @event)
        {
            Id = @event.Id;
            Balance += @event.Amount;
        }


        public void WithDrawFunds(double amount )
        {
            if (!Active)
            {
                throw new InvalidOperationException("Account is not active");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Withdraw amount must be greater than 0");
            }

            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            var withdrawnFundsEvent = new FundsWithdrawnEvent(amount, Id);
            RaiseEvent(withdrawnFundsEvent);
        }

        public void Apply(FundsWithdrawnEvent @event)
        {
            Id = @event.Id;
            Balance -= @event.Amount;
        }


        public void CloseAccount()
        {
            if (!Active)
            {
                throw new InvalidOperationException("Account is not active");
            }

            var accountClosedEvent = new AccountClosedEvent(Id);
            RaiseEvent(accountClosedEvent);
        }

        public void Apply(AccountClosedEvent @event)
        {
            Id = @event.Id;
            Active = false;
        }
    }
}
