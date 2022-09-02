using FleetManagement.DomainCore;

namespace FleetManagement.Domain.ShipmentAggregate
{
    public class DeliveryPoint : Entity
    {
        public string Name { get; private set; }

        public DeliveryPoint()
        {

        }

        public DeliveryPoint(string name)
        {
            Name = name;
        }
    }
}
