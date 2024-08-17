using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Application.Models;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Repositories;
using Banking.Cqrs.Core.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Banking.Account.Query.Infrastructure.Consumers
{
    public class BankAccountConsumerService : IHostedService
    {

        private readonly IBankAccountRepository bankAccountRepository;
        //credenciales de acceso a mi apache
        public KafkaSettings KafkaSettings { get; }

        

        public BankAccountConsumerService(IServiceScopeFactory factory)
        {
            //inyecto el objeto
            bankAccountRepository=factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();
            KafkaSettings = factory.CreateScope().ServiceProvider.GetRequiredService<IOptions<KafkaSettings>>().Value;

        }

        /// <summary>
        ///logica para poder llamar a un topic q continee los mensajes
        ///no solo leeer, sino mapear y persistir en la base de datos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //configuracion del consumidor
            var config =new ConsumerConfig { 
            GroupId= KafkaSettings.GroupId,
             BootstrapServers = $"{KafkaSettings.HostName}:{KafkaSettings.Port}",
             AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {

                using(var consumerBuilder=new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    //determino los topicos
                    var bankTopics = new string[]
                    {
                        typeof(AccountOpenedEvent).Name,
                        typeof(AccountClosedEvent).Name,
                        typeof(FundsDepositedEvent).Name,
                        typeof(FundsWithdrawnEvent).Name
                    };

                    //me suscribo a los topicos, para poder leer los mensajes
                    consumerBuilder.Subscribe(bankTopics);
                    var cancelToken=new CancellationTokenSource();

                    try
                    {
                        //leer ctamente la data
                        while (true)
                        {
                            //se puede refactorizar...

                           var consumer=consumerBuilder.Consume(cancelToken.Token);
                            if (consumer.Topic == typeof(AccountOpenedEvent).Name)
                            {
                                //data del topic a persistir, hay q deserializarlo
                                var accountOpenedEvent=JsonConvert.DeserializeObject<AccountOpenedEvent>(consumer.Message.Value);
                                
                                var bankAccount = new BankAccount
                                {
                                    AccountHolder = accountOpenedEvent.AccountHolder,
                                    Balance = accountOpenedEvent.OpeningBalance,
                                    Identifier = accountOpenedEvent.Id,
                                    creationDate = accountOpenedEvent.CreatedDate
                                };

                                bankAccountRepository.AddAsync(bankAccount);

                            }


                         
                            if (consumer.Topic == typeof(AccountClosedEvent).Name)
                            {
                                //data del topic a persistir, hay q deserializarlo
                                var accountClosedEvent = JsonConvert.DeserializeObject<AccountClosedEvent>(consumer.Message.Value);                                  

                                bankAccountRepository.DeleteByIdentifier(accountClosedEvent.Id).Wait();
                            }

                            if (consumer.Topic == typeof(FundsDepositedEvent).Name)
                            {
                                //data del topic a persistir, hay q deserializarlo
                                var fundsDepositedEvent = JsonConvert.DeserializeObject<FundsDepositedEvent>(consumer.Message.Value);

                                var bankAccount = new BankAccount
                                {
                                    Identifier = fundsDepositedEvent.Id,
                                    Balance = fundsDepositedEvent.Amount
                                };

                                bankAccountRepository.DepositBankAccountByIdentifier(bankAccount).Wait();
                            }



                            if (consumer.Topic == typeof(FundsWithdrawnEvent).Name)
                            {
                                //data del topic a persistir, hay q deserializarlo
                                var fundsWithdrawnEvent = JsonConvert.DeserializeObject<FundsWithdrawnEvent>(consumer.Message.Value);

                                var bankAccount = new BankAccount
                                {
                                    Identifier = fundsWithdrawnEvent.Id,
                                    Balance = fundsWithdrawnEvent.Amount
                                };

                                bankAccountRepository.WithdrawnBankAccountByIdentifier(bankAccount).Wait();
                            }   

                        }

                    }catch (OperationCanceledException ex)
                    {
                       consumerBuilder.Close();

                    }
                }

            }catch (Exception  ex)
            {
               System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
