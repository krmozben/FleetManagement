using FleetManagement.Application.Commands;
using FleetManagement.Application.Queries;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.State;
using FleetManagement.Domain.ShipmentAggregate.Type;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.DeliveryPoint.Implementation
{
    public abstract class DeliveryPoint
    {
        protected readonly IMediator _mediator;

        protected DeliveryPoint(IMediator mediator) => _mediator = mediator;

        public abstract Task<List<Model.Response.Delivery>> Unloaded(List<Shipment> shipments);

        public async Task CheckPackageSackState(List<Shipment> shipments)
        {
            foreach (var shipment in shipments)
            {
                if (shipment.ShipmentType == ShipmentType.Package && shipment.PackageSack != null)
                    await ChangeSackState(shipment);
                else
                    await ChangePackagesState(shipment);
            }
        }

        private async Task ChangeSackState(Shipment shipment)
        {
            var packagesInSack = await _mediator.Send(new GetPackagesBySackBarcode() { Barcode = shipment.PackageSack.Sack.Barcode });

            if (packagesInSack == null)
                return;

            if (packagesInSack.Any(x => x.ShipmentState != ShipmentState.Unloaded))
                return;

            shipment.PackageSack.Sack.SetState(ShipmentState.Unloaded);
            await _mediator.Send(new SetShipmentState { Shipments = new() { shipment } });
        }


        private async Task ChangePackagesState(Shipment shipment)
        {
            var packagesInSack = await _mediator.Send(new GetPackagesBySackBarcode() { Barcode = shipment.Barcode });

            if (packagesInSack == null)
                return;

            if (!packagesInSack.Any())
                return;

            packagesInSack.ForEach(x => x.SetState(ShipmentState.Unloaded));
            await _mediator.Send(new SetShipmentState { Shipments = packagesInSack });
        }
    }
}
