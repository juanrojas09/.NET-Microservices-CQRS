using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Domain.Common
{
    [AttributeUsage(AttributeTargets.Class,Inherited=false)]

    /**
         * Esta clase es un atributo que se va a usar para indicar el nombre de la coleccion en la base de datos
         *   de mongo
         *         */
    public class BsonCollectionAttribute:Attribute
    {

        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
