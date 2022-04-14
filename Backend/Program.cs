using Omagol.Hubs;
using Omagol.Infrastructure;

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
builder.Services.AddSingleton<IGroupProvider, GroupProvider>();

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