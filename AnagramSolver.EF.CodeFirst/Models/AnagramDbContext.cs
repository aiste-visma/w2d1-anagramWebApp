using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst.Models
{
    public class AnagramDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AnagramSolver_CF;Trusted_Connection=True;TrustServerCertificate=True;");

    }
}
