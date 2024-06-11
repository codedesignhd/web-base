using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignModels.EFModel
{
    public class Employee : ModelBase
    {
        public string fullname { get; set; }
        //public string department_id { get; set; }
        public virtual List<DepartmentEmployees> department_employees { get; set; }
    }
}
