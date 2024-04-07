using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesign.Models.EFModel;
using CodeDesign.Sql;

namespace CodeDesign.Sql.Repositories
{
    public interface IUnitOfWork
    {
        GenericRepository<Department> DepartmentRepo { get; }
        GenericRepository<Employee> EmployeeRepo { get; }
        GenericRepository<DepartmentEmployees> DepartmentEmployeeRepo { get; }

        void SaveChanges();
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DemoDbContext _context;

        public UnitOfWork(DemoDbContext context)
        {
            _context = context;
        }

        private GenericRepository<Department> _departmentRepository;

        public GenericRepository<Department> DepartmentRepo
        {
            get
            {
                if (_departmentRepository == null)
                {
                    return new GenericRepository<Department>(_context);
                }
                return _departmentRepository;
            }

        }

        private GenericRepository<Employee> _employeeRepository;
        public GenericRepository<Employee> EmployeeRepo
        {
            get
            {
                if (_employeeRepository == null)
                {
                    return new GenericRepository<Employee>(_context);
                }
                return _employeeRepository;
            }
        }

        private GenericRepository<DepartmentEmployees> _departmentEmployeeRepository;
        public GenericRepository<DepartmentEmployees> DepartmentEmployeeRepo
        {
            get
            {
                if (_departmentEmployeeRepository == null)
                {
                    return new GenericRepository<DepartmentEmployees>(_context);
                }
                return _departmentEmployeeRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Commit()
        {

        }
    }

}
