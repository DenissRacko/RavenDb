using Logic.Domain;
using Logic.Managers;
using Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(int companyId)
        {
            var model = new LoginModel
            {
                CompanyId = companyId
            };
            return View(model);
        }

        public ActionResult Authorize(Guid sessionId)
        {
            var model = ContextManager.GetUserBySession(sessionId);
            return Json(ContextManager.Authorize(model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var userModel = new UserAudit
                {
                    CompanyId = model.CompanyId,
                    Login = model.Login,
                    Password = model.Password,
                    SessionId = model.SessionId,
                };

                if(ContextManager.Authorize(userModel))
                {
                    var userToUpdate = ContextManager.GetCompanyUser(model.CompanyId);

                    if(userToUpdate != null)
                    {
                        userToUpdate.LastAuthorization = DateTime.Now;
                        userToUpdate.SessionId = model.SessionId;

                        ContextManager.SaveUserAudit(userToUpdate);
                    }

                    return RedirectToAction("Details", "Company", new { id = model.CompanyId });
                }
                else
                {
                    model.Successful = false;
                    ModelState.AddModelError("Login", "Login/Password is wrong");
                }

            }
            return View(model);
        }
    }
}