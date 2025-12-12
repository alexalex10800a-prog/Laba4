using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL2;

namespace Lab5MVC_asp.net.Models
{
    public class EmployeeDTO_MVC
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int SpecialtyCode { get; set; }
        public int DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
        public string SpecialtyName { get; set; }

        public EmployeeDTO_MVC() { }
        public EmployeeDTO_MVC(BLL2.models.EmployeeDTO e)
        {
            FullName = e.FullName;
            SpecialtyCode = e.SpecialtyCode;
            DepartmentCode = e.DepartmentCode;
            ID = e.ID;
        }
    }
}