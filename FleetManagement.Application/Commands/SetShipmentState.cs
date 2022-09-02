using FleetManagement.Domain.ShipmentAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Application.Commands
{
    public class SetShipmentState : IRequest
    {
        public List<Shipment> Shipments { get; set; }
    }
}
