FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Agenda.Api/Agenda.Api.csproj", "Agenda.Api/"]
COPY ["Agenda.Application/Agenda.Application.csproj", "Agenda.Application/"]
COPY ["Agenda.Domain/Agenda.Domain.csproj", "Agenda.Domain/"]
COPY ["Agenda.Infrastructure/Agenda.Infrastructure.csproj", "Agenda.Infrastructure/"]

RUN dotnet restore "Agenda.Api/Agenda.Api.csproj"

COPY . .
WORKDIR /src/Agenda.Api

RUN dotnet publish "Agenda.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y netcat

COPY --from=build /app/publish .

COPY Agenda.Api/entrypoint.sh .
RUN chmod +x entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["./entrypoint.sh"]