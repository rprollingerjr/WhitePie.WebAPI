using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhitePie.WebAPI.Models;

public class Moment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("imageId")]
    public string ImageId { get; set; }

    public int DisplayOrder { get; set; }
}
