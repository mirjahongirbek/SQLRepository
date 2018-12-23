using Microsoft.EntityFrameworkCore;


namespace SQLRepository.Interfaces
{
    public interface IDbContext
    {
        DbContext DataContext { get;  }
    }
}