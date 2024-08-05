using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Infrastructure.Repositories
{
    public class EventStoreRepository:MongoRepository<EventModel>, IEventStoreRepository
    {
        protected readonly IMongoCollection<EventModel> _collection;

        //el i options es el encargado de crear la cadena de conexion hacia el mongo db
        public EventStoreRepository(IOptions<MongoSettings>options):base(options) 
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.Database);
            _collection = database.GetCollection<EventModel>(typeof(EventModel).Name);
        }

        public async Task<IEnumerable<EventModel>> GetEventsByAggregateId(string aggregateId)
        {
            try
            {
                var filter=Builders<EventModel>.Filter.Eq(x => x.AggregateIdentifier, aggregateId);
                return await _collection.Find(filter).ToListAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
   
}
