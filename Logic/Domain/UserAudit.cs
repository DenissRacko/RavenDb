using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Domain
{
    public class UserAudit
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastAuthorization { get; set; }
        public Guid SessionId { get; set; }
    }
}
