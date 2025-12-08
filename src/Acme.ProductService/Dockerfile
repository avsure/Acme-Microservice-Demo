# =========================================================
#            1. BUILD STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files (for cache efficiency)
COPY ["Acme.ProductService.Api/Acme.ProductService.Api.csproj", "Acme.ProductService.Api/"]
COPY ["Acme.ProductService.Application/Acme.ProductService.Application.csproj", "Acme.ProductService.Application/"]
COPY ["Acme.ProductService.Domain/Acme.ProductService.Domain.csproj", "Acme.ProductService.Domain/"]
COPY ["Acme.ProductService.Infrastructure/Acme.ProductService.Infrastructure.csproj", "Acme.ProductService.Infrastructure/"]

# Restore NuGet packages
RUN dotnet restore "Acme.ProductService.Api/Acme.ProductService.Api.csproj"

# Copy all files
COPY . .

# Publish
WORKDIR "/src/Acme.ProductService.Api"
RUN dotnet publish -c Release -o /app/publish

# =========================================================
#            2. RUNTIME STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Acme.ProductService.Api.dll"]
