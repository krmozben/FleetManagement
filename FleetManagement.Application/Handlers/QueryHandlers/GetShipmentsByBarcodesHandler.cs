using FleetManagement.Application.Queries;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Infrastructure.Repositories.Base;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManagement.Application.Handlers.QueryHandlers
{
    public class GetShipmentsByBarcodesHandler : IRequestHandler<GetShipmentsByBarcodes, List<Shipment>>
    {
        private readonly IRepository<Shipment> _shipmentRepository;

        public GetShipmentsByBarcodesHandler(IRepository<Shipment> shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<List<Shipment>> Handle(GetShipmentsByBarcodes request, CancellationToken cancellationToken)
        {
            var shipments = await _shipmentRepository.GetAsync(x => request.Barcodes.Contains(x.Barcode));

            return shipments.ToList();
        }
    }
}
