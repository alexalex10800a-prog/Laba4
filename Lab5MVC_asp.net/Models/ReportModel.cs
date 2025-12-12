using BLL2.Interface;
using BLL2.models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lab5MVC_asp.net.Models
{
    public class ReportViewModel
    {
        [DisplayName("Выберите отдел")]
        [Required(ErrorMessage = "Выберите отдел")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите отдел")]
        public int SelectedDepartmentId { get; set; }

        public List<DepartmentDTO> Departments { get; set; }

        // Результаты отчета
        public List<EmployeeProjectsResult> Results { get; set; }
    }

    public class ReportResultsViewModel
    {
        public int SelectedDepartmentId { get; set; }
        public List<DepartmentDTO> Departments { get; set; }
        public List<EmployeeProjectsResult> Results { get; set; }

        public string GetSelectedDepartmentName()
        {
            return Departments?.FirstOrDefault(d => d.ID == SelectedDepartmentId)?.DepartmentName ?? "Неизвестный отдел";
        }
    }
}