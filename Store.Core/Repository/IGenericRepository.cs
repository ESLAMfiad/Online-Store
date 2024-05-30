using Store.Core.Models;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        #region without specifications
        //get all
        Task<IReadOnlyList<T>> GetAllAsync();
        //get by id
        Task<T> GetByIdAsync(int id);
        #endregion

        #region with spec
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);

        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);
        #endregion

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        Task Add(T item);
        void Delete(T item);
        void Update(T item);
    }
}
