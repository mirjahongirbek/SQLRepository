using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Joha.Interfaces
{
    public interface IGenericRepository<T, TKey, TResult> where T : class, IEntity<TKey>
    {
        T Add(T t, string logText="");
        Task<T> AddAsync(T t, string logText = "");
        int Count();
        Task<int> CountAsync();
        void Delete(T entity, string logText = "");
        Task<int> DeleteAsyn(T entity, string logText = "");
        void Dispose();
        T Find(Expression<Func<T, bool>> match, string logText = "");
        ICollection<T> FindAll(Expression<Func<T, bool>> match, string logText = "");
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, string logText = "");
        Task<T> FindAsync(Expression<Func<T, bool>> match, string logText = "");
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string logText = "");
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate, string logText = "");
        T Get(TKey id, string logText = "");
        IQueryable<T> GetAll(string logText = "");
        Task<ICollection<T>> GetAllAsyn(string logText = "");
        IQueryable<T> GetAllIncluding(string logText = "",params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id, string logText = "");
        void Save(string logText = "");
        Task<int> SaveAsync(string logText = "");
        T Update(T t, object key, string logText = "");
        Task<T> UpdateAsyn(T t, object key, string logText = "");
        List<T> GetAllFromCache(string logText = "");
        List<T> FindFromCache(Func<T, bool> func, string logText = "");
        //List<T> FindFromCache(Func<T, bool> func, DateTime date);
        Task<IEnumerable<T>> GetAllFromCacheAsync(string logText = "");
        Task<IEnumerable<T>> FindFromCacheAsync(Func<T, bool> func, string logText = "");
        Task<IEnumerable<T>> FindFromCacheAsync(Func<T, bool> func, DateTime date, string logText = "");
        TResult Filter(Func<T, bool> func, string logText = "");
        TResult Filter(Func<IQueryable<T>, IQueryable<T>> queryFilter, bool isEnabled = true, string logText = "");
        TResult Filter(Func<IQueryable<IEntity<TKey>>, IQueryable<IEntity<TKey>>> queryFilter, bool isEnabled = true, string logText = "");
        long Max(Expression<Func<T, long>> func, string logText = "");
    }
    public interface ILogger<TEntity>
    {
        int Second { get; set; }
        void Create(TEntity result,string text="");
        void Read(string text = "");
        
        void Update(TEntity result, string text = "");
        void Update(TEntity old, TEntity newEntity, string text = "");
        void Delete(TEntity result, string text = "");
        void Delete(Func<TEntity, bool> func, string text="");
        void Error(Exception exception);
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
