using Store.Core.Models;
using Store.Core.Models.Orderagg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Data
{
    public static class StoreContextSeed
    {
        //seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Store.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0) //not null w akbr mn zero
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                }
            }
            //seeding types
            if (!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Store.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0) //not null w akbr mn zero
                {
                    foreach (var type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(type);
                    }
                }
            }

            //seeding product
            if (!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Store.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (products?.Count > 0) //not null w akbr mn zero
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                }
            }

            if (!dbContext.DeliveryMethod.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Store.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0) //not null w akbr mn zero
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                }
            }
            await dbContext.SaveChangesAsync();


        }
    }
}
