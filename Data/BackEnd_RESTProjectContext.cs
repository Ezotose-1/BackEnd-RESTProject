using Microsoft.EntityFrameworkCore;
using BackEnd_RESTProject.Models;

namespace BackEnd_RESTProject.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<User> User {get; set;}
    }
}