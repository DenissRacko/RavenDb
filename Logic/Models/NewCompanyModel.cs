using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class NewCompanyModel : Company
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Employee> employees { get; set; }
    }
}
