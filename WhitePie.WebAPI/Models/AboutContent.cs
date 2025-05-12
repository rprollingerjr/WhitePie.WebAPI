using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhitePie.WebAPI.Models;

public class AboutContent
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = "about"; // singleton document

    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? HeroImageId { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public List<AboutSection> Sections { get; set; } = new();
}

public class AboutSection
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
}
