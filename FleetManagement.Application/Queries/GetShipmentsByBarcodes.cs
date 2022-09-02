using FleetManagement.Domain.ShipmentAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Application.Queries
{
    public class GetShipmentsByBarcodes : IRequest<List<Shipment>>
    {
        public List<string> Barcodes { get; set; }
    }
}
