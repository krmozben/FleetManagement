using FleetManagement.Application.Commands;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Infrastructure.Repositories.Base;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManagement.Application.Handlers.CommandHandlers
{
    public class SetShipmentStateHandler : IRequestHandler<SetShipmentState>
    {
        private readonly IRepository<Shipment> _shipmentRepository;

        public SetShipmentStateHandler(IRepository<Shipment> shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<Unit> Handle(SetShipmentState request, CancellationToken cancellationToken)
        {
            await _shipmentRepository.BulkUpdateAsync(request.Shipments);

            await _shipmentRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
