using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Logic.Models;
using Newtonsoft.Json;
using Logic.Managers;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Company");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult CreateCompany (NewCompanyModel company)
        {
            //NewCompanyModel model = JsonConvert.DeserializeObject<NewCompanyModel>(company);

            company = ContextManager.SaveNewCompany(company);

            return Json(company, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReAdd(EmployeeReAddModel result)
        {
            result = ContextManager.ReAddEmployees(result);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}