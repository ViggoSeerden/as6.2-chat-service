using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatService;
using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Services;
using ChatServiceDAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Dependency Injection
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ChatServiceBusiness.Services.ChatService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddHostedService<MessageReceiver>();

// Database Context
var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => mongoClient);

// Auth0
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = Environment.GetEnvironmentVariable("Auth__Domain");
    options.Audience = Environment.GetEnvironmentVariable("Auth__Audience");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
    if (builder.Environment.IsDevelopment())
    {
        options.RequireHttpsMetadata = false;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Token validation failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token successfully validated.");
                return Task.CompletedTask;
            }
        };
    }
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("read:chats", policy => policy.Requirements.Add(new
        HasScopeRequirement("read:chats", Environment.GetEnvironmentVariable("Auth__Domain"))))
    .AddPolicy("read:chat", policy => policy.Requirements.Add(new
        HasScopeRequirement("read:chat", Environment.GetEnvironmentVariable("Auth__Domain"))))
    .AddPolicy("write:add_chat", policy => policy.Requirements.Add(new
        HasScopeRequirement("write:add_chat", Environment.GetEnvironmentVariable("Auth__Domain"))))
    .AddPolicy("write:update_chat", policy => policy.Requirements.Add(new
        HasScopeRequirement("write:update_chat", Environment.GetEnvironmentVariable("Auth__Domain"))))
    .AddPolicy("write:delete_chat", policy => policy.Requirements.Add(new
        HasScopeRequirement("write:delete_chat", Environment.GetEnvironmentVariable("Auth__Domain"))));

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// Debugging
Console.WriteLine(builder.Environment.EnvironmentName);
// Console.WriteLine(Environment.GetEnvironmentVariable("Auth__Audience"));

// Cors
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("Development", policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
    }
    else
    {
        options.AddPolicy("Production", policy =>
        {
            policy.WithOrigins("http://localhost:3000"); //temp
            policy.AllowAnyHeader();
            policy.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
        });
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder.Environment.IsDevelopment() ? "Development" : "Production");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }