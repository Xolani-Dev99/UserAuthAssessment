# ------------------ Build Stage ------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only main project csproj and restore dependencies
COPY UserAuthApi.csproj ./
RUN dotnet restore

# Copy only necessary source files (exclude tests, obj/bin)
COPY Controllers ./Controllers
COPY Data ./Data
COPY DTOs ./DTOs
COPY Interfaces ./Interfaces
COPY Mappings ./Mappings
COPY Models ./Models
COPY Services ./Services
COPY Program.cs ./
COPY appsettings.json ./
COPY appsettings.Development.json ./

# Publish only the main project
RUN dotnet publish UserAuthApi.csproj -c Release -o out

# ------------------ Runtime Stage ------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out .

# Expose API port
EXPOSE 5000

# Run the API
ENTRYPOINT ["dotnet", "UserAuthApi.dll"]