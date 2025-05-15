# Use the .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the shared library project file
COPY ["Shared/Shared.csproj", "Shared/"]

# Copy the ProductService project file
COPY ["ProductService/ProductService.csproj", "ProductService/"]

# Restore dependencies
RUN dotnet restore "ProductService/ProductService.csproj"

# Copy all source code
COPY Shared ./Shared
COPY ProductService ./ProductService

# Build the project
WORKDIR "/src/ProductService"
RUN dotnet build "ProductService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]
