using FleetManagement.Application.Commands;
using FleetManagement.Application.Model.Request;
using FleetManagement.Application.Model.Response;
using FleetManagement.Application.Queries;
using FleetManagement.Application.Services.DeliveryPoint.DeliveryPointProvider;
using FleetManagement.Application.Services.FleetManagementService.Implementation;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.State;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.FleetManagementService
{
    public class FleetManagementService : IFleetManagementService
    {
        private readonly DeliveryPointProvider _deliveryPointProvider;
        private readonly IMediator _mediator;
        private readonly ILogger<FleetManagementService> _logger;

        public FleetManagementService(DeliveryPointProvider deliveryPointProvider, IMediator mediator, ILogger<FleetManagementService> logger)
        {
            _deliveryPointProvider = deliveryPointProvider;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<VehicleDeliveryResponse> Distribute(VehicleDeliveryRequest request)
        {
            VehicleDeliveryResponse response = new();
            response.Vehicle = request.Vehicle;

            _logger.LogInformation(new EventId(1234), $"Vehicle starts delivery");

            foreach (var route in request.Route)
            {
                var loadedShipments = await ShipmentLoaded(route.Deliveries);

                var deliveryPoint = _deliveryPointProvider.DeliveryPointBuilder(route);

                var deliveries = await deliveryPoint.Unloaded(loadedShipments);

                response.Route.Add(new Model.Response.Route { Deliveries = deliveries, DeliveryPoint = route.DeliveryPoint });
            }

            return response;
        }

        private async Task<List<Shipment>> ShipmentLoaded(List<Model.Request.Delivery> deliveries)
        {
            var shipments = await _mediator.Send(new GetShipmentsByBarcodes() { Barcodes = deliveries.Select(x => x.Barcode).ToList() });

            shipments.ForEach(x => x.SetState(ShipmentState.Loaded));

            await _mediator.Send(new SetShipmentState() { Shipments = shipments });

            return shipments;
        }
    }
}
