using Store.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; } =new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        //get all
        public BaseSpecifications()
        {
            //Includes = new List<Expression<Func<T, object>>>();  
        }
        //get by id
        public BaseSpecifications(Expression<Func<T,bool>> criterialExpression)
        {
            Criteria = criterialExpression;
            //Includes = new List<Expression<Func<T,object>>>();

        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy= orderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }

        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;
            Skip= skip;
            Take= take;
        }
    }
}
