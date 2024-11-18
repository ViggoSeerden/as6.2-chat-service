using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Services;
using ChatServiceDAL.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ChatServiceBusiness.Services.ChatService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddHostedService<MessageReceiver>();

var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => mongoClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }