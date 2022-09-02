using FleetManagement.Application.Model.Request;
using FleetManagement.Application.Model.Response;
using FleetManagement.Application.Services.DeliveryPoint;
using FleetManagement.Application.Services.DeliveryPoint.DeliveryPointProvider;
using FleetManagement.Application.Services.FleetManagementService;
using FleetManagement.Application.Services.FleetManagementService.Implementation;
using FleetManagement.Domain.ShipmentAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FleetManagement.Test
{
    [TestClass]
    public class FleetManagementTest
    {
        [TestMethod]
        public void DeliveryPointProvider_Branch_Test()
        {
            var deliveriPoint = new DeliveryPoint("Branch");
            deliveriPoint.Id = 1;

            var shipment1 = new Shipment("P7988000121", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(5));
            var shipment2 = new Shipment("P7988000122", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(5));

            var _mediator = Substitute.For<IMediator>();
            var _logger = Substitute.For<ILogger<Branch>>();
            var branch = new Branch(_mediator, _logger);

            var response = branch.Unloaded(new List<Shipment> { shipment1, shipment2 }).Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeliveryPointProvider_DistributionCenter_Test()
        {
            var deliveriPoint = new DeliveryPoint("Distribution Center");
            deliveriPoint.Id = 2;

            var shipmentsack = new Shipment("C725799", Domain.ShipmentAggregate.Type.ShipmentType.Sack, deliveriPoint, null);
            var shipment1 = new Shipment("P8988000124", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(9));
            var shipment2 = new Shipment("P8988000125", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(33));

            shipment1.LoadedIntoSack(new PackageSack(shipmentsack));
            shipment2.LoadedIntoSack(new PackageSack(shipmentsack));

            var _mediator = Substitute.For<IMediator>();
            var _logger = Substitute.For<ILogger<DistributionCenter>>();
            var distributionCenter = new DistributionCenter(_mediator, _logger);

            var response = distributionCenter.Unloaded(new List<Shipment> { shipment1, shipment2 }).Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeliveryPointProvider_TransferCenter_Test()
        {
            var deliveriPoint = new DeliveryPoint("Transfer Center");
            deliveriPoint.Id = 3;

            var shipment1 = new Shipment("P9988000128", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(5));
            var shipment2 = new Shipment("P9988000129", Domain.ShipmentAggregate.Type.ShipmentType.Package, deliveriPoint, new Desi(5));

            var _mediator = Substitute.For<IMediator>();
            var _logger = Substitute.For<ILogger<TransferCenter>>();
            var transferCenter = new TransferCenter(_mediator, _logger);

            var response = transferCenter.Unloaded(new List<Shipment> { shipment1, shipment2 }).Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void FleetManagementTest_Distribute_Test()
        {
            var request = new VehicleDeliveryRequest();
            request.Vehicle = "34 TL 34";
            request.Route = new List<Application.Model.Request.Route>
            {
                new Application.Model.Request.Route
                {
                        DeliveryPoint = 1,
                        Deliveries= new()
                        {
                            new() { Barcode = "P7988000121" },
                            new() { Barcode = "P7988000122" },
                            new() { Barcode = "P7988000123" },
                            new() { Barcode = "P8988000121" },
                            new() { Barcode = "C725799" }
                        }
                },
                new Application.Model.Request.Route
                {
                        DeliveryPoint = 2,
                        Deliveries= new()
                        {
                            new() { Barcode = "P8988000123" },
                            new() { Barcode = "P8988000124" },
                            new() { Barcode = "P8988000125" },
                            new() { Barcode = "C725799" }
                        }
                },
                 new Application.Model.Request.Route
                {
                        DeliveryPoint = 1,
                        Deliveries= new()
                        {
                            new() { Barcode = "P9988000126" },
                            new() { Barcode = "P9988000127" },
                            new() { Barcode = "P9988000128" },
                            new() { Barcode = "P9988000129" },
                            new() { Barcode = "P9988000130" }
                        }
                }
            };

            var _serviceProvider = Substitute.For<IServiceProvider>();
            var _deliveryPointProvider = new DeliveryPointProvider(_serviceProvider);
            var _mediator = Substitute.For<IMediator>();
            var _logger = Substitute.For<ILogger<FleetManagementService>>();

            IFleetManagementService service = new FleetManagementService(_deliveryPointProvider, _mediator, _logger);

            var response = service.Distribute(request);

            Assert.IsNotNull(response);
        }
    }
}