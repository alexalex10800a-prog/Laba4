using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL2.models
{
    public class AssignmentDTO
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; } = "Active";
    }

}
