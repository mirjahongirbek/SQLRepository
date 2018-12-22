using Microsoft.EntityFrameworkCore;


namespace SQRRepository.Interfaces
{
    public interface IDbContext
    {
        DbContext DataContext { get;  }
    }
}
