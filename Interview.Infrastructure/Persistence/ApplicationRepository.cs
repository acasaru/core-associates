using Ardalis.Specification;
using Interview.Application.Core.Entitities;
using Interview.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Infrastructure.Persistence
{
    public class ApplicationRepository<T> : IApplicationRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };

            return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync<T1>(Expression<Func<T, bool>> predicate, Expression<Func<T, T1>> include,  CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().Include(include)
                .SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

       
    }
}
