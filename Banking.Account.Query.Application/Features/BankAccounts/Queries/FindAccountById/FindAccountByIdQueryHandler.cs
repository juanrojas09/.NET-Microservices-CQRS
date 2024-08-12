using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById
{
    public class FindAccountByIdQueryHandler : IRequestHandler<FindAccountByIdQuery, BankAccount>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public FindAccountByIdQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccount> Handle(FindAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.FindByAccountIdentifier(request.Identifier);
        }
    }
}
