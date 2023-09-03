using Omagol.Hubs;
using Omagol.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

const string signalRCors = "signalRCORS";
// Adding SignalR service
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
  options.AddPolicy(signalRCors,
                    builder =>
                    {
                      builder
                        .SetIsOriginAllowed(_ => true)
                        // .WithOrigins("http://localhost:5000", "http://localhost:3000")
                        // .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
});
builder.Services.AddSingleton<IGroupProvider, BasicGroupProvider>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<IStorageFactory, StorageFactory>();
builder.Services.AddTransient<IGroupIdGenerator, GuidGroupIdGenerator>();
builder.Services.AddSingleton(typeof(ICollection<>), typeof(List<>));
builder.Services.AddSingleton(typeof(IDictionary<,>), typeof(Dictionary<,>));

var app = builder.Build();

app.UseCors(signalRCors);
app.UseRouting();

// add signalR endpoint
app.MapHub<ChatRoom>("/chatroom");
app.MapHub<OmagolRoom>("/omagol");

app.MapGet("/", () => "The webapplication is up and running.");

app.Run();