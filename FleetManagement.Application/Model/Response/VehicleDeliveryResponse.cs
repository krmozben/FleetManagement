using System.Collections.Generic;

namespace FleetManagement.Application.Model.Response
{
    public class Delivery
    {
        public string Barcode { get; set; }
        public int State { get; set; }
    }

    public class VehicleDeliveryResponse
    {
        public VehicleDeliveryResponse()
        {
            Route = new();
        }

        public string Vehicle { get; set; }
        public List<Route> Route { get; set; }
    }

    public class Route
    {
        public Route()
        {
            Deliveries = new();
        }
        public object DeliveryPoint { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
