using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorageService.Models;
using FileStorageService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileStorageAppService>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/files/upload", async (IFormFile file, FileStorageAppService service, CancellationToken ct) =>
{
    try
    {
        var response = await service.SaveFileAsync(file, ct);
        return Results.Ok(response);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.DisableAntiforgery(); ;

app.Run();