using ARMExplorer.App_Start;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSystemWebAdapters();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSystemWebAdapters();
app.MapControllers();
WebApiConfig.Register(app);
app.Run();