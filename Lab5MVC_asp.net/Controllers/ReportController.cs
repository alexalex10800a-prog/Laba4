using BLL2.Interface;
using BLL2.models;
using Lab5MVC_asp.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lab5MVC_asp.net.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IDbCrud _crudService;

        public ReportController(IReportService reportService, IDbCrud crudService)
        {
            _reportService = reportService;
            _crudService = crudService;
        }

        // GET: Report/Index - главная страница отчетов
        public ActionResult Index()
        {
            return View();
        }

        // GET: Report/EmployeesByDepartment - форма для отчета
        [HttpGet]
        public ActionResult EmployeesByDepartment()
        {
            var departments = _crudService.GetAllDepartments();
            var model = new ReportViewModel
            {
                Departments = departments,
                SelectedDepartmentId = 0
            };

            return View(model);
        }

        // POST: Report/EmployeesByDepartment - обработка формы
        [HttpPost]
        public ActionResult EmployeesByDepartment(ReportViewModel model)
        {
            if (ModelState.IsValid && model.SelectedDepartmentId > 0)
            {
                try
                {
                    // Получаем данные отчета
                    var results = _reportService.GetEmployeesWithProjectsByDepartment(model.SelectedDepartmentId);

                    // Сохраняем в TempData для передачи в представление результатов
                    TempData["ReportResults"] = results;
                    TempData["SelectedDepartmentId"] = model.SelectedDepartmentId;

                    return RedirectToAction("ReportResults");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                }
            }

            // Если ошибка, загружаем отделы заново
            model.Departments = _crudService.GetAllDepartments();
            return View(model);
        }

        // GET: Report/ReportResults - отображение результатов
        public ActionResult ReportResults()
        {
            var results = TempData["ReportResults"] as List<EmployeeProjectsResult>;
            var departmentId = TempData["SelectedDepartmentId"] as int? ?? 0;

            if (results == null)
            {
                return RedirectToAction("EmployeesByDepartment");
            }

            var model = new ReportResultsViewModel
            {
                Results = results,
                SelectedDepartmentId = departmentId,
                Departments = _crudService.GetAllDepartments()
            };

            return View(model);
        }

        // Альтернативный вариант: все в одном действии
        public ActionResult EmployeesByDepartment2(int? departmentId)
        {
            var departments = _crudService.GetAllDepartments();
            var model = new ReportViewModel
            {
                Departments = departments
            };

            if (departmentId.HasValue && departmentId > 0)
            {
                try
                {
                    var results = _reportService.GetEmployeesWithProjectsByDepartment(departmentId.Value);
                    model.Results = results;
                    model.SelectedDepartmentId = departmentId.Value;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                }
            }

            return View("EmployeesByDepartment", model);
        }
    }
}