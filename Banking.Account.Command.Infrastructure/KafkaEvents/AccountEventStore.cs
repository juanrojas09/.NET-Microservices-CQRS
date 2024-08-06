using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Domain;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Infrastructure;
using Banking.Cqrs.Core.Producers;

namespace Banking.Account.Command.Infrastructure.KafkaEvents
{
    public class AccountEventStore : IEventStore
    {
        private readonly IEventStoreRepository eventStoreRepository;
        private readonly IEventProducer eventProducer;
        public AccountEventStore(IEventProducer eventProducer, IEventStoreRepository eventStoreRepository)
        {
            this.eventProducer = eventProducer;
            this.eventStoreRepository = eventStoreRepository;
        }
        public async Task<List<BaseEvent>> GetAllEvents(string aggregateId)
        {
            try
            {
                var  eventStreams=await eventStoreRepository.GetEventsByAggregateId(aggregateId);
                if(eventStreams == null || eventStreams.Count() == 0)
                {
                    throw new Exception("La cuenta bancaria no existe");
                }

                return eventStreams.Select(x => x.EventData).ToList();
            }
            catch(Exception ex )
            {
                throw ex;
            }
        }

        /// <summary>
        /// Este metodo se encarga de guardar los eventos en la base de datos y enviarlos a kafka
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="events"></param>
        /// <param name="expectedVersion"></param>
        /// <returns></returns>
        public async Task Save(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            try
            {
                //obtener los eventos de la base de datos, busco todos los eventos por el id del aggregate
                var eventStream =await eventStoreRepository.GetEventsByAggregateId(aggregateId);
                //valido si el evento que estoy guardando es el mismo que esta en la base de datos
                if (expectedVersion!=-1 && eventStream.ElementAt(eventStream.Count()-1).version != expectedVersion)
                {
                    throw new Exception("Concurrency Exception");
                }

                //asigno la version a los eventos
                var version = expectedVersion;
                //recorro los eventos que me llegan y los guardo en la base de datos y los envio a kafka
                foreach (var @event in events)
                {
                    version++;
                    @event.Version = version;
                    var eventModel = new EventModel()
                    {
                        Timestamp = DateTime.Now,
                        AggregateIdentifier = aggregateId,
                        EventType = @event.GetType().Name,
                        version = version,
                        EventData = @event,
                        AggregateType = nameof(AccountAggregate)

                    };
                  await  eventStoreRepository.InsertDocument(eventModel);

                    //enviar a kafka
                    eventProducer.Produce(@event.GetType().Name,@event);
                }

            }
            catch(Exception ex )
            {
                throw ex;
            }
        }
    }
}
