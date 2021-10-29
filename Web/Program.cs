using Business.Services;

var builder = WebApplication.CreateBuilder(args);
// builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false);

// Add services to the container.
builder.Services.AddTransient<IImageReader, ImageReader>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();