using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CardManagement.Core.Domain.Common;
using CardManagement.Core.Models.Pagination;
namespace CardManagement.Core
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<ICollection<T>> GetByIdAsync(ICollection<Guid> id);
        Task<IdentityResult> InsertAsync(T entity);
        Task<T> InsertAsync2(T entity);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);


        Task<IdentityResult> InsertAsync(ICollection<T> entities);
        Task<IdentityResult> UpdateAsync(T entity);
        Task<IdentityResult> UpdateAsync(ICollection<T> entities);
        Task<IdentityResult> DeleteAsync(T entity);
        Task<IdentityResult> DeleteAsync(ICollection<T> entities);
        Task<IdentityResult> DeleteAsync(List<Guid> ids);
        Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<PagedList<T>> ListPagedAsync(int pageNumber, int pageSize);
        IQueryable<T> FindAllAsQueryable(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task LoadRelatedDataAsync(T entity, params Expression<Func<T, object>>[] navigationProperties);
        Task<IQueryable<T>> FindAllAsQueryable2(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);


    }

}
