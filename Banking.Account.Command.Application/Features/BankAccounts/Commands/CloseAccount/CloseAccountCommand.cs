using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount
{
    public class CloseAccountCommand:IRequest<bool>
    {
        public string id { get; set; } = string.Empty;
    }
}
