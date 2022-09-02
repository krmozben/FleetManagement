using FleetManagement.Application.Commands;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.State;
using FleetManagement.Domain.ShipmentAggregate.Type;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.DeliveryPoint
{
    public class Branch : Implementation.DeliveryPoint
    {
        private ILogger<Branch> _logger;
        public Branch(IMediator mediator, ILogger<Branch> logger) : base(mediator)
        {
            _logger = logger;
        }

        public override async Task<List<Model.Response.Delivery>> Unloaded(List<Shipment> shipments)
        {
            List<Model.Response.Delivery> respone = new();

            shipments.ForEach(shipment =>
            {
                if (shipment.DeliveryPoint.Id == (int)Constants.DeliveryPoint.Branch && shipment.ShipmentType == ShipmentType.Package && shipment.PackageSack == null)
                    shipment.SetState(ShipmentState.Unloaded);
                else
                    _logger.LogWarning(new EventId(1234), $"The delivery point({Constants.DeliveryPoint.Branch.ToString()}) and the shipment delivery point ({shipment.DeliveryPoint.Name}) do not match. Shipment Barcode: {shipment.Barcode}");

                respone.Add(new() { Barcode = shipment.Barcode, State = (int)shipment.ShipmentState });
            });

            await _mediator.Send(new SetShipmentState() { Shipments = shipments });

            return respone;
        }
    }
}
