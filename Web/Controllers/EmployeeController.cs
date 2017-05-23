using Logic.Domain;
using Logic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult List(int? companyId)
        {
            List<Employee> list = new List<Employee>();

            if(companyId > 0)
            {
                list = ContextManager.GetAllCompanyEmployees((int)companyId);
            }
            else
            {
                list = ContextManager.GetAllEmployees();
            }
            return View(list);
        }

        public ActionResult Details(int id)
        {
            var model = ContextManager.GetEmployeeById(id);
            return View(model);
        }

        public ActionResult Create (int companyId)
        {

            var cookie = Request.Cookies["session-" + companyId];
            var companyUser = ContextManager.GetCompanyUser(companyId);

            if (cookie == null && companyUser != null)
            {
                return RedirectToAction("Login", "Login", new { companyId = companyId });
            }
            else if (cookie != null && companyUser != null)
            {
                var sessionId = Guid.Parse(cookie.Value);
                var userAudit = ContextManager.GetUserBySession(sessionId);

                if (!ContextManager.Authorize(userAudit))
                {
                    return RedirectToAction("Login", "Login", new { companyId = companyId });
                }
            }

            Employee model = new Employee
            {
                CompanyId = companyId,
            };

            

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Employee model)
        {
            if(ModelState.IsValid)
            {
                ContextManager.SaveEmployee(model);
                return RedirectToAction("Details", "Company", new { id = model.CompanyId });
            }
            return View(model);
        }

        public ActionResult EditEmployee(int id)
        {
            var model = ContextManager.GetEmployeeById(id);

            var cookie = Request.Cookies["session-" + model.CompanyId];
            var companyUser = ContextManager.GetCompanyUser(model.CompanyId);

            if (cookie == null && companyUser != null)
            {
                return RedirectToAction("Login", "Login", new { companyId = model.CompanyId });
            }
            else if (cookie != null && companyUser != null)
            {
                var sessionId = Guid.Parse(cookie.Value);
                var userAudit = ContextManager.GetUserBySession(sessionId);

                if (!ContextManager.Authorize(userAudit))
                {
                    return RedirectToAction("Login", "Login", new { companyId = model.CompanyId });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                ContextManager.SaveEmployee(model);
                return RedirectToAction("Details", "Company", new { id = model.CompanyId});
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {         
            var model = ContextManager.GetEmployeeById(id);

            var cookie = Request.Cookies["session-" + model.CompanyId];
            var companyUser = ContextManager.GetCompanyUser(model.CompanyId);

            if (cookie == null && companyUser != null)
            {
                return RedirectToAction("Login", "Login", new { companyId = model.CompanyId });
            }
            else if (cookie != null && companyUser != null)
            {
                var sessionId = Guid.Parse(cookie.Value);
                var userAudit = ContextManager.GetUserBySession(sessionId);

                if (!ContextManager.Authorize(userAudit))
                {
                    return RedirectToAction("Login", "Login", new { companyId = model.CompanyId });
                }
            }

            ContextManager.DeleteEmployee(id);
            return RedirectToAction("Details", "Company", new { id = model.CompanyId });

        }
    }
}