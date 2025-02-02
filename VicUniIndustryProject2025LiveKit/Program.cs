using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using VicUniIndustryProject2025LiveKit;
using VicUniIndustryProject2025LiveKit.EditableModels;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<ChatService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseAzureSql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ChatDbContext>();


//IN MEMORY TO TEST FIRST
var employees = new List<Employee>
            {
                new Employee(4, "Tan", "tan@gmail.com"),
                new Employee(2, "Bob", "bob@example.com"),
                new Employee(1, "Alice", "alice@example.com"),
                new Employee(3, "Charlie", "charlie@example.com")
            };

var products = new List<Product>
{
    new Product(1, "MacBook", "Brand new MacBook that is slick but weak"),
    new Product(2, "ThinkPad", "Old but sturdy laptop that look hideous")
};



var app = builder.Build();

// Configure Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Console.WriteLine("Running");

// Preload employees data at startup
app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var chatService = scope.ServiceProvider.GetRequiredService<ChatService>();
    await chatService.SeedDatabaseAsync();
});

// --------------------------------------------------------------------
// Sample "WeatherForecast" endpoint (kept from template)
// --------------------------------------------------------------------
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
    "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
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



// --------------------------------------------------------------------
// New Visitor Management Endpoints
// --------------------------------------------------------------------

app.MapGet("/products", async (ChatService chatService) =>
{
    return products;
    return await chatService.GetAllProductAsync();
})
.WithName("Get All Products")
.WithOpenApi();

app.MapGet("/employees", async (ChatService chatService) =>
{
    //return employees;
    return await chatService.GetAllEmployeesAsync();
})
.WithName("GetAllEmployees")
.WithOpenApi();


app.MapGet("/visitors", async (ChatService chatService) =>
{
    return await chatService.GetAllVisitorsAsync();
})
.WithName("GetAllVisitors")
.WithOpenApi();

app.MapGet("/on_site", async (ChatService chatService) =>
{
    return await chatService.GetAllOnSiteAsync();
})
.WithName("GetAllOnSite")
.WithOpenApi();


app.MapGet("/visitors/on-site", async (ChatService chatService) =>
{
    return await chatService.GetVisitorsOnSiteAsync();
})
.WithName("GetVisitorsOnSite")
.WithOpenApi();



app.MapPost("/visitors/arrive-meeting", async (ChatService chatService, VisitorArriveMeetingRequest request) =>
{
    var result = await chatService.VisitorArriveMeetingAsync(request);

    if (!result.Approved)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Ok(result.Message);
})
.WithName("VisitorArriveMeeting")
.WithOpenApi();


app.MapPost("/visitors/arrive-courier", async (ChatService chatService, VisitorArriveCourierRequest request) =>
{
    var result = await chatService.VisitorArriveCourierAsync(request);

    if (!result.Approved)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Ok(result.Message);

})
.WithName("VisitorArriveCourier")
.WithOpenApi();


app.MapPost("/visitors/arrive-contractor", async (ChatService chatService, VisitorArriveContractorRequest request) =>
{
    var result = await chatService.VisitorArriveContractorAsync(request);

    if (!result.Approved)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Ok(result.Message);
})
.WithName("VisitorArriveContractor")
.WithOpenApi();



app.MapPost("/visitors/sign-out", async (ChatService chatService, VisitorSignOutRequest request) =>
{
    var result = await chatService.VisitorSignOutAsync(request);

    if (!result.Approved)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Ok(result.Message);
})
.WithName("VisitorSignOut")
.WithOpenApi();

app.Run();






// --------------------------------------------------------------------
// Supporting WeatherForecast record (from template)
// --------------------------------------------------------------------
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}








// --------------------------------------------------------------------
// Entities
// --------------------------------------------------------------------

//RECORD ONLY DATA GOES HERE
public record Employee(int Id, string Name, string Email);
public record OnSite(int Id, string Name, string Type);






// --------------------------------------------------------------------
// Request DTOs for inbound data
// --------------------------------------------------------------------

/// <summary>
/// Request to sign in a visitor who has come for a meeting.
/// </summary>
public record VisitorArriveMeetingRequest(
    string VisitorName,
    string MeetingWith
);

/// <summary>
/// Request to sign in a courier.
/// </summary>
public record VisitorArriveCourierRequest(
    string CourierName
);

/// <summary>
/// Request to sign in a contractor.
/// </summary>
public record VisitorArriveContractorRequest(
    string VisitorName,
    string Company
);

/// <summary>
/// Request to sign out an existing visitor by ID.
/// </summary>
public record VisitorSignOutRequest(
    int VisitorId
);

public record OnSiteRequest(
    string Name
);
