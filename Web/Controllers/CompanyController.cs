using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic.Managers;
using Logic.Domain;
using Logic.ViewModels;

namespace Web.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult List()
        {
            var list = ContextManager.GetAllCompanies();
            return View(list);
        }
     
        public ActionResult Details(int id)
        {
            CompanyDetailsModel model = new CompanyDetailsModel();
            Company company = ContextManager.GetCompanyById(id);

            model.Id = company.Id;
            model.CompanyId = company.CompanyId;
            model.Name = company.Name;
            model.OwnerId = company.OwnerId;
            model.Budget = company.Budget;
            model.AddressLine = company.AddressLine;

            model.Employees = ContextManager.GetAllCompanyEmployees(model.Id);

            return View(model);
        }

        public ActionResult CreateCompany()
        {
            var model = new CompanyCreateEditModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyCreateEditModel model)
        {          
            if(ModelState.IsValid)
            {
                ContextManager.SaveCompany(model);
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult EditCompany(int id)
        {

            var cookie = Request.Cookies["session-" + id];
            var companyUser = ContextManager.GetCompanyUser(id);

            if (cookie == null && companyUser != null)
            {
                return RedirectToAction("Login", "Login", new { companyId = id });
            }
            else if(cookie != null && companyUser != null)
            {
                var sessionId = Guid.Parse(cookie.Value);
                var userAudit = ContextManager.GetUserBySession(sessionId);

                if (!ContextManager.Authorize(userAudit))
                {
                    return RedirectToAction("Login", "Login", new { companyId = id });
                }
            }

            var model = new CompanyCreateEditModel();
            var company = ContextManager.GetCompanyById(id);

            model.Id = company.Id;
            model.Name = company.Name;
            model.Budget = company.Budget;
            model.AddressLine = company.AddressLine;
            model.CompanyId = company.CompanyId;
            model.OwnerId = company.OwnerId;

            if(companyUser != null)
            {
                model.Login = companyUser.Login;
                model.Password = companyUser.Password;
            }
            

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                ContextManager.SaveCompany(model);
                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }

        public ActionResult DeleteCompany(int id)
        {
            var cookie = Request.Cookies["session-" + id];
            var companyUser = ContextManager.GetCompanyUser(id);

            if (cookie == null && companyUser != null)
            {
                return RedirectToAction("Login", "Login", new { companyId = id });
            }
            else if (cookie != null && companyUser != null)
            {
                var sessionId = Guid.Parse(cookie.Value);
                var userAudit = ContextManager.GetUserBySession(sessionId);

                if (!ContextManager.Authorize(userAudit))
                {
                    return RedirectToAction("Login", "Login", new { companyId = id });
                }
            }

            var model = ContextManager.GetCompanyById(id);
            ContextManager.DeleteCompany(id);
            return RedirectToAction("List");
        }
    }
}