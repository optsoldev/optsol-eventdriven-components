#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.Hotel.Driving.Commands/Sample.Hotel.Driving.Commands.csproj", "src/Sample.Hotel.Driving.Commands/"]
RUN dotnet restore "src/Sample.Hotel.Driving.Commands/Sample.Hotel.Driving.Commands.csproj"
COPY . .
WORKDIR "/src/src/Sample.Hotel.Driving.Commands"
RUN dotnet build "Sample.Hotel.Driving.Commands.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Hotel.Driving.Commands.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Hotel.Driving.Commands.dll"]