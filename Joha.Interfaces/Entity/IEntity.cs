namespace Joha.Interfaces.Entity
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
   
}
