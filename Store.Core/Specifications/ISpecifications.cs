using Store.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseModel
    {
        //_dbContext.Products.where(P=>P.id== id).Include(P => P.ProductBrand).Include(P => P.ProductType);

        //signature for property of where condition [where(P=>P.id== id)]
        public Expression<Func<T, bool>> Criteria { get; }

        //signat for property for list of includes 
        public List<Expression<Func<T, object>>> Includes { get; }
        public Expression<Func<T,object>> OrderBy { get; set; }
        // property for order by[orderbydesc(p=>p.name)]
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        //take(2)
        public int Take { get; set; }
        //skip(2)
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }




    }
}
