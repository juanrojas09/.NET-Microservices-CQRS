using Banking.Account.Query.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Contracts.Persistence
{
    public interface IBankAccountRepository : IAsyncRepository<BankAccount>
    {
        Task<BankAccount> FindByAccountIdentifier(string accountIdentifier );

        Task<IEnumerable<BankAccount>> FindByAccountHolder(string accountHolder);

        Task<IEnumerable<BankAccount>> FindByByBalanceGreatherThan(double balance);
        Task<IEnumerable<BankAccount>> FindByByBalanceLessThan(double balance);
        Task DeleteByIdentifier(string accountIdentifier);

        Task DepositBankAccountByIdentifier(BankAccount bankAccount);
        Task WithdrawnBankAccountByIdentifier(BankAccount bankAccount);

    }
}
