using Banking.Cqrs.Core.Events;


namespace Banking.Cqrs.Core.Producers
{
    public interface IEventProducer
    {
        //topic es el channel o donde fluiran los eventos
        //@event se pone el arroba pq event es una palabra reservada
        void Produce(string topic, BaseEvent @event);
    }
}