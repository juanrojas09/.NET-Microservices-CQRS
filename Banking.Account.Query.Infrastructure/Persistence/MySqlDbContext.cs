using Banking.Account.Query.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Infrastructure.Persistence
{
    public class MySqlDbContext:DbContext
    {

        public MySqlDbContext(DbContextOptions<MySqlDbContext>options):base(options)
        {
            
        }
        public DbSet<BankAccount>? BankAccounts { get; set; }


     
    }
}
