namespace Sample.Saga.Contracts.Commands;

public class SubmitTravel
{
    public Guid UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int HotelId { get; set; }

    public DateTime Departure { get; set; }
}

