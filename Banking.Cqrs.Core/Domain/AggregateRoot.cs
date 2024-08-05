using Banking.Cqrs.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Cqrs.Core.Domain
{
    public abstract class AggregateRoot
    {
        public string Id { get; set; }=string.Empty;

        public int version { get; set; } = -1;

        //eventos no almacenados en la db en mongo ni enviados en el bus de eventos o kafka
        List<BaseEvent> changes=new List<BaseEvent>();


        public int GetVersion()
        {
            return version;
        }

        public void SetVersion(int version)
        {
            this.version = version;
        }

        public List<BaseEvent> GetUncommittedChanges()
        {
            return changes;
        }

        public void MarkChangesAsCommitted()
        {
            changes.Clear();
        }

        public void ApplyChanges(BaseEvent @event,bool isNewEvent)
        {
            try
            {
                var EventClassType=@event.GetType();

                //indicar referencia al metodo a ejecutar, el apply lo va a ejecutar un hijo de aggregateRoot
                //por eso se usa GetType() para obtener el tipo de la clase hija
                //y se va a diferenciar por el tipo de evento que se esta aplicando
                //importante entender que la refrencia a la clase hija q se hace es en tiempo de ejecucion
                var method =GetType().GetMethod("Apply", new [] { EventClassType });
                method.Invoke(this, new object[] { @event });

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (isNewEvent)
                {
                    changes.Add(@event);
                }
            }
           
        }

        //utils

        //agregacion de un nuevo evento
        public void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }

        //re ejecutar eventos q ya existen
        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChanges(@event, false);
            }   
        }


    }
}
