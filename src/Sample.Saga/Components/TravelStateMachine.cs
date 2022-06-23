﻿using MassTransit;
using Sample.Saga.Contracts;

using Sample.Hotel.Contracts.Commands;
using Sample.Flight.Contracts.Commands;
using Sample.Flight.Contracts.Events;

namespace Sample.Saga.Components
{
    public partial class TravelStateMachine : MassTransitStateMachine<TravelState>
    {
        public TravelStateMachine()
        {
            Event(() => TravelBookingSubmitted, context => context.CorrelateById(m => m.Message.CorrelationId));
            Event(() => FlightBooked, context => context.CorrelateById(m => m.Message.CorrelationId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(TravelBookingSubmitted)
                .Then(context =>
                {
                    Console.WriteLine("TravelBookingSubmited");                    
                })
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.HotelId = context.Message.HotelId;                    
                })
                .SendAsync(new Uri("queue:book-flight"), 
                    context => context.Init<IBookFlight>(new
                    {
                        context.Message.CorrelationId,
                        context.Message.From,
                        context.Message.To,
                        context.Message.Departure,
                        context.Message.TravelId
                    }))
                .TransitionTo(FlightBookingRequested));

            During(FlightBookingRequested,
                When(FlightBooked)
                .Then(_ => Console.WriteLine("Flight Booked"))
                .SendAsync(new Uri("queue:book-hotel"), context => context.Init<IBookHotel>(new
                {
                    context.Saga.HotelId,
                    context.Message.TravelId
                }))
                    .TransitionTo(HotelBookingRequested));
        }

        public Event<ITravelBookingSubmitted> TravelBookingSubmitted { get; set; }
        public Event<IFlightBooked> FlightBooked { get; set; }

        public State HotelBookingRequested { get; set; }
        public State FlightBookingRequested { get; set; }
        public State Finalized { get; set; }

    }

    public class TravelState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public Guid HotelId { get; set; }

        public long Version { get; set; }
    }
}
