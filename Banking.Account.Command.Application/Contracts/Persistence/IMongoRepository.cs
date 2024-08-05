using Banking.Account.Command.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Contracts.Persistence
{
    public interface IMongoRepository<TDocument> where TDocument :IDocument
    {
        Task<TDocument> InsertDocument(TDocument document);
        Task<TDocument> Get(string id);
        Task<IEnumerable<TDocument>> GetAll();
        Task<bool> UpdateDocument(TDocument document);
        Task<bool> DeleteById(string id);
    }
}
