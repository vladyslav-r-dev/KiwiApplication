FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/KiwiApp/KiwiApp.csproj", "src/KiwiApp/"]
RUN dotnet restore "src/KiwiApp/KiwiApp.csproj"

COPY . .
WORKDIR "/src/src/KiwiApp"
RUN dotnet publish "KiwiApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KiwiApp.dll"]
