# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers (best for caching)
COPY ["WhitePie.WebAPI/WhitePie.WebAPI/WhitePie.WebAPI/WhitePie.WebAPI.csproj", "WhitePie.WebAPI/"]
RUN dotnet restore "WhitePie.WebAPI/WhitePie.WebAPI.csproj"

# Copy the rest of the source tree
COPY . .

# Build and publish the app
WORKDIR "/src/WhitePie.WebAPI/WhitePie.WebAPI/WhitePie.WebAPI"
RUN dotnet publish "WhitePie.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WhitePie.WebAPI.dll"]
