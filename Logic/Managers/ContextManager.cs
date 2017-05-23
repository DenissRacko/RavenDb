using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client;
using Logic.Domain;
using Raven.Client.Indexes;
using Logic.ViewModels;

namespace Logic.Managers
{
    public static class ContextManager
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    ConnectionStringName = "EmployementMarket"
                };

                return store.Initialize();             
            });

        public static IDocumentStore Store => LazyStore.Value;
#region Company
        public static List<Company> GetAllCompanies()
        {
            var allCompanies = new List<Company>();
            using (var session = Store.OpenSession())
            {
                allCompanies = session.Query<Company>("Companies/All").Customize(cu => cu.WaitForNonStaleResults()).ToList();
            }

            return allCompanies;
        }

        public static Company GetCompanyById(int companyId)
        {
            Company model = new Company();
            using (var session = Store.OpenSession())
            {
                model = session.Load<Company>("companies/" + companyId);
            }

            return model;
        }

        public static void SaveCompany (CompanyCreateEditModel model)
        {
            using (var session = Store.OpenSession())
            {
                var company = new Company
                {
                    Id = model.Id,
                    Name = model.Name,
                    Budget = model.Budget,
                    AddressLine = model.AddressLine,
                    CompanyId = model.CompanyId,
                    OwnerId = model.OwnerId,
                };

                session.Store(company);

                if(model.Login != null && model.Password != null)
                {
                    var newCompanyId = company.Id;

                    var userAudit = new UserAudit
                    {
                        CompanyId = newCompanyId,
                        Login = model.Login,
                        Password = model.Password,
                        LastAuthorization = DateTime.Now
                    };

                    session.Store(userAudit);

                    company.OwnerId = userAudit.Id;

                    session.Store(company);

                }                
                session.SaveChanges();
            }
        }

        public static void DeleteCompany(int companyId)
        {
            Company model = new Company();
            using (var session = Store.OpenSession())
            {               
                DeleteCompanyEmployees(companyId);

                model = session.Load<Company>("companies/" + companyId);
                session.Delete<Company>(model);
                session.SaveChanges();
            }
        }
        #endregion

        #region Employee
        public static List<Employee> GetAllEmployees()
        {
            var allEmployees = new List<Employee>();
            using (var session = Store.OpenSession())
            {
                allEmployees = session.Query<Employee>("Employees/All").Customize(cu => cu.WaitForNonStaleResults()).ToList();
            }

            return allEmployees;
        }

        public static List<Employee> GetAllCompanyEmployees(int companyId)
        {
            var allEmployees = new List<Employee>();
            using (var session = Store.OpenSession())
            {
                allEmployees = session.Query<Employee>("Employees/All")
                                .Customize(cu => cu.WaitForNonStaleResults())
                                .Where(e => e.CompanyId == companyId)
                                .ToList();
            }

            return allEmployees;
        }

        public static Employee GetEmployeeById(int employeeId)
        {
            Employee model = new Employee();
            using (var session = Store.OpenSession())
            {
                model = session.Load<Employee>("employees/" + employeeId);
            }

            return model;
        }

        public static void SaveEmployee(Employee model)
        {
            using (var session = Store.OpenSession())
            {
                session.Store(model);
                session.SaveChanges();
            }
        }

        public static void DeleteEmployee(int employeeId)
        {
            Employee model = new Employee();
            using (var session = Store.OpenSession())
            {
                model = session.Load<Employee>("employees/" + employeeId);
                session.Delete<Employee>(model);
                session.SaveChanges();
            }
        }

        public static void DeleteCompanyEmployees(int companyId)
        {
            using (var session = Store.OpenSession())
            {
                var employeesList = session.Query<Employee>("Employees/All")
                                .Customize(cu => cu.WaitForNonStaleResults())
                                .Where(e => e.CompanyId == companyId)
                                .ToList();

                foreach (var employee in employeesList)
                {
                    session.Delete<Employee>(employee);
                }            
                
                session.SaveChanges();
            }
        }
        #endregion


        #region UserAudit

        public static void SaveUserAudit(UserAudit model)
        {
            using (var session = Store.OpenSession())
            {
                session.Store(model);
                session.SaveChanges();
            }
        }

        public static UserAudit GetCompanyUser(int companyId)
        {
            var user = new UserAudit();
            using (var session = Store.OpenSession())
            {
                user = session.Query<UserAudit>("UserAudits/All")
                                .Customize(cu => cu.WaitForNonStaleResults())
                                .Where(u => u.CompanyId == companyId)
                                .FirstOrDefault();
            }

            return user;
        }

        public static UserAudit GetUserBySession(Guid sessionId)
        {
            var user = new UserAudit();
            using (var session = Store.OpenSession())
            {
                user = session.Query<UserAudit>("UserAudits/All")
                                .Customize(cu => cu.WaitForNonStaleResults())
                                .Where(u => u.SessionId == sessionId)
                                .FirstOrDefault();
            }
            return user;
        }


        public static bool Authorize(UserAudit model)
        {
            var user = new UserAudit();
            using (var session = Store.OpenSession())
            {
                user = session.Query<UserAudit>("UserAudits/All")
                                .Customize(cu => cu.WaitForNonStaleResults())
                                .Where(u => u.CompanyId == model.CompanyId)
                                .FirstOrDefault();

                if (user == null)
                {
                    return true;
                }
                else if (model.SessionId == user.SessionId && ((DateTime.Now - model.LastAuthorization).Hours < 1) && model.SessionId != Guid.Empty)
                {
                    return true;
                }
                else if (user.CompanyId == model.CompanyId && user.Login == model.Login && user.Password == model.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
