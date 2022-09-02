using FleetManagement.DomainCore;

namespace FleetManagement.Domain.ShipmentAggregate
{
    public class Desi : ValueObject
    {
        public int? Value { get; private set; }

        public Desi()
        {

        }

        public Desi(int value)
        {
            Value = value;
        }
    }
}
