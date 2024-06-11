using MongoDB;
using MongoDB.Driver;
using MongoDB.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PadreDbSettings>(builder.Configuration.GetSection("padreDbSettings"));
builder.Services.AddSingleton<PadreServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/padres", async (PadreServices padreServices) =>
{
    var padres = await padreServices.GetAsync();

    return padres;
});

app.MapGet("/padres/{id}", async (PadreServices padreServices, string id) =>
{
    var padre = await padreServices.GetAsync(id);

    return padre is null ? Results.NotFound() : Results.Ok(padre);
});

app.MapPost("/padres", async (PadreServices padreServices, Padre padre) =>
{
    await padreServices.CreateAsync(padre);

    return padre;

   
});

app.MapPut("/padres/{id}", async (PadreServices padreServices, string id, Padre padre) =>
{
    return await padreServices.UpdateAsync(id, padre);

});


app.MapDelete("/padres/{id}", async (PadreServices padreServices, string id) =>
{
   
    return await padreServices.DeleteAsync(id);

});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
