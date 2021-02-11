#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5003;http://*:5002
ENV ASPNETCORE_HTTPS_PORT=5003
EXPOSE 5002
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Web.Api/Sheaft.Web.Api.csproj", "Sheaft.Web.Api/"]
COPY ["Sheaft.Web.Common/Sheaft.Web.Common.csproj", "Sheaft.Web.Common/"]
COPY ["Sheaft.GraphQL/Sheaft.GraphQL.csproj", "Sheaft.GraphQL/"]
COPY ["Sheaft.Application/Sheaft.Application.csproj", "Sheaft.Application/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
COPY ["Sheaft.Infrastructure/Sheaft.Infrastructure.csproj", "Sheaft.Infrastructure/"]
COPY ["Sheaft.Localization/Sheaft.Localization.csproj", "Sheaft.Localization/"]
RUN dotnet restore "Sheaft.Web.Api/Sheaft.Web.Api.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Api"
RUN dotnet build "Sheaft.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Api.dll"]
