FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Actor/DemoActor/DemoActor.csproj", "Actor/DemoActor/"]
COPY ["../src/Dapr.Actors.AspNetCore/Dapr.Actors.AspNetCore.csproj", "../src/Dapr.Actors.AspNetCore/"]
COPY ["../src/Dapr.Actors/Dapr.Actors.csproj", "../src/Dapr.Actors/"]
COPY ["Actor/IDemoActor/IDemoActor.csproj", "Actor/IDemoActor/"]
RUN dotnet restore "Actor/DemoActor/DemoActor.csproj"
COPY . .
WORKDIR "/src/Actor/DemoActor"
RUN dotnet build "DemoActor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoActor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoActor.dll"]