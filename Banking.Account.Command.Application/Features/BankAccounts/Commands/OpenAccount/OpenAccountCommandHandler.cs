using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount
{
    public class OpenAccountCommandHandler : IRequestHandler<OpenAccountCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandler;

        public OpenAccountCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregate = new AccountAggregate(request);
                await _eventSourcingHandler.Save(aggregate);

                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

/*
 * Cuando se inyecta IEventSourcingHandler<AccountAggregate> en OpenAccountCommandHandler, 
 * el contenedor de dependencias proporciona una instancia de AccountEventSourcingHandler.
 * Por lo tanto, cuando Handle llama a _eventSourcingHandler.Save(aggregate),
 * está llamando al método Save definido en AccountEventSourcingHandler, pasando aggregate que es de tipo AccountAggregate.
 * El método Save de AccountEventSourcingHandler luego ejecuta la lógica necesaria para guardar el estado del AccountAggregate en el IEventStore.
 * 
 * Uso de la Dependencia Inyectada:

    En el constructor de OpenAccountCommandHandler,
    se inyecta IEventSourcingHandler<AccountAggregate>.
    Aunque la variable se declara como IEventSourcingHandler<AccountAggregate>
    , en tiempo de ejecución, contiene una instancia de AccountEventSourcingHandler.

 */
