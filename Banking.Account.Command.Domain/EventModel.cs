using Banking.Account.Command.Domain.Common;
using Banking.Cqrs.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Domain
{
    [BsonCollection("eventStore")]
    public class EventModel : Document
    {
        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("aggregateIdentifier")]
        public string AggregateIdentifier { get; set; } = string.Empty;

        [BsonElement("aggregateType")]
        public string AggregateType { get; set; } = string.Empty;

        [BsonElement("version")]
        public int version { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; } = string.Empty;

        [BsonElement("eventData")]
        public BaseEvent? Events { get; set; }
    }
}
