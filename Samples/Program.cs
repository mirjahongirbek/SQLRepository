using Joha.Interfaces;
using SQRRepository;
using SQRRepository.Interfaces;
using System;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
        }
    }
    public class SomeEntity : IEntity<int>
    {
      public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Generic : GenericRepository<SomeEntity, int>
    {
        public Generic(IDbContext context) : base(context)
        {
           var s= Filter(m => m.Id == 1);
           
        }
    }
}
