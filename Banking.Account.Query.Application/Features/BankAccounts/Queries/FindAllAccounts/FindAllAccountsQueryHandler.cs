using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAccounts
{
    public class FindAllAccountsQueryHandler : IRequestHandler<FindAllAccountsQuery, IEnumerable<BankAccount>>
    {

        private readonly IBankAccountRepository _repository;

        public FindAllAccountsQueryHandler(IBankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BankAccount>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
        {
           return await _repository.GetAllAsync();
        }
    }
}
