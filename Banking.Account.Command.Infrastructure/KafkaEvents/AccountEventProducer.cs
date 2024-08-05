using Banking.Account.Command.Application.Models;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Infrastructure.KafkaEvents
{
    public class AccountEventProducer : IEventProducer
    {
        public KafkaSettings _kafkaSettings;

        public AccountEventProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
        }
        public void Produce(string topic, BaseEvent @event)
        {
            try
            {
                //sirve para configurar el productor de eventos de kafka
                var config =new ProducerConfig { 
                    BootstrapServers = $"{_kafkaSettings.HostName}:{_kafkaSettings.Port}" 
                
                };
                //para inicializar el contexto de kafka, para poder enviar mensajes
                using (var producer=new ProducerBuilder<Null,string>(config).Build()    )
                {
                    //aca podre usar el producer
                    //primero le debo pasar el nombre del topico y el mensaje que quiero enviar
                    var classEvent = @event.GetType();
                    var serializedEvent = Newtonsoft.Json.JsonConvert.SerializeObject(@event);
                    var message=new Confluent.Kafka.Message<Null, string> { Value = serializedEvent };
                    producer.ProduceAsync(topic, message).GetAwaiter().GetResult();
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
