using FleetManagement.Application.Commands;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.State;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.DeliveryPoint
{
    public class DistributionCenter : Implementation.DeliveryPoint
    {
        private ILogger<DistributionCenter> _logger;
        public DistributionCenter(IMediator mediator, ILogger<DistributionCenter> logger) : base(mediator)
        {
            _logger = logger;
        }

        public override async Task<List<Model.Response.Delivery>> Unloaded(List<Shipment> shipments)
        {
            List<Model.Response.Delivery> respone = new();

            foreach (var shipment in shipments)
            {
                if (shipment.DeliveryPoint.Id == (int)Constants.DeliveryPoint.DistributionCenter)
                    shipment.SetState(ShipmentState.Unloaded);
                else
                    _logger.LogWarning(new EventId(1234), $"The delivery point({Constants.DeliveryPoint.DistributionCenter.ToString()}) and the shipment delivery point ({shipment.DeliveryPoint.Name}) do not match. Shipment Barcode: {shipment.Barcode}");

                respone.Add(new() { Barcode = shipment.Barcode, State = (int)shipment.ShipmentState });
            }

            await _mediator.Send(new SetShipmentState() { Shipments = shipments });

            await CheckPackageSackState(shipments);

            return respone;
        }
    }
}
