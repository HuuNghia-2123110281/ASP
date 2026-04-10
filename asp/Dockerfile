# Sử dụng SDK .NET 8.0 để build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["asp.csproj", "./"]
RUN dotnet restore "asp.csproj"
COPY . .
RUN dotnet publish "asp.csproj" -c Release -o /app/publish

# Môi trường chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "asp.dll"]