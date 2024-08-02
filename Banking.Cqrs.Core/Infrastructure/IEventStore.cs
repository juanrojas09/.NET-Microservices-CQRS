using Banking.Cqrs.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Cqrs.Core.Infrastructure
{
    public interface IEventStore
    {
       public Task Save(string aggregateId,IEnumerable<BaseEvent> events,int expectedVersion);
        public Task<List<BaseEvent>> GetAllEvents(string aggregateId);
    }
}
