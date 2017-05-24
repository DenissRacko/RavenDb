using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int CompanyId { get; set; }
    }
}
