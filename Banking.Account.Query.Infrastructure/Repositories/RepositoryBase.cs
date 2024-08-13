using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain.Common;
using Banking.Account.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseDomainModel
    {
        public readonly MySqlDbContext _context;

        public RepositoryBase(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            //Set<T> referencio esa entidad generica
            _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //por si queres usar unit of work
        public void AddEntity(T entity)
        {
           _context.Set<T>().Add(entity);

        }

        public async Task<T> DeleteAsync(T entity)
        {
            var result =  _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);   
          
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
           return  await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
           await _context.SaveChangesAsync();
            return entity;
        }

        public void UpdateEntity(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
