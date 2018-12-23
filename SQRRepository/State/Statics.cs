using System;
using System.Collections.Generic;
using System.Linq;
using Joha.Interfaces.Entity;
using Joha.Interfaces.Enums;

namespace SQLRepository.State
{
public static class Statics
    {
        public static Dictionary<string, Dictionary<MethodType, UInt64>> Stat = new Dictionary<string, Dictionary<MethodType, ulong>>();
        public static Dictionary<MethodType, UInt64> GetState<TResult, TKey>(this TResult entity)
            where TResult:class, IEntity<TKey>
        {
            var name = typeof(TResult).Name;
            return Stat.FirstOrDefault(m => m.Key == name).Value;                
        }
    }
}
