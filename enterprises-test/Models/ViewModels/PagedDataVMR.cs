using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Models.ViewModels
{
    public class PagedDataVMR<T>
    {
        public int total { get; set; }
        public List<T> elements { get; set; }

        public PagedDataVMR()
        {
            total = 0;
            elements = new List<T>();
        }
    }
}
