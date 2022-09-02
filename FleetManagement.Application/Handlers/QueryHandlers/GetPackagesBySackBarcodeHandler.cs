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
    public class GetPackagesBySackBarcodeHandler : IRequestHandler<GetPackagesBySackBarcode, List<Shipment>>
    {
        private readonly IRepository<Shipment> _shipmentRepository;

        public GetPackagesBySackBarcodeHandler(IRepository<Shipment> shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<List<Shipment>> Handle(GetPackagesBySackBarcode request, CancellationToken cancellationToken)
        {
            var shipments = await _shipmentRepository.GetAsync(x => x.PackageSack.Sack.Barcode == request.Barcode);

            return shipments.ToList();
        }
    }
}
