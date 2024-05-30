using Store.Core;
using Store.Core.Models;
using Store.Core.Repository;
using Store.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
          return await _dbContext.SaveChangesAsync(); 
        }

        public async ValueTask DisposeAsync()
        {
           await _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            //product , genericrepository of product (key: value)
            var type = typeof(TEntity).Name; //product
            if (!_repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext); //h5zno fhashtable 3shan my3mlsh create ll obj kol ma anadeh wy7fz 3ndo
                _repositories.Add(type, Repository);
            }
            return _repositories[type] as  IGenericRepository<TEntity>; 
            //kda enta 5let elfunction tgenerate object mn elgeneric repo mn eltype l m7ddo
        }
    }
}
