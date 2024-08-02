using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Cqrs.Core.Events
{
    public class AccountOpenedEvent : BaseEvent
    {
        public string AccountHolder { get; set; }=String.Empty;
        public string AccountType { get; set; }= String.Empty;
        public DateTime CreatedDate { get; set; }
        public double OpeningBalance { get; set; }

         
        public AccountOpenedEvent(string accountHolder, string accountType, DateTime createdDate, double openingBalance,string id) : base(id)
        {
            AccountType = accountType;
            CreatedDate = createdDate;
            OpeningBalance = openingBalance;
            AccountHolder = accountHolder;
        }

    }
}
