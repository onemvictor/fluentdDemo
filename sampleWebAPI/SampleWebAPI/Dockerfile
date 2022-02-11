#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine3.14 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SampleWebAPI/SampleWebAPI.csproj", "SampleWebAPI/"]
RUN dotnet restore "SampleWebAPI/SampleWebAPI.csproj"
COPY . .
WORKDIR "/src/SampleWebAPI"
RUN dotnet build "SampleWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleWebAPI.dll"]