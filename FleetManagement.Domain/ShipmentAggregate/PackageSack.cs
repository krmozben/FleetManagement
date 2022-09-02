using FleetManagement.Domain.ShipmentAggregate.State;
using FleetManagement.DomainCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Domain.ShipmentAggregate
{
    public class PackageSack : Entity
    {
        [ForeignKey("PackageId")]
        public Shipment Package { get; private set; }
        [ForeignKey("SackId")]
        public Shipment Sack { get; private set; }

        public PackageSack()
        {
        }

        public PackageSack(Shipment sack)
        {
            Sack = sack;
        }
    }
}
