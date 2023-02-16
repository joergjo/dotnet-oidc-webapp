#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AadClaimsViewer/AadClaimsViewer.csproj", "src/AadClaimsViewer/"]
RUN dotnet restore "src/AadClaimsViewer/AadClaimsViewer.csproj"
COPY . .
WORKDIR "/src/src/AadClaimsViewer"
RUN dotnet build "AadClaimsViewer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AadClaimsViewer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AadClaimsViewer.dll"]