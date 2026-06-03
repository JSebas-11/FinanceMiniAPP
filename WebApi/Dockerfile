# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi.dll"]