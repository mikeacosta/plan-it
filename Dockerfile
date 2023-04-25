FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PlanIt.API/PlanIt.API.csproj", "PlanIt.API/"]
RUN dotnet restore "PlanIt.API/PlanIt.API.csproj"
COPY . .
WORKDIR "/src/PlanIt.API"
RUN dotnet build "PlanIt.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanIt.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanIt.API.dll"]

ENV ASPNETCORE_ENVIRONMENT=Development
