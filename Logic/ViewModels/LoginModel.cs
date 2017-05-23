using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ViewModels
{
    public class LoginModel
    {
        public int CompanyId { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public Guid SessionId { get; set; }

        public bool Successful { get; set; }
    }
}
