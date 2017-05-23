using Logic.Domain;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Indexes
{
    public class Employees_All : AbstractIndexCreationTask<Employee>
    {
        public Employees_All ()
        {
            Map = Employees => from employee in Employees
                               select new
                               {
                                   employee.Name,
                                   employee.Surname,
                                   employee.Age,
                                   employee.Gender,
                                   employee.CompanyId
                               };
        }
    }
}
