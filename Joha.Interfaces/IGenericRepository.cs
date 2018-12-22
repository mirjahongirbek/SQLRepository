using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Joha.Interfaces
{
    public interface IGenericRepository<T, TKey, TResult> where T : class, IEntity<TKey>
    {
        T Add(T t);
        Task<T> AddAsync(T t);
        int Count();
        Task<int> CountAsync();
        void Delete(T entity);
        Task<int> DeleteAsyn(T entity);
        void Dispose();
        T Find(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        T Get(int id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsyn();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id);
        void Save();
        Task<int> SaveAsync();
        T Update(T t, object key);
        Task<T> UpdateAsyn(T t, object key);
        List<T> GetAllFromCache();
        List<T> FindFromCache(Func<T, bool> func);
        //List<T> FindFromCache(Func<T, bool> func, DateTime date);
        Task<IEnumerable<T>> GetAllFromCacheAsync();
        Task<IEnumerable<T>> FindFromCacheAsync(Func<T, bool> func);
        Task<IEnumerable<T>> FindFromCacheAsync(Func<T, bool> func, DateTime date);
        TResult Filter(Func<T, bool> func);
        TResult Filter(Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true);
        TResult Filter(Func<IQueryable<IEntity<TKey>>, IQueryable<IEntity<TKey>>> queryFilter, bool isEnabled = true);
        long Max(Expression<Func<T, long>> func);
    }
    public interface ILogger<TEntity>
    {
        int Second { get; set; }
        void Create(TEntity result,string text="");
        void Update(TEntity result, string text = "");
        void Delete(TEntity result, string text = "");
        void Delete(Func<TEntity, bool> func, string text="");
        void Error(Exception exception, TEntity result, MethodType type, string text="");
    }
    public enum MethodType
    {
        Create,
        Read,
        Update,
        Delete,

    }
}
