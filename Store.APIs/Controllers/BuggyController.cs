using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Repository.Data;

namespace Store.APIs.Controllers
{
 
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Not Found")]
        public ActionResult GetNotFound()
        {
            var Product= _dbContext.Products.Find(100);
            if(Product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(Product);
        }

        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturn= Product.ToString(); //error 
            //null reference exception
            return Ok(ProductToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadReq()
        {
            return BadRequest();
        }
        [HttpGet("BadRequest/{id}")]
        public  ActionResult GetBadReq(int id)
        {
            return Ok();
        }
    }
}
