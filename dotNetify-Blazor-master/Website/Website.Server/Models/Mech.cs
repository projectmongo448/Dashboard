using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Server
{
    public class Mech
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("machineId")]
        public string machineId { get; set; }
        [BsonElement("voltmean")]
        public Int64 voltmean { get; set; }
        [BsonElement("rotatemean")]
        public Int64 rotatemean { get; set; }
        [BsonElement("pressuremean")]
        public Int64 pressuremean { get; set; }
        [BsonElement("vibrationmean")]
        public Int64 vibrationmean { get; set; }
    }
}
