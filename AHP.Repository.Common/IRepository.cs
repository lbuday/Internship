using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository.Common
{
    public interface IRepository<T>
    {
        T Add(T entity);
        List<T> AddRange(List<T> entities);
        Task<bool> DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<int> SaveAsync();
    }
}
