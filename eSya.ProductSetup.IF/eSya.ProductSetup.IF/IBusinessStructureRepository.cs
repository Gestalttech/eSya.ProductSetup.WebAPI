using eSya.ProductSetup.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ProductSetup.IF
{
    public interface IBusinessStructureRepository
    {
        Task<List<DO_BusinessEntity>> GetBusinessEntities();
    }
}
