using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Infrastructure.Repositories
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        //al heredar el bank account desde el repository base, debo pasarle el contexto, tiene un ctor con la cadena de conexxion
        //debo inicializarlo 
        public BankAccountRepository(MySqlDbContext context) : base(context)
        {
        }

        public async Task DeleteByIdentifier(string accountIdentifier)
        {
           var result = await _context.BankAccounts!.Where(x=>x.Identifier == accountIdentifier).FirstOrDefaultAsync();
            if(result == null)
            {
                throw new Exception("no se encontro la cuenta");
            }
            _context.Set<BankAccount>().Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task DepositBankAccountByIdentifier(BankAccount bankAccount)
        {
            var result = await _context.BankAccounts!.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new Exception("no se encontro la cuenta");
            }
            result.Balance += bankAccount.Balance;
              await UpdateAsync(result);

        }

        public async Task<IEnumerable<BankAccount>> FindByAccountHolder(string accountHolder)
        {
            var result = await _context.BankAccounts.Where(x => x.AccountHolder == accountHolder).ToListAsync();
            return result;
        }

        public async Task<BankAccount> FindByAccountIdentifier(string accountIdentifier)
        {
            var result = await _context.BankAccounts.Where(x => x.Identifier == accountIdentifier).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<BankAccount>> FindByByBalanceGreatherThan(double balance)
        {
            return await _context.BankAccounts!.Where(x => x.Balance > balance).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> FindByByBalanceLessThan(double balance)
        {
            return await _context.BankAccounts.Where(x=>x.Balance < balance ).ToListAsync();
        }

        public Task WithdrawnBankAccountByIdentifier(BankAccount bankAccount)
        {
           var result=_context.BankAccounts.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefault();
            if (result == null)
            {
                throw new Exception("no se encontro la cuenta");
            }

            if(result.Balance < bankAccount.Balance)
            {
                throw new Exception("no hay suficiente saldo");
            }
            result.Balance -= bankAccount.Balance;
            return UpdateAsync(result);
        }
    }
}
