using Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ViewModels
{
    public class CompanyCreateEditModel : Company
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
