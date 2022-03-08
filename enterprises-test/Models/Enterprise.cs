using System;
using System.Collections.Generic;

#nullable disable

namespace enterprises_test
{
    public partial class Enterprise
    {
        public Enterprise()
        {
            Departments = new HashSet<Department>();
        }

        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? Status { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
