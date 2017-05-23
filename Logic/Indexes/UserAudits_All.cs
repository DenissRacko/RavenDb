using Logic.Domain;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logic.Indexes
{
    public class UserAudits_All : AbstractIndexCreationTask<UserAudit>
    {
        public UserAudits_All()
        {
            Map = UserAudits => from user in UserAudits
                                select new
                                {
                                    user.Login,
                                    user.Password,
                                    user.LastAuthorization,
                                    user.SessionId,
                                    user.Id,
                                    user.CompanyId
                                };
        }
    }
}
