#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Web.Manage/Sheaft.Web.Manage.csproj", "Sheaft.Web.Manage/"]
RUN dotnet restore "Sheaft.Web.Manage/Sheaft.Web.Manage.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Manage"
RUN dotnet build "Sheaft.Web.Manage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Manage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Manage.dll"]