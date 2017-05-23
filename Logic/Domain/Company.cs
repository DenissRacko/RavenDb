using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int OwnerId { get; set; }
        public string AddressLine { get; set; }

        [Range(0, Double.MaxValue)]
        public float Budget { get; set; }

        public Company ()
        {

        }
    }
}
