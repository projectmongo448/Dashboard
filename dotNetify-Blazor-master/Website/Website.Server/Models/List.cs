using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Server
{
    public class List
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Product_Name")]
        public string Product_Name { get; set; }
        [BsonElement("Cost")]
        public Int64 Cost { get; set; }
        [BsonElement("Income")]
        public Int64 Income { get; set; }
    }
}
