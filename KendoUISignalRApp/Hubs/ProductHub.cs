using Kendo.DynamicLinq;
using KendoUISignalRApp.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using Task = System.Threading.Tasks.Task;

namespace KendoUISignalRApp.Hubs

{
    public class ProductHub : Hub
    {
        private ProductService productService;
        private static readonly Dictionary<string, SignalRClient> HubClients = new Dictionary<string, SignalRClient>();

        public ProductHub()
        { 
            productService = new ProductService(new SampleDataContext());
        }

        public DataSourceResult Read(KendoDataSourceRequest request)
        {
            try
            {
                CurrentClient().KendoDataSourceRequest = request;

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

            var hub = GlobalHost.ConnectionManager.GetHubContext<ProductHub>();
            var tempProdlist = new List<ProductViewModel> {product};

            foreach (var client in HubClients.Values)
            {
                // Mimic behaviour of Clients.Others
                // Continue the foreach when client is itself
                if (client.ConnectionID == Context.ConnectionId) continue;

                var request = client.KendoDataSourceRequest;
                var result = tempProdlist.AsQueryable().ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter, request.Aggregates);

                // Only send data if (filtered and paged) result holds data
                if (result.Data.Any())
                {
                    hub.Clients.Client(client.ConnectionID).update(result);
                }
                else
                {
                    hub.Clients.Client(client.ConnectionID).destroy(new DataSourceResult { Data = new[] { product } });  // Remove results that are not valid for this filter
                }
            }
        }

        public void Destroy(ProductViewModel product)
        {
            productService.Destroy(product);
            Clients.Others.destroy(new DataSourceResult { Data = new[] { product } });
        }

        public DataSourceResult Create(ProductViewModel product)
        {
            productService.Create(product);

            var dataSourceResult = new DataSourceResult {Data = new[] {product}}; // For this client

            var hub = GlobalHost.ConnectionManager.GetHubContext<ProductHub>();
            var tempProdlist = new List<ProductViewModel> { product };

            foreach (var client in HubClients.Values)
            {
                // Mimic behaviour of Clients.Others
                // Continue the foreach when client is itself
                if (client.ConnectionID == Context.ConnectionId) continue;

                var request = client.KendoDataSourceRequest;
                var result = tempProdlist.AsQueryable().ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter, request.Aggregates);

                // Only send data if (filtered and paged) result holds data
                if (result.Data.Any())
                {
                    hub.Clients.Client(client.ConnectionID).create(result);
                }
            }

            Clients.Others.showMessage($"{DateTime.Now} Message from server: Create product {product.ProductID}");

            return dataSourceResult;
        }
        
        private SignalRClient CurrentClient()
        {
            return HubClients[Context.ConnectionId];
        }

        private void AddClient()
        {
            // Create a new client object for this context
            var client = new SignalRClient(Context.ConnectionId);
            HubClients.Add(client.ConnectionID, client);
        }

        private void RemoveClient()
        {
            // Remove client object from collection
            if (HubClients.ContainsKey(Context.ConnectionId))
            {
                HubClients.Remove(Context.ConnectionId);
            }
        }
        public override Task OnConnected()
        {
            Debug.WriteLine($"Connected: Client = {Context.ConnectionId}");
            AddClient();
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
            RemoveClient();
            return base.OnReconnected();
        }
    }
}