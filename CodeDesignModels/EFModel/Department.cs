using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models.EFModel
{
    public class Department : BaseModel
    {
        public string department_name { get; set; }
        public virtual List<DepartmentEmployees> department_employees { get; set; }
    }
}
