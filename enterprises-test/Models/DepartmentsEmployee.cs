using System;
using System.Collections.Generic;

#nullable disable

namespace enterprises_test
{
    public partial class DepartmentsEmployee
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Status { get; set; }
        public long IdDepartment { get; set; }
        public long IdEmployee { get; set; }

        public virtual Department IdDepartmentNavigation { get; set; }
        public virtual Employee IdEmployeeNavigation { get; set; }
    }
}
