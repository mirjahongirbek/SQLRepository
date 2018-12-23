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
      protected  DbContext _context;
      protected  DbSet<TEntity> _dbSet;
       protected ILogger<TEntity> _logger;
        bool _state;
        public GenericRepository(IDbContext context)
        {
            _context = context.DataContext;
            _dbSet = context.DataContext.Set<TEntity>();
        }
        public GenericRepository(IDbContext context, bool state) : this(context)
        {
            _state = state;
            var name=typeof(TEntity).Name;
           var statics= State.Statics.Stat.FirstOrDefault(m=>m.Key== name);
            if (statics.Key == null)
            {
                State.Statics.Stat.Add(name, new Dictionary<MethodType, UInt64>() { { MethodType.Create,0},{ MethodType.Delete,0}, { MethodType.Read,0},{ MethodType.Update,0} });
            }
        }
        public GenericRepository(IDbContext context, ILogger<TEntity> loggger, int second, bool state=false):this(context, state)
        {
            _logger = loggger;
            _logger.Second = second;
        }
        #region
        private void StateAdd() { }
        private void StateUpdate() { }
        private void StateDelete() { }
        private void StateRead() { }
        private void StateError(TEntity entity, Exception exception) { }
        private void StateError() { }
        private void StateError(Exception exption) { }
        private void StateError(Exception exption, TKey id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Add

        public TEntity Add(TEntity t, string logText = "")
        {
            try
            {
                _context.Set<TEntity>().Add(t);
                _context.SaveChanges();
                _logger?.Create(t);
                StateAdd();
                return t;
            }catch(Exception exeption)
            {
                StateError(t, exeption);
                _logger?.Error(exeption, t,MethodType.Create);
                return null;
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity t, string logText = "")
        {
            try
            {
                _context.Set<TEntity>().Add(t);
                await _context.SaveChangesAsync();
                _logger?.Create(t);
                StateAdd();
                return t;
            }
            catch(Exception exeption)
            {
                _logger.Error(exeption, t, MethodType.Create);
                StateError(t, exeption);
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
        public virtual void Delete(TEntity entity, string logText = "")
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                _logger?.Delete(entity);
                StateDelete();
            }
            catch(Exception exeption)
            {
                _logger?.Error(exeption, entity, MethodType.Delete);
                StateError(entity, exeption);
            }
          
        }
        public virtual async Task<int> DeleteAsyn(TEntity entity, string logText = "")
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _logger?.Delete(entity);
                StateDelete();
                return await _context.SaveChangesAsync();
            }
            catch (Exception exeption)
            {
                StateError(entity, exeption);
                _logger?.Error(exeption, entity, MethodType.Delete);
                return 0;
            }
                        
        }
        public virtual void Delete(Func<TEntity, bool> func, string logText = "")
        {
            try
            {
                _dbSet.Where(func).AsQueryable().Delete();
                _logger?.Delete(func);
                StateDelete();
            }catch(Exception exption)
            {
                StateError(null, exption);
                _logger?.Error(exption, null, MethodType.Delete, "116-Line Delete Func Exeption  116-Line");
            }
            
        }
        #endregion
        #region Find
        public virtual TEntity Find(Expression<Func<TEntity, bool>> match, string loggText="")
        {
            try
            {
                StateRead();
                return _context.Set<TEntity>().SingleOrDefault(match);
            }
            catch(Exception exeption)
            {
                StateError();
                _logger?.Error(null, null,MethodType.Read, loggText);
                return null;
            }
            
           
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match, string logtext="")
        {
            try
            {

                StateRead();
                return await _context.Set<TEntity>().SingleOrDefaultAsync(match);
            }
            catch(Exception exeption)
            {
                StateError();
                _logger?.Error(null, null, MethodType.Read, logtext = "");
                return null;
            }
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match, string logText="")
        {
            try
            {
                StateRead();
                return _context.Set<TEntity>().Where(match).ToList();
            }
            catch(Exception exeption)
            {
                StateError(exeption);
                _logger.Error(null, null, MethodType.Read, logText);
                return null;
            }
           
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match, string logtext)
        {
            try
            {
                StateRead();
                return await _context.Set<TEntity>().Where(match).ToListAsync();
            }
            catch(Exception expetion)
            {
                StateError(expetion);
                _logger.Error(null,null, MethodType.Read,logtext);
                return null;
                
            }          
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string logText = "")
        {
            try
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
                StateRead();
                return query;
            } catch (Exception exeption)
            {
                StateError(exeption);
                return null;
            }
            
        }

        public virtual async Task<ICollection<TEntity>> FindByAsyn(Expression<Func<TEntity, bool>> predicate, string logText = "")
        {
            try
            {
                StateRead();
                return await _context.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception exeption)
            {
                _logger.Error(exeption);
                StateError(exeption);
                return null;
            }
            
        }
        #endregion
        #region Get         
        public TEntity Get(TKey id, string logText="")
        {
            try
            {
               var result= _context.Set<TEntity>().Find(id);
                StateRead();
                return result;
            }
            catch(Exception exception)
            {
                StateError(exception, id);
                _logger?.Error(exception,null,MethodType.Read, logText);
                return null;
            }            
        }       

        public IQueryable<TEntity> GetAll(string logText="")
        {
            try
            {
                var result=_context.Set<TEntity>();
                StateRead();
                _logger?.Read(logText);
                return result;
            }
            catch(Exception exception)
            {
                _logger?.Error(exception);
                return null;
            }
           
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsyn(string logText="")
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch(Exception exception)
            {

                return null;
            }
            
        }

        public IQueryable<TEntity> GetAllIncluding(string logText = "",params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        public virtual async Task<TEntity> GetAsync(int id, string logText = "")
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        #endregion
        #region Update
        public virtual TEntity Update(TEntity t, object key, string logText = "")
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

        public virtual async Task<TEntity> UpdateAsyn(TEntity t, object key, string logText = "")
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
        public virtual void Save(string logText = "")
        {

            _context.SaveChanges();
        }
        public async virtual Task<int> SaveAsync(string logText = "")
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
        public virtual List<TEntity> GetAllFromCache(string logText) {
            return _dbSet.FromCache().ToList();
        }
        public List<TEntity> FindFromCache(Func<TEntity, bool> func,string logText = "")
        {
            return _dbSet.Where(func).AsQueryable().FromCache().ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllFromCacheAsync(string logText = "")
        {
            return await _dbSet.FromCacheAsync();
        }
        public async Task<IEnumerable<TEntity>> FindFromCacheAsync(Func<TEntity, bool> func, string logText = "")
        {
            return await _dbSet.Where(func).AsQueryable().FromCacheAsync();
        }
        public async Task<IEnumerable<TEntity>> FindFromCacheAsync(Func<TEntity, bool> func, DateTime date, string logText = "")
        {
            return null;
            //return _dbSet.Where(func).AsQueryable().FromCache(date).;
        }
        public BaseQueryFilter Filter(Func<TEntity, bool> func, string logtext)
        {
            return _context.Filter<TEntity>(m => m.Where(func).AsQueryable());
        }
        public BaseQueryFilter Filter(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool isEnabled = true, string logText = "")
        {
            return _context.Filter(queryFilter, isEnabled);
        }

        public BaseQueryFilter Filter(Func<IQueryable<IEntity<TKey>>, IQueryable<IEntity<TKey>>> queryFilter, bool isEnabled = true, string logText = "")
        {
            return _context.Filter(queryFilter, isEnabled);
        }
        public long Max(Expression<Func<TEntity, long>> func, string logText = "")
        {
            return _dbSet.DeferredMax(func).FutureValue<long>().Value;
        }


        #endregion

    }
    
}
