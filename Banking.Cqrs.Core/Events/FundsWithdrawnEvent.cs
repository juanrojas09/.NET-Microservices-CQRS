﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Cqrs.Core.Events
{
    public  class FundsWithdrawnEvent : BaseEvent
    {
        public double Amount { get; set; }
        public FundsWithdrawnEvent(double amount, string id) : base(id)
        {
            Amount = amount;
        }
  
    }
}
