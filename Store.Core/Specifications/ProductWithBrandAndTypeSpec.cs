using Store.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public class ProductWithBrandAndTypeSpec :BaseSpecifications<Product>
    {
        //ctor used for  get all products
        public ProductWithBrandAndTypeSpec(ProductSpecParam Params)
            :base(P=>
            (string.IsNullOrEmpty(Params.Search)|| P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            && (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            //100 product
            //page size=10
            //page index=5
            // skip =40,take=10
            //
            ApplyPagination(Params.PageSize*(Params.PageIndex-1),Params.PageSize);
        }
        //ctor used for get product by id
        public ProductWithBrandAndTypeSpec(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
