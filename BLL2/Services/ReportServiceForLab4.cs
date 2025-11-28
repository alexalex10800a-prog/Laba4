using BLL2.models;
using DAL2.Entities;
using System;
using DAL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace BLL2.Services
{
    public class ReportServiceForLab4
    {
        // Your report data structure (like OrdersByMonth in the example)
        public class EmployeeProjectsResult
        {
            public int EmployeeId { get; set; }
            public string FullName { get; set; }
            public string DepartmentName { get; set; }
            public string SpecialtyName { get; set; }
            public int ProjectCode { get; set; }
            public string ParticipationStatus { get; set; }
        }

        // Execute stored procedure (like ExecuteSP in the example)
        public static List<EmployeeProjectsResult> GetEmployeesWithProjectsByDepartment(int departmentId)
        {
            // Create DbContext internally (like PhonesDB in the example)
            Model1 db = new Model1();

            // Call stored procedure directly
            var result = db.Database.SqlQuery<EmployeeProjectsResult>(
                "EXEC mydb.GetEmployeesWithProjectsByDepartment @DepartmentId",
                new SqlParameter("@DepartmentId", departmentId))
                .ToList();

            return result;
        }

    }
}