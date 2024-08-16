using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount
{
    public class OpenAccountCommand : IRequest<bool>
    {
        public string? Id { get; set; }
        public string AccountHolder { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;

        public double OpeningBalance { get; set; }  

    }
}
