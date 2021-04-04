#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["1. Presentation/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp.csproj", "1. Presentation/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp/"]
RUN dotnet restore "1. Presentation/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp.csproj"

# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs

COPY . .
WORKDIR "/src/1. Presentation/SCode.Client.Student.WebApp/SCode.Client.Student.WebApp"
RUN dotnet build "SCode.Client.Student.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SCode.Client.Student.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SCode.Client.Student.WebApp.dll"]