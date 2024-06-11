using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignModels.EFModel
{
    public class Department : ModelBase
    {
        public string department_name { get; set; }
        public virtual List<DepartmentEmployees> department_employees { get; set; }
    }
}
