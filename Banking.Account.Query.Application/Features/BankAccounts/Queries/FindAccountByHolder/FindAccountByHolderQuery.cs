using Banking.Account.Query.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByHolder
{
    public class FindAccountByHolderQuery : IRequest<IEnumerable<BankAccount>>
    {
        //lo que me pasa el cliente
        public string AccountHolder { get; set; } = string.Empty;

   
    }
}
