using System.Collections.Concurrent;
using Omagol.Hubs;
using Omagol.Infrastructure;
using Omagol.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var signalRCors = "signalRCORS";
// Adding SignalR service
builder.Services.AddSignalR();
builder.Services.AddCors(options => {
  options.AddPolicy(signalRCors,
                    builder => {
                      builder.WithOrigins("http://localhost:5000", "http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
});
builder.Services.AddSingleton<IGroupProvider, BasicGroupProvider>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<IStorageProvider, StorageProvider>();
builder.Services.AddTransient<IGroupIdGenerator, GuidGroupIdGenerator>();
builder.Services.AddSingleton(typeof(ICollection<>), typeof(List<>));
builder.Services.AddSingleton<IDictionary<User, Group>>(ServiceProvider => {
  return Activator.CreateInstance<Dictionary<User, Group>>();
});
builder.Services.AddSingleton<IDictionary<string, User>>(ServiceProvider => {
  return Activator.CreateInstance<Dictionary<string, User>>();
});

IProducerConsumerCollection<string> test = new ConcurrentQueue<string>();

var app = builder.Build();

app.UseCors(signalRCors);
app.UseRouting();

// add signalR endpoint
app.UseEndpoints(endpoints => {
  endpoints.MapHub<ChatRoom>("/chatroom");
  endpoints.MapHub<OmagolRoom>("/omagol");
});

app.MapGet("/", () => "Hello World!");

app.Run();