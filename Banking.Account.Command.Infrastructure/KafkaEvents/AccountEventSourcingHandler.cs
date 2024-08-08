using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Handlers;
using Banking.Cqrs.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Infrastructure.KafkaEvents
{
    public class AccountEventSourcingHandler : IEventSourcingHandler<AccountAggregate>
    {

        private readonly IEventStore _eventStore;

        public AccountEventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<AccountAggregate> GetById(string id)
        {
            var aggregate=new AccountAggregate();
            var events = await _eventStore.GetAllEvents(id);
            if(events.Any())
            {
                aggregate.ReplayEvents(events);
              aggregate.SetVersion(events.Max(x=>x.Version));
                
            }
            return aggregate;
        }

        public async Task Save(AggregateRoot aggregate)
        {
              await _eventStore.Save(aggregate.Id, aggregate.GetUncommittedChanges(),aggregate.version);
            aggregate.MarkChangesAsCommitted();

        }
    }
}
