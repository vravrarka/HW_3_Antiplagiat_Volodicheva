using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using APIGateway.Models;
using Microsoft.Graph;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("FileStorage", client =>
{
    client.BaseAddress = new Uri("http://localhost:8081");
});
builder.Services.AddHttpClient("FileAnalysis", client =>
{
    client.BaseAddress = new Uri("http://localhost:8082");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//отправляем на проверку
app.MapPost("/api/works/submit", async ([FromForm] WorkSubmitRequest req, IHttpClientFactory httpClientFactory) =>
{
    var file = req.File;
    var studentId = req.StudentId;
    var assignmentId = req.AssignmentId;
    if (file is null || file.Length == 0)
        return Results.BadRequest("File is required");
    if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(assignmentId))
        return Results.BadRequest("studentId and assignmentId are required");

    var fileStorageClient = httpClientFactory.CreateClient("FileStorage");
    using var content = new MultipartFormDataContent();
    var fileContent = new StreamContent(file.OpenReadStream());
    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
    content.Add(fileContent, "file", file.FileName);
    var fileResp = await fileStorageClient.PostAsync("/api/files/upload", content);
    if (!fileResp.IsSuccessStatusCode)
    {
        return Results.Problem(
            title: "File storage error",
            detail: $"Status code: {(int)fileResp.StatusCode}",
            statusCode: 503
        );
    }

    var uploadResponse = await fileResp.Content.ReadFromJsonAsync<FileUploadResponse>();
    if (uploadResponse is null)
        return Results.Problem("Invalid response from FileStorageService", statusCode: 502);

    var fileAnalysisClient = httpClientFactory.CreateClient("FileAnalysis");

    var analysisRequest = new AnalysisRequest(
        StudentId: studentId,
        AssignmentId: assignmentId,
        FileId: uploadResponse.FileId,
        FileHash: uploadResponse.FileHash
    );

    var analysisResp = await fileAnalysisClient.PostAsJsonAsync("/api/analysis/analyze", analysisRequest);
    if (!analysisResp.IsSuccessStatusCode)
    {
        return Results.Problem(
            title: "Analysis service error",
            detail: $"Status code: {(int)analysisResp.StatusCode}",
            statusCode: 503
        );
    }

    var report = await analysisResp.Content.ReadFromJsonAsync<WorkReport>();
    if (report is null)
        return Results.Problem("Invalid response from AnalysisService", statusCode: 502);

    return Results.Ok(report);
})
.DisableAntiforgery();



app.MapGet("/api/works/{assignmentId}/reports", async (string assignmentId, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("FileAnalysis");
    var resp = await client.GetAsync($"/api/analysis/reports/{assignmentId}");

    if (!resp.IsSuccessStatusCode)
    {
        return Results.Problem(
            title: "Analysis service error",
            detail: $"Status code: {(int)resp.StatusCode}",
            statusCode: 503
        );
    }

    var reports = await resp.Content.ReadFromJsonAsync<List<WorkReport>>();
    return Results.Ok(reports ?? new List<WorkReport>());
});

app.Run();
