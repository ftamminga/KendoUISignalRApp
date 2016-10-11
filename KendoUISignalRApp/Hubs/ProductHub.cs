using Kendo.DynamicLinq;
using KendoUISignalRApp.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Diagnostics;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace KendoUISignalRApp.Hubs

{
    public class ProductHub : Hub
    {
        private ProductService productService;

        public ProductHub()
        { 
            productService = new ProductService(new SampleDataContext());
        }

        public DataSourceResult Read(KendoDataSourceRequest request)
        {
            try
            {
                var products = productService.Read();

                if (request.Sort == null || !request.Sort.Any())
                {
                    products = products.OrderBy(p => p.ProductName);
                }

                var q = products.AsQueryable();
                var retval = products.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter, request.Aggregates);
                return retval;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Update(ProductViewModel product)
        {
            productService.Update(product);
            Clients.Others.update(new DataSourceResult{ Data = new[] {product} } );
        }

        public void Destroy(ProductViewModel product)
        {
            productService.Destroy(product);
            Clients.Others.destroy(new DataSourceResult { Data = new[] { product } });
        }

        public DataSourceResult Create(ProductViewModel product)
        {
            productService.Create(product);

            var dataSourceResult = new DataSourceResult {Data = new[] {product}};

            Clients.Others.create(dataSourceResult);
            Clients.Others.showMessage($"{DateTime.Now} Message from server: Create product {product.ProductID}");

            return dataSourceResult;
        }

        public override Task OnConnected()
        {
            Debug.WriteLine($"Connected: Client = {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Debug.WriteLine($"Disconnected: Client = {Context.ConnectionId}");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Debug.WriteLine($"Reconnected: Client = {Context.ConnectionId}");
            return base.OnReconnected();
        }
    }
}