namespace WhitePie.WebAPI.DTOs;

public class EventCreateDto
{
    public string EventTitle { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? TicketUrl { get; set; }
}
