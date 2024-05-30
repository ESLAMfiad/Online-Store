using Microsoft.EntityFrameworkCore;
using Store.Core.Models;
using Store.Core.Repository;
using Store.Core.Specifications;
using Store.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) //ask clr for creating obj from dbcontext implicitly
        {
            _dbContext = dbContext;
        }
        #region without spec
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // EL IF de moskn 3wdnah bel specification t7t 5las
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>) await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }



        public async Task<T> GetByIdAsync(int id)
        {
            //return await _dbContext.Set<T>().Where(X=>X.Id==id).FirstOrDefaultAsync();
            return await _dbContext.Set<T>().FindAsync(id);
            //return await _dbContext.Set<T>().Where(P => P.Id == id).Include(P => P.Productbrand).Include(P => P.ProductType);
        }
        #endregion

        #region with spec
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpec(Spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpec(Spec).FirstOrDefaultAsync();
        } 

        private IQueryable<T> ApplySpec(ISpecifications<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpec(Spec).CountAsync();
        }

        public async Task Add(T item)
        {
           await _dbContext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbContext.Remove(item);
        }

        public void Update(T item)
        {
           _dbContext.Update(item);
        }
        #endregion
    }
}
