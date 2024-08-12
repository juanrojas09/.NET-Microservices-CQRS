using Banking.Account.Query.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Domain
{
    public class BankAccount : BaseDomainModel
    {
        public string Identifier { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public DateTime creationDate { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public double Balance { get; set; }
    }
}
