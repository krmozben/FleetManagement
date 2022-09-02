using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Domain.ShipmentAggregate.Type;
using FleetManagement.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Application.FleetContextSeed
{
    public class FleetContextSeed
    {
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly ILogger<FleetContextSeed> _logger;

        public FleetContextSeed(IRepository<Shipment> shipmentRepository, ILogger<FleetContextSeed> logger)
        {
            _shipmentRepository = shipmentRepository;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _shipmentRepository.MigrateDatabaseAsync();

            var anyShipment = await _shipmentRepository.GetAsync(x => true);

            if (anyShipment.ToList().Any())
                return;

            await _shipmentRepository.AddRangeAsync(GetSeedData());

            _logger.LogInformation(new EventId(1234), $"The initial data for the business logic is created.");
        }

        private List<Shipment> GetSeedData()
        {
            DeliveryPoint dp1 = new("Branch");
            DeliveryPoint dp2 = new("Distribution Center");
            DeliveryPoint dp3 = new("Transfer Center");

            Shipment s1 = new("C725799", ShipmentType.Sack, dp2, null);
            Shipment s2 = new("C725800", ShipmentType.Sack, dp3, null);

            Shipment ps1 = new("P9988000128", ShipmentType.Package, dp3, new(55));
            ps1.LoadedIntoSack(new(s2));
            Shipment ps2 = new("P9988000129", ShipmentType.Package, dp3, new(28));
            ps2.LoadedIntoSack(new(s2));
            Shipment ps3 = new("P8988000126", ShipmentType.Package, dp2, new(50));
            ps3.LoadedIntoSack(new(s1));
            Shipment ps4 = new("P8988000122", ShipmentType.Package, dp2, new(26));
            ps4.LoadedIntoSack(new(s1));

            var shipmentList = new List<Shipment>()
            {
                new("P7988000121",ShipmentType.Package,dp1,new(5)),
                new("P7988000122",ShipmentType.Package,dp1,new(5)),
                new("P7988000123",ShipmentType.Package,dp1,new(9)),
                new("P8988000120",ShipmentType.Package,dp2,new(33)),
                new("P8988000121",ShipmentType.Package,dp2,new(17)),
                new("P8988000123",ShipmentType.Package,dp2,new(35)),
                new("P8988000124",ShipmentType.Package,dp2,new(1)),
                new("P8988000125",ShipmentType.Package,dp2,new(200)),
                new("P9988000126",ShipmentType.Package,dp3,new(15)),
                new("P9988000127",ShipmentType.Package,dp3,new(16)),
                new("P9988000130",ShipmentType.Package,dp3,new(17))
            };

            shipmentList.AddRange(new[] { s1, s2, ps1, ps2, ps3, ps4 });

            return shipmentList;
        }
    }
}
