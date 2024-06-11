using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDB.Models
{
    public class Padre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? nombre { get; set; }
        public string? tipoVivienda { get; set; }

        [BsonElement("cantHijos")]
        public int  cantidadHijos { get; set; }
        public bool recibeAyuda { get; set; }
        public string? telefono { get; set; }
    }
}