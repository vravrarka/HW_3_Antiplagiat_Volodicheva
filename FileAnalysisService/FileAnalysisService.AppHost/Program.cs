using FileAnalysisService.Services;
using FileAnalysisService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AnalysisAppService>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/analysis/analyze", (AnalysisRequest request, AnalysisAppService service) =>
{
    var report = service.Analyze(request);
    return Results.Ok(report);
});

app.MapGet("/api/analysis/reports/{assignmentId}", (string assignmentId, AnalysisAppService service) =>
{
    var reports = service.GetReportsByAssignment(assignmentId);
    return Results.Ok(reports);
});

app.Run();