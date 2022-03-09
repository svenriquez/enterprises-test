using System;
using System.Collections.Generic;

namespace enterprises_test.Models.ViewModels
{
    public class EmployeeFormDataVMR
    {
        public List<Enterprise> enterpriseList { get; set; }
        public List<Department> departmentList { get; set; }

        public EmployeeFormDataVMR()
        {
            enterpriseList = new List<Enterprise>();
            departmentList = new List<Department>();
        }
    }
}
