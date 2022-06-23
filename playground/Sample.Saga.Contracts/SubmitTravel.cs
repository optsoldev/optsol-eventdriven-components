namespace Sample.Saga.Contracts;

public class SubmitTravel
{
    public Guid UserId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int HotelId { get; set; }
}
