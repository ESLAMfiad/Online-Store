using Microsoft.EntityFrameworkCore;
using Store.Core.Models;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseModel
    {
        //function to build query
        //_dbContext.Set<T>().Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType);
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> Spec)
        {
            var Query=InputQuery; //_Dbcontext.set<T>() , _dbcontext.Products
            if( Spec.Criteria is not null)
            {
                Query=Query.Where(Spec.Criteria); //_dbContext.Set<T>().Where(P => P.Id == id)
            }
            if(Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy); // _dbcontext.Products.OrderBy(p=p.name)
            }
            if(Spec.OrderByDesc is not null)
            {
                Query=Query.OrderByDescending(Spec.OrderByDesc);
            }
            if (Spec.IsPaginationEnabled)
            {
                Query=Query.Skip(Spec.Skip).Take(Spec.Take);
            }
            //Include(P => P.ProductBrand).Include(P => P.ProductType);
            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            //_dbcontext.Products.OrderBy(p=p.name).Include(P => P.ProductBrand).Include(P => P.ProductType);

            return Query;
        }
    }
}
