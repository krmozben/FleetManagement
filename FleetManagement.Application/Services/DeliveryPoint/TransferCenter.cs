using FleetManagement.Application.Commands;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.State;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.DeliveryPoint
{
    public class TransferCenter : Implementation.DeliveryPoint
    {
        private ILogger<TransferCenter> _logger;
        public TransferCenter(IMediator mediator, ILogger<TransferCenter> logger) : base(mediator)
        {
            _logger = logger;
        }

        public override async Task<List<Model.Response.Delivery>> Unloaded(List<Shipment> shipments)
        {
            List<Model.Response.Delivery> respone = new();

            foreach (var shipment in shipments)
            {
                if (shipment.DeliveryPoint.Id != (int)Constants.DeliveryPoint.TransferCenter)
                {
                    _logger.LogWarning(new EventId(1234), $"The delivery point({Constants.DeliveryPoint.TransferCenter}) and the shipment delivery point ({shipment.DeliveryPoint.Name}) do not match. Shipment Barcode: {shipment.Barcode}");

                    break;
                }

                if (shipment.PackageSack != null)
                    shipment.SetState(ShipmentState.Unloaded);
                else
                    _logger.LogWarning(new EventId(1234), $"Only sacks and packages in sacks can be left at this delivery point. Shipment Barcode: {shipment.Barcode}, Shipment Type: {shipment.ShipmentType}");

                respone.Add(new() { Barcode = shipment.Barcode, State = (int)shipment.ShipmentState });
            }

            await _mediator.Send(new SetShipmentState() { Shipments = shipments });

            await CheckPackageSackState(shipments);

            return respone;
        }
    }
}
