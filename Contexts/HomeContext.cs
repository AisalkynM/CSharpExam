using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Contexts
{
    public class HomeContext:DbContext
    {
        public HomeContext(DbContextOptions options): base (options){}

        public DbSet<User> Users {get;set;}
        public DbSet <Banana> Bananas {get; set;}
        public DbSet <Participation> Participations {get; set;}

    }
}