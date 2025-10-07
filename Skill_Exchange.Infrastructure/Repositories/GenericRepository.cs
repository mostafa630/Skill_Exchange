using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure.Peresistence;

namespace Skill_Exchange.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected DbSet<T> _dbSet;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
            //_dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> spec)
        {
            if (spec is null)
                return await GetAllAsync();

            var query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
