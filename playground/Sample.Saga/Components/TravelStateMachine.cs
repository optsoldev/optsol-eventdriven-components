using MassTransit;
using MongoDB.Bson.Serialization.Attributes;
using Sample.Flight.Contracts;
using Sample.Hotel.Contracts.Commands;
using Sample.Hotel.Contracts.Events;
using Sample.Saga.Contracts.Events;

namespace Sample.Saga.Components
{
    public partial class TravelStateMachine : MassTransitStateMachine<TravelState>
    {
        public TravelStateMachine()
        {
            Event(() => TravelBookingSubmitted, context => context.CorrelateById(m => m.Message.CorrelationId));
            Event(() => FlightBooked, context => context.CorrelateById(m => m.Message.CorrelationId));
            Event(() => HotelBooked, context => context.CorrelateById(m => m.Message.CorrelationId));
            Event(() => HotelBookedFailed, context => context.CorrelateById(m => m.Message.CorrelationId));

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
                        context => context.Init<BookFlight>(new
                        {
                            context.Message.CorrelationId,
                            context.Message.From,
                            context.Message.To,
                            context.Message.TravelId,
                            UserId = default(Guid)
                        }))
                    .TransitionTo(FlightBookingRequested));

            During(FlightBookingRequested,
                When(FlightBooked)
                    .TransitionTo(HotelBookingRequested)
                    .Then(_ => Console.WriteLine("Flight Booked"))
                    .Then(context =>
                    {
                        context.Saga.FlightBookId = context.Message.ModelId;
                    })
                    .SendAsync(new Uri("queue:book-hotel"), context => context.Init<BookHotel>(new
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        HotelId = context.Saga.HotelId,
                        TravelId = context.Message.TravelId
                    })));

            During(HotelBookingRequested,
                When(HotelBooked)
                    .Then(_ => Console.WriteLine("Hotel Booked"))
                    .SendAsync(new Uri("queue:booking-notification"),
                    context => context.Init<BookingNotification>(new BookingNotification()
                    {
                        CorrelationId = context.Message.CorrelationId
                    }))
                    .TransitionTo(TravelBooked),
                When(HotelBookedFailed)
                    .Then(_ => Console.WriteLine("Hotel Booked Failed"))
                    .SendAsync(new Uri("queue:unbook-flight"), context => context.Init<UnbookFlight>(new
                    {
                        context.Saga.CorrelationId,
                        ModelId = context.Saga.FlightBookId,                        
                    }))
                    .Finalize());

            //Exemplo de CurrentState.
            //Exemplo de Projecao.
            //Exemplo de mais de um consumer para projecao.
        }

        public Event<ITravelBookingSubmitted> TravelBookingSubmitted { get; set; }
        public Event<FlightBooked> FlightBooked { get; set; }
        public Event<HotelBooked> HotelBooked { get; set; }
        public Event<HotelBookedFailed> HotelBookedFailed { get; set; }


        public State HotelBookingRequested { get; set; }
        public State FlightBookingRequested { get; set; }
        public State TravelBooked { get; set; }

    }

    public class TravelState : SagaStateMachineInstance,
        ISagaVersion
    {
        [BsonId]
        public Guid CorrelationId { get; set; }

        public string? CurrentState { get; set; }

        public int HotelId { get; set; }
        public Guid FlightBookId { get; set; }

        public int Version { get; set; }
    }
}
