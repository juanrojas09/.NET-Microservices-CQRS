using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Infrastructure;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configure kafka settings
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

//inicializo la comunicacion entre el objeto command y el command handler, no hace falta en todos los commands, solo con una
builder.Services.AddMediatR(typeof(OpenAccountCommand).Assembly);

//inicializo los servicios de infraestructura donde inyecte las dependencias y los ciclos de vida de las interfaces con clases concretas
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
               builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                                     .AllowAnyHeader());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
app.UseCors("CorsPolicy");  
app.MapControllers();

app.Run();
