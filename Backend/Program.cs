using Omagol.Hubs;

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

var app = builder.Build();

app.UseCors(signalRCors);
app.UseRouting();

// add signalR endpoint
app.UseEndpoints(endpoints => {
  endpoints.MapHub<ChatRoom>("/chatroom");
});

app.MapGet("/", () => "Hello World!");

app.Run();
