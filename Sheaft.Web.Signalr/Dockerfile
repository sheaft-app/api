#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5010;http://*:5009
ENV ASPNETCORE_HTTPS_PORT=5010
EXPOSE 5010
EXPOSE 5009

FROM mcr.microsoft.com/dotnet/sdk:5.0.300-buster-slim AS build
WORKDIR /src
COPY ["Sheaft.Web.Signalr/Sheaft.Web.Signalr.csproj", "Sheaft.Web.Signalr/"]
COPY ["Sheaft.Web.Common/Sheaft.Web.Common.csproj", "Sheaft.Web.Common/"]
COPY ["Sheaft.Application/Sheaft.Application.csproj", "Sheaft.Application/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
COPY ["Sheaft.Mediatr/Sheaft.Mediatr.csproj", "Sheaft.Mediatr/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Mailing/Sheaft.Mailing.csproj", "Sheaft.Mailing/"]
COPY ["Sheaft.Infrastructure/Sheaft.Infrastructure.csproj", "Sheaft.Infrastructure/"]
RUN dotnet restore "Sheaft.Web.Signalr/Sheaft.Web.Signalr.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Signalr"
RUN dotnet build "Sheaft.Web.Signalr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Signalr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Signalr.dll"]
