#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.Hotel.Driving.Projections/Sample.Hotel.Driving.Projections.csproj", "src/Sample.Hotel.Driving.Projections/"]
RUN dotnet restore "src/Sample.Hotel.Driving.Projections/Sample.Hotel.Driving.Projections.csproj"
COPY . .
WORKDIR "/src/src/Sample.Hotel.Driving.Projections"
RUN dotnet build "Sample.Hotel.Driving.Projections.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Hotel.Driving.Projections.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Hotel.Driving.Projections.dll"]