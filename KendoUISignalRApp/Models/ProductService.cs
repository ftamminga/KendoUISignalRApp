using System.Data.Entity;

namespace KendoUISignalRApp.Models
{
    using System;
    using System.Linq;

    public class ProductService : IDisposable
    {
        private SampleDataContext dataContext;

        public ProductService(SampleDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        //int take, int skip, IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates
        public IQueryable<ProductViewModel> Read()
        {
            var retval = dataContext.Products.Select(product => new ProductViewModel
            {
                ProductID = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice.HasValue ? product.UnitPrice.Value : default(decimal),
                UnitsInStock = product.UnitsInStock.HasValue ? product.UnitsInStock.Value : default(short),
                Discontinued = product.Discontinued
            });
            return retval;
        }

        public void Create(ProductViewModel product)
        {
            var entity = new Product();

            entity.ProductName = product.ProductName;
            entity.UnitPrice = product.UnitPrice;
            entity.UnitsInStock = (short)product.UnitsInStock;
            entity.Discontinued = product.Discontinued;

            dataContext.Products.Add(entity);
            dataContext.SaveChanges();

            product.ProductID = entity.ProductId;
        }

        public void Update(ProductViewModel product)
        {
            var entity = new Product();

            entity.ProductId = product.ProductID;
            entity.ProductName = product.ProductName;
            entity.UnitPrice = product.UnitPrice;
            entity.UnitsInStock = (short)product.UnitsInStock;
            entity.Discontinued = product.Discontinued;

            dataContext.Products.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            dataContext.SaveChanges();
        }

        public void Destroy(ProductViewModel product)
        {
            var entity = new Product();

            entity.ProductId = product.ProductID;

            dataContext.Products.Attach(entity);

            dataContext.Products.Remove(entity);

            var orderDetails = dataContext.OrderDetails.Where(pd => pd.ProductId == entity.ProductId);

            foreach (var orderDetail in orderDetails)
            {
                dataContext.OrderDetails.Remove(orderDetail);
            }

            dataContext.SaveChanges();
        }

        public void Dispose()
        {
            dataContext.Dispose();
        }
    }
}