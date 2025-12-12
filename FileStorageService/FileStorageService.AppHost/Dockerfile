FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FileStorageService.csproj ./
RUN dotnet restore FileStorageService.csproj

COPY . .
RUN dotnet publish FileStorageService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8081

ENTRYPOINT ["dotnet", "FileStorageService.dll"]
