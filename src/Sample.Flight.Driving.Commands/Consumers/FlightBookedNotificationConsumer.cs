using MassTransit;
using Sample.Flight.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Flight.Driving.Commands.Consumers
{
    public class FlightBookedNotificationConsumer : IConsumer<IFlightBooked>
    {
        public Task Consume(ConsumeContext<IFlightBooked> context)
        {
            throw new NotImplementedException();
        }
    }
}
