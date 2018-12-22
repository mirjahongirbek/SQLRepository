using Joha.Interfaces;
using SQRRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;


namespace SQRRepository
{

    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey, BaseQueryFilter> where TEntity : class, IEntity<TKey>


    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
        ILogger<TEntity> _logger;
        public GenericRepository(IDbContext context)
        {
            _context = context.DataContext;
            _dbSet = context.DataContext.Set<TEntity>();
        }
        public GenericRepository(IDbContext context, ILogger<TEntity> loggger, int second):this(context)
        {
            _logger = loggger;
            _logger.Second = second;
        }
        #region Add

        public TEntity Add(TEntity t)
        {
            try
            {
                _context.Set<TEntity>().Add(t);
                _context.SaveChanges();
                _logger?.Create(t);
                return t;
            }catch(Exception exeption)
            {
                _logger?.Error(exeption, t,MethodType.Create);
                return null;
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity t)
        {
            try
            {
                _context.Set<TEntity>().Add(t);
                await _context.SaveChangesAsync();
                _logger?.Create(t);
                return t;
            }
            catch(Exception exeption)
            {
                _logger.Error(exeption, t, MethodType.Create);
                return null;
            }
           
        }
        #endregion
        #region Count
        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }
        #endregion
        #region Delete
        public virtual void Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                _logger?.Delete(entity);
            }
            catch(Exception exeption)
            {
                _logger?.Error(exeption, entity, MethodType.Delete);
                
            }
          
        }
        public virtual async Task<int> DeleteAsyn(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _logger?.Delete(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception exeption)
            {
                _logger?.Error(exeption, entity, MethodType.Delete);
                return 0;
            }

            
        }
        public virtual void Delete(Func<TEntity, bool> func)
        {
            try
            {
                _dbSet.Where(func).AsQueryable().Delete();
                _logger?.Delete(func);
            }catch(Exception exption)
            {
                _logger?.Error(exption, null, MethodType.Delete, "116-Line Delete Func Exeption  116-Line");
            }
            
        }
        #endregion
        #region Find
        public virtual TEntity Find(Expression<Func<TEntity, bool>> match)
        {
            return _context.Set<TEntity>().SingleOrDefault(match);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(match);
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
        {
            return _context.Set<TEntity>().Where(match).ToList();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _context.Set<TEntity>().Where(match).ToListAsync();
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<TEntity>> FindByAsyn(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        #endregion
        #region Get         
        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsyn()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        #endregion
        #region Update
        public virtual TEntity Update(TEntity t, object key)
        {
            try
            {
                if (t == null)
                    return null;
                TEntity exist = _context.Set<TEntity>().Find(key);
                if (exist != null)
                {
                    _context.Entry(exist).CurrentValues.SetValues(t);
                    _context.SaveChanges();
                }
                _logger?.Update(t, "");
                return exist;
            }
            catch(Exception exeption)
            {
                _logger?.Error(exeption, t, MethodType.Update);
                return null;
            }
           
        }

        public virtual async Task<TEntity> UpdateAsyn(TEntity t, object key)
        {
            try
            {
                if (t == null)
                    return null;
                TEntity exist = await _context.Set<TEntity>().FindAsync(key);
                if (exist != null)
                {
                    _context.Entry(exist).CurrentValues.SetValues(t);
                    await _context.SaveChangesAsync();
                }
                _logger?.Update(t, "");
                return exist;
            }
            catch(Exception exeption)
            {
                _logger.Error(exeption, t, MethodType.Update);
                return null;
            }
            
        }
        #endregion
        #region Save
        public virtual void Save()
        {

            _context.SaveChanges();
        }
        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        #endregion
        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #region News
        public virtual List<TEntity> GetAllFromCache() {
            return _dbSet.FromCache().ToList();
        }
        public List<TEntity> FindFromCache(Func<TEntity, bool> func)
        {
            return _dbSet.Where(func).AsQueryable().FromCache().ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllFromCacheAsync()
        {
            return await _dbSet.FromCacheAsync();
        }
        public async Task<IEnumerable<TEntity>> FindFromCacheAsync(Func<TEntity, bool> func)
        {
            return await _dbSet.Where(func).AsQueryable().FromCacheAsync();
        }
        public async Task<IEnumerable<TEntity>> FindFromCacheAsync(Func<TEntity, bool> func, DateTime date)
        {
            return null;
            //return _dbSet.Where(func).AsQueryable().FromCache(date).;
        }
        public BaseQueryFilter Filter(Func<TEntity, bool> func)
        {
            return _context.Filter<TEntity>(m => m.Where(func).AsQueryable());
        }
        public BaseQueryFilter Filter(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool isEnabled = true)
        {
            return _context.Filter(queryFilter, isEnabled);
        }

        public BaseQueryFilter Filter(Func<IQueryable<IEntity<TKey>>, IQueryable<IEntity<TKey>>> queryFilter, bool isEnabled = true)
        {
            return _context.Filter(queryFilter, isEnabled);
        }
        public long Max(Expression<Func<TEntity, long>> func)
        {
            return _dbSet.DeferredMax(func).FutureValue<long>().Value;
        }


        #endregion

    }
    
}
