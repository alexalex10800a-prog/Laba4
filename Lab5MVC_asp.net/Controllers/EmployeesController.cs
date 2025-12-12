using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL2.Interface;
using Lab5MVC_asp.net.Models;
namespace Lab5MVC_asp.net.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        IDbCrud crudServ;
        public EmployeesController(IDbCrud crudDb) { 
        
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
            return View(new EmployeeDTO_MVC());
        }
        [HttpPost]
        public ActionResult Create(EmployeeDTO_MVC emp)
        {
            BLL2.models.EmployeeDTO e = new BLL2.models.EmployeeDTO();
            e.FullName = emp.FullName;
            e.SpecialtyCode = emp.SpecialtyCode;
            e.DepartmentCode = emp.DepartmentCode;
            crudServ.CreateEmployee(e);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<BLL2.models.SpecialtyDTO> spec = crudServ.GetAllSpecialties();
            List<BLL2.models.DepartmentDTO> dep = crudServ.GetAllDepartments();
            EmployeeDTO_MVC e = new EmployeeDTO_MVC(crudServ.GetEmployeeById(id), dep, spec);
            return View(e);
        }
        [HttpPost]
        public ActionResult Edit(EmployeeDTO_MVC emp)
        {
            BLL2.models.EmployeeDTO e = new BLL2.models.EmployeeDTO();
            e.ID = emp.ID;
            e.FullName = emp.FullName;
            e.SpecialtyCode = emp.SpecialtyCode;
            e.DepartmentCode = emp.DepartmentCode;
            crudServ.UpdateEmployee(e);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            crudServ.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }
}