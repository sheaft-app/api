#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5020;http://*:5019
ENV ASPNETCORE_HTTPS_PORT=5020
EXPOSE 5020
EXPOSE 5019

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Web.Jobs/Sheaft.Web.Jobs.csproj", "Sheaft.Web.Jobs/"]
RUN dotnet restore "Sheaft.Web.Jobs/Sheaft.Web.Jobs.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Jobs"
RUN dotnet build "Sheaft.Web.Jobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Jobs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Jobs.dll"]