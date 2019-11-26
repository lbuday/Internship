using AHP.DAL;
using AHP.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private AHPEntities _context;
        public UnitOfWorkFactory(AHPEntities context)
        {
            _context = context;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork();
        }
    }
}
