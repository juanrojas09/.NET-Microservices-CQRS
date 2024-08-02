
namespace Banking.Cqrs.Core.Messages
{
    public abstract class Message
    {
        public string Id { get;set; }

        public Message(string id)
        {
            this.Id = id;
        }
    }
}
