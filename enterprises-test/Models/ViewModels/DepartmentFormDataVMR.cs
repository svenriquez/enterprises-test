using System.Collections.Generic;

namespace enterprises_test.Models.ViewModels
{
    public class DepartmentFormDataVMR
    {
        public List<Enterprise> enterpriseList { get; set; }

        public DepartmentFormDataVMR()
        {
            enterpriseList = new List<Enterprise>();
        }
    }
}
