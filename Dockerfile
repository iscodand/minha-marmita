FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/Presentation/Presentation.csproj", "src/Presentation/"]
COPY ["src/Infrastructure.Ioc/Infrastructure.Ioc.csproj", "src/Infrastructure.Ioc/"]
COPY ["src/Infrastructure.Identity/Infrastructure.Identity.csproj", "src/Infrastructure.Identity/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure.Data/Infrastructure.Data.csproj", "src/Infrastructure.Data/"]
COPY ["src/Tests/Tests.csproj", "src/Tests/"]

RUN dotnet restore "src/Presentation/Presentation.csproj"
COPY . .

RUN dotnet build "src/Presentation/Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Presentation/Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]