using FleetManagement.Domain.ShipmentAggregate.State;
using FleetManagement.Domain.ShipmentAggregate.Type;
using FleetManagement.DomainCore;

namespace FleetManagement.Domain.ShipmentAggregate
{
    public class Shipment : Entity, IAggregateRoot
    {
        public ShipmentType ShipmentType { get; private set; }
        public ShipmentState ShipmentState { get; private set; }
        public string Barcode { get; private set; }
        public DeliveryPoint DeliveryPoint { get; private set; }
        public PackageSack PackageSack { get; private set; }
        public Desi Desi { get; private set; }

        public Shipment()
        {

        }

        public Shipment(string barcode, ShipmentType shipmentType, DeliveryPoint deliveryPoint, Desi desi)
        {
            Barcode = barcode;
            ShipmentType = shipmentType;
            DeliveryPoint = deliveryPoint;
            Desi = desi;
            ShipmentState = ShipmentState.Created;
        }

        public void LoadedIntoSack(PackageSack packageSack)
        {
            if (packageSack.Sack.DeliveryPoint != DeliveryPoint)
                return;

            PackageSack = packageSack;
            ShipmentState = ShipmentState.LoadedIntoSack;
        }

        public void SetState(ShipmentState state) => ShipmentState = state;
    }
}