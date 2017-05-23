using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ViewModels
{
    public class CompanyDetailsModel : Company
    {
        public List<Employee> Employees { get; set; }
    }
}
