using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Application.Model.Request
{
    public class Delivery
    {
        [Required]
        public string Barcode { get; set; }
    }

    public class VehicleDeliveryRequest
    {
        public string Vehicle { get; set; }
        public List<Route> Route { get; set; }
    }

    public class Route
    {
        [Required]
        public int DeliveryPoint { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
