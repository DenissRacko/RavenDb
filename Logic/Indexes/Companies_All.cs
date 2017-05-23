using Logic.Domain;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logic.Indexes
{
    public class Companies_All : AbstractIndexCreationTask<Company>
    {
        public Companies_All()
        {
            Map = Companies => from company in Companies
                               select new
                               {
                                   company.Name,
                                   company.Budget,
                                   company.AddressLine,
                                   company.Id,
                                   company.OwnerId,
                                   company.CompanyId
                               };
        }
    }
}
