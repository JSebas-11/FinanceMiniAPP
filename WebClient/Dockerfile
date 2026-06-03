# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish WebClient/WebClient.csproj -c Release -o /app

FROM nginx:alpine

COPY --from=build /app/wwwroot /usr/share/nginx/html

EXPOSE 80