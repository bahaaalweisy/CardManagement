using CardManagement.Core.Domain.Common;
using CardManagement.Core.Models.Pagination;
using CardManagement.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CardManagement.Infrastructure.Context
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly CardManagementDbDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(CardManagementDbDbContext dbContext)
        {
            this._dbContext = dbContext;
            _dbSet = this._dbContext.Set<T>();
        }
        public async Task<IdentityResult> DeleteAsync(T entity)
        {
            try
            {
                switch (entity)
                {
                    case null:
                        throw new ArgumentNullException(nameof(entity));
                    case ISoftDeleteEntity softDeleteEntity:
                        softDeleteEntity.IsDeleted = true;
                        _dbSet.Update(entity);
                        await _dbContext.SaveChangesAsync();
                        break;
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(ICollection<T> entities)
        {
            try
            {
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(typeof(T)))
                {
                    foreach (var entity in entities)
                    {
                        _dbContext.Entry(entity).State = EntityState.Modified;
                        var softDeleteEntity = (ISoftDeleteEntity)entity;
                        softDeleteEntity.IsDeleted = true;
                    }
                }
                else
                {
                    _dbContext.Set<T>().RemoveRange(entities);
                }
                _dbSet.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(List<Guid> ids)
        {
            var entites = _dbSet.Where(x => ids.Contains(x.Id)).AsNoTracking().ToList();
            return await this.DeleteAsync(entites);
        }

        public IQueryable<T> FindAllAsQueryable(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.Where(predicate);

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }
        public async Task<IQueryable<T>> FindAllAsQueryable2(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await Task.FromResult(query);
        }



        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);


        public async Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsQueryable();

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<ICollection<T>> GetByIdAsync(ICollection<Guid> id) => await _dbSet.Where(e => id.Contains(e.Id)).ToListAsync();

        public async Task<IdentityResult> InsertAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> InsertAsync(ICollection<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }

        public async Task<PagedList<T>> ListPagedAsync(int pageNumber, int pageSize)
        {
            return new PagedList<T>(_dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize);
        }

        public async Task<IdentityResult> UpdateAsync(T entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                entity.UpdatedOnUtc = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateAsync(ICollection<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    _dbContext.Entry(entity).State = EntityState.Modified;
                    entity.UpdatedOnUtc = DateTime.UtcNow;
                }
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "900", Description = ex.Message });
            }
        }
        public async Task<T> InsertAsync2(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Log the detailed exception here
                // Consider logging the entity state if it does not contain sensitive data
                throw new InvalidOperationException("Failed to insert entity", ex);
            }
        }
        public async Task LoadRelatedDataAsync(T entity, params Expression<Func<T, object>>[] navigationProperties)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            foreach (var navigationProperty in navigationProperties)
            {
                await _dbContext.Entry(entity).Reference(navigationProperty).LoadAsync();
            }
        }


    }


}
