using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models.EFModel
{
    public class Employee : ModelBase
    {
        public string fullname { get; set; }
        //public string department_id { get; set; }
        public virtual List<DepartmentEmployees> department_employees { get; set; }
    }
}
