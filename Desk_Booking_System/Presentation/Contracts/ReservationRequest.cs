namespace Presentation.Contracts
{
    public sealed record ReservationRequest(Guid DeskId, Guid UserId, 
        DateTime StartDate, DateTime EndDate);
}
