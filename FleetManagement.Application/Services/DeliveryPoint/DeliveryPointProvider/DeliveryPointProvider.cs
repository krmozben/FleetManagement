using FleetManagement.Application.Model.Request;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FleetManagement.Application.Services.DeliveryPoint.DeliveryPointProvider
{
    public class DeliveryPointProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DeliveryPointProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Implementation.DeliveryPoint DeliveryPointBuilder(Route route)
        {
            Implementation.DeliveryPoint deliveryPoint = null;
            switch (route.DeliveryPoint)
            {
                case (int)Constants.DeliveryPoint.Branch:
                    deliveryPoint = _serviceProvider.GetRequiredService<Branch>();
                    break;
                case (int)Constants.DeliveryPoint.DistributionCenter:
                    deliveryPoint = _serviceProvider.GetRequiredService<DistributionCenter>();
                    break;
                case (int)Constants.DeliveryPoint.TransferCenter:
                    deliveryPoint = _serviceProvider.GetRequiredService<TransferCenter>();
                    break;
            }
            return deliveryPoint;
        }
    }
}