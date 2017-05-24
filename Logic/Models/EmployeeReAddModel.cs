using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class EmployeeReAddModel
    {
        public int EmployeeId { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
