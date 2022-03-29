using Omagol.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Adding SignalR service
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

// add signalR endpoint
app.UseEndpoints(endpoints => {
  endpoints.MapHub<ChatRoom>("/chatroom");
});

app.MapGet("/", () => "Hello World!");

app.Run();
