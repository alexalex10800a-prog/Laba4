using BLL2.Interface;
using BLL2.models;
using Lab5MVC_asp.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Lab5MVC_asp.net.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        IDbCrud crudServ;
        public EmployeesController(IDbCrud crudDb)
        {

            crudServ = crudDb;

        }
        public ActionResult Index()
        {
            List<BLL2.models.SpecialtyDTO> slist = crudServ.GetAllSpecialties();
            List<BLL2.models.DepartmentDTO> dlist = crudServ.GetAllDepartments();
            var items = crudServ.GetAllEmployees().Select(i => new EmployeeDTO_MVC(i, dlist, slist));
            return View(items);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Получить списки
            var departments = crudServ.GetAllDepartments();
            var specialties = crudServ.GetAllSpecialties();
            System.Diagnostics.Debug.WriteLine($"Create: Departments count = {departments?.Count}");
            System.Diagnostics.Debug.WriteLine($"Create: Specialties count = {specialties?.Count}");

            if (departments == null || specialties == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Departments or Specialties is null!");
            }
            // Передать в конструктор модели (как в примере!)
            var model = new EmployeeDTO_MVC(departments, specialties);
            return View(model);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            // Получить сотрудника
            var employee = crudServ.GetEmployeeById(id);

            // Получить списки (как в примере!)
            var departments = crudServ.GetAllDepartments();
            var specialties = crudServ.GetAllSpecialties();

            // Передать все в конструктор (как в примере!)
            var model = new EmployeeDTO_MVC(employee, departments, specialties);
            return View(model);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeDTO_MVC model)
        {
            if (ModelState.IsValid)
            {
                // Создать DTO для обновления
                var employeeDto = new EmployeeDTO
                {
                    ID = model.ID,
                    FullName = model.FullName,
                    DepartmentCode = model.DepartmentCode,
                    SpecialtyCode = model.SpecialtyCode
                };

                crudServ.UpdateEmployee(employeeDto);
                return RedirectToAction("Index");
            }

            // ЕСЛИ ОШИБКА - НУЖНО ЗАГРУЗИТЬ СПИСКИ ЗАНОВО!
            model.Department = crudServ.GetAllDepartments();
            model.Specialty = crudServ.GetAllSpecialties();
            return View(model);
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(EmployeeDTO_MVC model)
        {
            if (ModelState.IsValid)
            {
                var employeeDto = new EmployeeDTO
                {
                    FullName = model.FullName,
                    DepartmentCode = model.DepartmentCode,
                    SpecialtyCode = model.SpecialtyCode
                };

                crudServ.CreateEmployee(employeeDto);
                return RedirectToAction("Index");
            }

            // ЕСЛИ ОШИБКА - ЗАГРУЗИТЬ СПИСКИ!
            model.Department = crudServ.GetAllDepartments();
            model.Specialty = crudServ.GetAllSpecialties();
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var employee = crudServ.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Для отображения названий отделов и специальностей
            var departments = crudServ.GetAllDepartments();
            var specialties = crudServ.GetAllSpecialties();

            var model = new EmployeeDTO_MVC(employee, departments, specialties);
            return View(model);
        }

        // POST: Employees/Delete/5 - фактическое удаление
        [HttpPost]
        [ActionName("Delete")] // То же имя, но разный HTTP метод
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                crudServ.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении: {ex.Message}");

                // Если ошибка, показываем форму снова
                var employee = crudServ.GetEmployeeById(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }

                var departments = crudServ.GetAllDepartments();
                var specialties = crudServ.GetAllSpecialties();
                var model = new EmployeeDTO_MVC(employee, departments, specialties);

                return View("Delete", model);
            }
        }
    }
}