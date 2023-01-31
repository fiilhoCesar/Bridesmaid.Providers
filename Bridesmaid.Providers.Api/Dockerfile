FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bridesmaid.Providers.Api/Bridesmaid.Providers.Api.csproj", "Bridesmaid.Providers.Api/"]
RUN dotnet restore "Bridesmaid.Providers.Api/Bridesmaid.Providers.Api.csproj"
COPY . .
WORKDIR "/src/Bridesmaid.Providers.Api"
RUN dotnet build "Bridesmaid.Providers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bridesmaid.Providers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bridesmaid.Providers.Api.dll"]
