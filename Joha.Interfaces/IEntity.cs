﻿namespace Joha.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
   
}
