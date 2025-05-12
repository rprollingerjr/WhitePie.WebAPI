# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["WhitePie.WebAPI.csproj", "."]
RUN dotnet restore "WhitePie.WebAPI.csproj"

# Copy the rest of the code
COPY . .
RUN dotnet publish "WhitePie.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WhitePie.WebAPI.dll"]
