# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./
RUN dotnet restore "./GFontProxy.NET/GFontProxy.NET.csproj" --disable-parallel
RUN dotnet publish "./GFontProxy.NET/GFontProxy.NET.csproj" -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
RUN mkdir -p /app/fonts
ENTRYPOINT ["dotnet", "GFontProxy.NET.dll"]