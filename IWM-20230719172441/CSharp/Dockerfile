FROM registry-dev.truesight.asia/truesight/aspnet:6.0.11-bullseye-slim AS base
WORKDIR /app
RUN apt-get update && apt-get install -y net-tools curl iputils-ping telnet nano vim libc6-dev libgdiplus

FROM registry-dev.truesight.asia/truesight/dotnet-sdk:6.0.403-bullseye-slim AS build
WORKDIR /src
COPY ["IWM.csproj", "./"]
RUN dotnet restore "IWM.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "IWM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IWM.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

COPY ["docker-entrypoint.sh", "."]

RUN chmod a+x docker-entrypoint.sh

CMD ["./docker-entrypoint.sh"]

EXPOSE 8080
