using AHP.DAL;
using AHP.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AHP.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionScope transactionScope;
        private bool disposedValue = false;

        public UnitOfWork()
        {
            transactionScope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Commit()
        {
            transactionScope.Complete();
        }


        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (transactionScope != null)
                    {
                        transactionScope.Dispose();
                    }
                }
                disposedValue = true;
            }
        }
    }
}
