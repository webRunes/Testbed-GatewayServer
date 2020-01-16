FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Demo/Demo.csproj", "Demo/"]
RUN dotnet restore "Demo/Demo.csproj"
COPY . .
WORKDIR "/src/Demo"
RUN dotnet build "Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.dll"]