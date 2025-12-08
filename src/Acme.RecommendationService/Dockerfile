# =========================================================
#            1. BUILD STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["Acme.RecommendationService.Api/Acme.RecommendationService.Api.csproj", "Acme.RecommendationService.Api/"]
COPY ["Acme.RecommendationService.Application/Acme.RecommendationService.Application.csproj", "Acme.RecommendationService.Application/"]
COPY ["Acme.RecommendationService.Domain/Acme.RecommendationService.Domain.csproj", "Acme.RecommendationService.Domain/"]
COPY ["Acme.RecommendationService.Infrastructure/Acme.RecommendationService.Infrastructure.csproj", "Acme.RecommendationService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Acme.RecommendationService.Api/Acme.RecommendationService.Api.csproj"

# Copy full solution
COPY . .

# Publish
WORKDIR "/src/Acme.RecommendationService.Api"
RUN dotnet publish -c Release -o /app/publish

# =========================================================
#            2. RUNTIME STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Acme.RecommendationService.Api.dll"]
