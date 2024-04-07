using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesign.Models.EFModel;
using Microsoft.EntityFrameworkCore;

namespace CodeDesign.Sql
{
    public static class EntityConfiguration
    {
        public static void CreateEntityConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .ToTable("Departments")
                .HasKey(x => x.id);


            modelBuilder.Entity<Employee>()
                .ToTable("Employees")
                .HasKey(x => x.id);

            modelBuilder.Entity<DepartmentEmployees>()
                .ToTable("DepartmentEmployees")
                .HasKey(x => new { x.employee_id, x.department_id });

            modelBuilder.Entity<DepartmentEmployees>()
               .HasOne(de => de.department)
               .WithMany(d => d.department_employees)
               .HasForeignKey(de => de.department_id);


            modelBuilder.Entity<DepartmentEmployees>()
               .HasOne(de => de.employee)
               .WithMany(d => d.department_employees)
               .HasForeignKey(de => de.employee_id);


        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            List<Department> departments = new List<Department>
                {
                     new Department
                     {
                         id=Guid.NewGuid().ToString(),
                         department_name="Phòng Kỹ thuật",
                     }
                };


            List<Employee> employees = new List<Employee>
                {
                    new Employee
                    {
                        id=Guid.NewGuid().ToString(),
                        fullname="Vũ Viết Tùng",
                    }
                };

            List<DepartmentEmployees> departmentEmployees = new List<DepartmentEmployees>
            {
                new DepartmentEmployees
                {
                    department_id=departments[0].id,
                    employee_id=employees[0].id,
                }
            };

            modelBuilder.Entity<Department>()
                .HasData(departments);

            modelBuilder.Entity<Employee>()
                .HasData(employees);

            modelBuilder.Entity<DepartmentEmployees>()
                .HasData(departmentEmployees);
        }
    }
}
