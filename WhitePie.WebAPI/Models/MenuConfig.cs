using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhitePie.WebAPI.Models;

public class MenuConfig
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = "menu-config";

    public string Caption { get; set; } = string.Empty;
    public string FooterNote { get; set; } = string.Empty;

    public List<MenuTheme> Themes { get; set; } = new();
}

public class MenuTheme
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<MenuItem> Items { get; set; } = new();
    public List<string> EventIds { get; set; } = new();
}

public class MenuItem
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string? ImageId { get; set; }
}
