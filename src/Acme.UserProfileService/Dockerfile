# =========================================================
#            1. BUILD STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files for restore
COPY ["Acme.UserService.Api/Acme.UserService.Api.csproj", "Acme.UserService.Api/"]
COPY ["Acme.UserService.Application/Acme.UserService.Application.csproj", "Acme.UserService.Application/"]
COPY ["Acme.UserService.Domain/Acme.UserService.Domain.csproj", "Acme.UserService.Domain/"]
COPY ["Acme.UserService.Infrastructure/Acme.UserService.Infrastructure.csproj", "Acme.UserService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Acme.UserService.Api/Acme.UserService.Api.csproj"

# Copy full solution
COPY . .

# Publish
WORKDIR "/src/Acme.UserService.Api"
RUN dotnet publish -c Release -o /app/publish

# =========================================================
#            2. RUNTIME STAGE
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Acme.UserService.Api.dll"]
