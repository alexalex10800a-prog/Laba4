using BLL2.models;
using BLL2.Services;
using DAL2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BLL2
{
    // BLL/DBDataOperations.cs
    public class DBDataOperations
    {
        private Model1 _context;

        public DBDataOperations()
        {
            _context = new Model1();
            _context.employees.Load();
        }
        public bool HasEmployeesWithSpecialty(int specialtyId)
        {
            return _context.employees.Any(e => e.specialty_code_FK1 == specialtyId);
        }
        public bool Save()
        {
            try
            {
                // Check if there are any changes first
                if (!_context.ChangeTracker.HasChanges())
                {
                    // No changes to save - this is NOT an error!
                    return true; // ← CHANGE THIS from false to true
                }

                int recordsAffected = _context.SaveChanges();

                // SaveChanges() returns 0 when no changes were made
                // This is SUCCESS, not failure!
                return true; // ← ALWAYS return true if no exception occurred
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save error: {ex.Message}");
                return false; // ← Only return false on actual errors
            }
        }
        public void UpdateEmployee(EmployeeDTO emp)
        {
            employee e = _context.employees.Find(emp.ID);
            e.full_name = emp.FullName;
            e.specialty_code_FK1 = emp.SpecialtyCode;
            e.department_code_FK2 = emp.DepartmentCode;

            Save();
        }
        public void DeleteEmployee(int id)
        {
            employee emp = _context.employees.Find(id);
            if (emp != null)
            {
                _context.employees.Remove(emp);
                Save();
            }
        }
        public BusinessResult CreateEmployee(EmployeeDTO e)
        {
            try
            {
                // BUSINESS RULE: Check if department already has 3 employees with this specialty
                var sameSpecialtyCount = _context.employees
                    .Count(emp => emp.department_code_FK2 == e.DepartmentCode
                               && emp.specialty_code_FK1 == e.SpecialtyCode);

                if (sameSpecialtyCount >= 3)
                {
                    var department = _context.departments.Find(e.DepartmentCode);
                    var specialty = _context.specialties.Find(e.SpecialtyCode);

                    return BusinessResult.Fail(
                        $"Отдел '{department.department_name}' уже имеет {sameSpecialtyCount} сотрудников " +
                        $"со специальностью '{specialty.specialty_name}'. Максимум 3 разрешено.");
                }

                // If rule passes, create the employee
                _context.employees.Add(new employee()
                {
                    full_name = e.FullName,
                    specialty_code_FK1 = e.SpecialtyCode,
                    department_code_FK2 = e.DepartmentCode
                });

                bool saveResult = Save();

                if (saveResult)
                    return BusinessResult.Success("Сотрудник успешно создан!");
                else
                    return BusinessResult.Fail("Ошибка при сохранении сотрудника");
            }
            catch (Exception ex)
            {
                return BusinessResult.Fail($"Ошибка системы: {ex.Message}");
            }
        }
        public List<EmployeeDTO> GetAllEmployees()
        {
            return _context.employees.ToList().Select(i => new EmployeeDTO(i)).ToList();
        }

        public void UpdateSpecialty(SpecialtyDTO sp)
        {
            specialty s = _context.specialties.Find(sp.ID);
            s.specialty_name = sp.SpecialtyName;
            
            Save();
        }
        public void DeleteSpecialty(int id)
        {
            specialty sp = _context.specialties.Find(id);
            if (sp != null)
            {
                _context.specialties.Remove(sp);
                Save();
            }
        }
        public void CreateSpecialty(SpecialtyDTO e)
        {
            _context.specialties.Add(new specialty() { specialty_name = e.SpecialtyName });
            Save();
            //db.Phones.Attach(p);
        }

        /* public List<OrderModel> GetAllOrders()
         {
             return db.Orders.ToList().Select(i => new OrderModel(i)).ToList();
         }

         public PhoneModel GetPhone(int Id)
         {
             return new PhoneModel(db.Phones.Find(Id));
         }

         

         public void UpdatePhone(PhoneModel p)
         {
             Phone ph = db.Phones.Find(p.Id);
             ph.Name = p.Name;
             ph.Cost = p.Cost;
             ph.Description = p.Description;
             ph.ManufacturerId = p.ManufacturerId;
             Save();
         }

         public void DeletePhone(int id)
         {
             Phone p = db.Phones.Find(id);
             if (p != null)
             {
                 db.Phones.Remove(p);
                 Save();
             }
         }


         public bool Save()
         {
             if (db.SaveChanges() > 0) return true;
             return false;
         }
        */
        public List<DepartmentStatsDto> GetDepartmentStats()
        {
            var query = from e in _context.employees
                        join d in _context.departments on e.department_code_FK2 equals d.department_code
                        join c in _context.contracts on e.employee_id equals c.leader_code_FK
                        where c.cost > 10000
                        group new { e, c } by d.department_name into g
                        select new DepartmentStatsDto
                        {
                            DepartmentName = g.Key,
                            EmployeeCount = g.Select(x => x.e.employee_id).Distinct().Count(),
                            AvgContractCost = g.Average(x => x.c.cost)
                        };

            return query.ToList();
        }
        public List<EmployeeDTO> GetEmployeesByDepartment(int departmentId)
        {
            return _context.Database
                .SqlQuery<EmployeeDTO>(
                    "EXEC mydb.GetEmployeesByDepartment @DepartmentId",
                    new SqlParameter("@DepartmentId", departmentId))
                .ToList();
        }
        public List<EmployeeProjectsDTO> GetEmployeesWithProjectsByDepartment(int departmentId)
        {
            return _context.Database
                .SqlQuery<EmployeeProjectsDTO>(
                    "EXEC mydb.GetEmployeesWithProjectsByDepartment @DepartmentId",
                    new SqlParameter("@DepartmentId", departmentId))
                .ToList();
        }
        // Also add a method to get departments for the combobox
        public List<DepartmentDTO> GetAllDepartments1()
        {
            return _context.departments
                .Select(d => new DepartmentDTO
                {
                    ID = d.department_code,
                    DepartmentName = d.department_name
                })
                .ToList();
        }
        // In DBDataOperations.cs
        public EmployeeDTO GetEmployeeById(int employeeId)
        {
            return _context.employees
                .Where(e => e.employee_id == employeeId)
                .Include(e => e.specialty)
                .Include(e => e.department)
                .Select(e => new EmployeeDTO
                {
                    ID = e.employee_id,
                    FullName = e.full_name,
                    SpecialtyCode = e.specialty_code_FK1,
                    SpecialtyName = e.specialty.specialty_name,
                    DepartmentCode = e.department_code_FK2,  // Fixed property name
                    DepartmentName = e.department.department_name
                })
                .FirstOrDefault();
        }
        public List<SpecialtyDTO> GetAllSpecialties()
        {
            return _context.specialties.ToList().Select(i => new SpecialtyDTO(i)).ToList();
        }
        public List<DepartmentDTO> GetAllDepartments()
        {
            return _context.departments.ToList().Select(a => new DepartmentDTO(a)).ToList();
        }
    }
}
