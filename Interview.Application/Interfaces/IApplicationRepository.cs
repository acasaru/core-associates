using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Interfaces
{
    public interface IApplicationRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> SingleOrDefaultAsync<T1>(Expression<Func<T, bool>> predicate, Expression<Func<T, T1>> include, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        
    }
}
