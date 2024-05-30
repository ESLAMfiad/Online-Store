using Microsoft.EntityFrameworkCore;
using Store.Core.Models;
using Store.Core.Models.Orderagg;
using Store.Repository.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data
{
    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext>options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fluent apis
            //modelBuilder.ApplyConfiguration(new ProductConfig()); old way to apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //btgeb kol elconfigs wt3mlha apply 3n treq interface l felconfigs

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethod { get; set;}
        

    }
}
