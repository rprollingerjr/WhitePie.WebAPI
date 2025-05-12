using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace WhitePie.WebAPI.Models;

public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; set; }

    public string EventTitle { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Description {  get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string? TicketUrl { get; set; }
    public string? ImageId { get; set; }
    public bool IsEdibleMamiHosted { get; set; }
    [BsonIgnore]
    public EventTimeInfo EventTimeInfo => new EventTimeInfo(StartTime, EndTime);
}


public class EventTimeInfo
{
    public string AbbreviatedMonth { get; }
    public string DayOfTheMonth { get; }
    public string StartTime { get; }
    public string EndTime { get; }
    public string Year { get; }

    public EventTimeInfo(DateTime start, DateTime end)
    {
        AbbreviatedMonth = start.ToString("MMM");
        DayOfTheMonth = start.Day.ToString();
        Year = start.Year.ToString();
        StartTime = start.ToString("h:mm tt");
        EndTime = end.ToString("h:mm tt");
    }
}
