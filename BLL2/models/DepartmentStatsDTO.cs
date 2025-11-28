using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL2.models
{
    public class DepartmentStatsDto
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal AvgContractCost { get; set; }
    }
}
