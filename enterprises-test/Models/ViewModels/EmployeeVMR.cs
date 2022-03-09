using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Models.ViewModels
{
    public class EmployeeVMR
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Status { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }
        public long IdEnterprise { get; set; }
        public long IdDepartment { get; set; }
    }
}
