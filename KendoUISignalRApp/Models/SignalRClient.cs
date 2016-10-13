namespace KendoUISignalRApp.Models
{
    public class SignalRClient
    {
        public string ConnectionID { get; private set; } // ConnectionId is a unique Id per connection
        public KendoDataSourceRequest KendoDataSourceRequest { get; set; } 

        public SignalRClient(string connectionID)
        {
            ConnectionID = connectionID;
        }
    }
}