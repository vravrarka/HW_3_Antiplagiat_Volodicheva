FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FileAnalysisService.csproj ./
RUN dotnet restore FileAnalysisService.csproj

COPY . .
RUN dotnet publish FileAnalysisService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8082

ENTRYPOINT ["dotnet", "FileAnalysisService.dll"]