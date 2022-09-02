using FleetManagement.Application.Model.Request;
using FleetManagement.Application.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Application.Services.FleetManagementService.Implementation
{
    public interface IFleetManagementService
    {
        public Task<VehicleDeliveryResponse> Distribute(VehicleDeliveryRequest request);
    }
}