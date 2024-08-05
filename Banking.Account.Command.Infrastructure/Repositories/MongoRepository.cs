using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Domain.Common;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Banking.Account.Command.Infrastructure.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;
        public MongoRepository(IOptions<MongoSettings> options)
        {
           var client= new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.Database);
            _collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault()).CollectionName;
       
        }
        public async Task<bool> DeleteById(string id)
        {
            try
            {
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
                var result= await _collection.FindOneAndDeleteAsync(filter);
                return true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<TDocument> Get(string id)
        {
            try{ 
                var filter=Builders<TDocument>.Filter.Eq(x => x.Id, id);
                return _collection.Find(filter).FirstOrDefaultAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            try
            {
                return await _collection.Find(p=>true).ToListAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TDocument> InsertDocument(TDocument document)
        {
            try
            {
                 await _collection.InsertOneAsync(document);
                return document;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateDocument(TDocument document)
        {
            try
            {
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);
                _collection.FindOneAndReplace(filter,document);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
