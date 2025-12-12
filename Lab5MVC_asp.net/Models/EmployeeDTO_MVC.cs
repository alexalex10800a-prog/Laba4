using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using BLL2;
using BLL2.models;

namespace Lab5MVC_asp.net.Models
{
    public class EmployeeDTO_MVC
    {
        public int ID { get; set; }
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        public int SpecialtyCode { get; set; }
        public int DepartmentCode { get; set; }

        [DisplayName("Отдел")]
        public string DepartmentName { get; set; }
        [DisplayName("Специальность")]
        public string SpecialtyName { get; set; }
        public List<DepartmentDTO> Department { get; set; }
        public List<SpecialtyDTO> Specialty { get; set; }
        public EmployeeDTO_MVC() { }

        public EmployeeDTO_MVC(List<DepartmentDTO> Dep, List<SpecialtyDTO> Sp) {
            this.Department = Dep;
            this.Specialty = Sp; 
        }
        public EmployeeDTO_MVC(BLL2.models.EmployeeDTO e, List<DepartmentDTO> Dep, List<SpecialtyDTO> Sp)
        {
            FullName = e.FullName;
            SpecialtyCode = e.SpecialtyCode;
            DepartmentCode = e.DepartmentCode;
            DepartmentName = Dep.Where(i => i.ID == e.DepartmentCode).FirstOrDefault().DepartmentName;
            SpecialtyName = Sp.Where(i => i.ID == e.SpecialtyCode).FirstOrDefault().SpecialtyName;
            ID = e.ID;
            this.Department = Dep;
            this.Specialty = Sp;
        }
    }
}