using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementRepository.Entity;

namespace UserManagementRepository
{
    public class UserDBContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Configure entity relationships, constraints, etc.
            base.OnModelCreating(modelBuilder);
        }
    }
}
