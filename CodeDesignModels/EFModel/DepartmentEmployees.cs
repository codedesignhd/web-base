using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models.EFModel
{
    public class DepartmentEmployees
    {
        public string department_id { get; set; }
        public string employee_id { get; set; }

        public Department department { get; set; }
        public Employee employee { get; set; }
    }
}
