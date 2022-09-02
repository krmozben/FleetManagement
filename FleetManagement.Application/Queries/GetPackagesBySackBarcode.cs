using FleetManagement.Domain.ShipmentAggregate;
using MediatR;
using System.Collections.Generic;

namespace FleetManagement.Application.Queries
{
    public class GetPackagesBySackBarcode : IRequest<List<Shipment>>
    {
        public string Barcode { get; set; }
    }
}
