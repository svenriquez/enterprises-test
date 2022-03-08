using System;
using System.Collections.Generic;

#nullable disable

namespace enterprises_test
{
    public partial class Employee
    {
        public Employee()
        {
            DepartmentsEmployees = new HashSet<DepartmentsEmployee>();
        }

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

        public virtual ICollection<DepartmentsEmployee> DepartmentsEmployees { get; set; }
    }
}
