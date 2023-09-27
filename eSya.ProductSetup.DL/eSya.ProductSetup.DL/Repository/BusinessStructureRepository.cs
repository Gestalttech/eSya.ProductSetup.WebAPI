using eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.DO;
using eSya.ProductSetup.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.DL.Repository
{
    public class BusinessStructureRepository: IBusinessStructureRepository
    {
        public async Task<List<DO_BusinessEntity>> GetBusinessEntities()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsens
                                   .Where(w => w.ActiveStatus)
                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessId.ToString() + " - " + be.BusinessDesc,
                                      ActiveStatus=be.ActiveStatus
                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
