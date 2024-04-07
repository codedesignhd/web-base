using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesign.Models.EFModel;
using Microsoft.EntityFrameworkCore;

namespace CodeDesign.Sql
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> DepartmentEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityConfiguration.CreateEntityConfig(modelBuilder);
            EntityConfiguration.SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
