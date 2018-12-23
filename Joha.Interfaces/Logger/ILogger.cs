using System;
using Joha.Interfaces.Enums;

namespace Joha.Interfaces.Logger
{
    public interface ILogger<TEntity>
    {
        int Second { get; set; }
        void Create(TEntity result,string text="");
        void Read(string text = "");
        void Update(TEntity result, string text = "");
        void Update(TEntity old, TEntity newEntity, string text = "");
        void Delete(TEntity result, string text = "");
        void Delete(Func<TEntity, bool> func, string text="");
        void Error(Exception exception, string text = "");
        void Error(Exception exception);
        void Error(Exception exception, TEntity result, MethodType type, string text="");
    }
}
