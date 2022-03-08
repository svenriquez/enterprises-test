using System;
using System.Collections.Generic;

#nullable disable

namespace enterprises_test
{
    public partial class Department
    {
        public Department()
        {
            DepartmentsEmployees = new HashSet<DepartmentsEmployee>();
        }

        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public long IdEnterprise { get; set; }

        public virtual Enterprise IdEnterpriseNavigation { get; set; }
        public virtual ICollection<DepartmentsEmployee> DepartmentsEmployees { get; set; }
    }
}
