#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["2. Services/SCode.WebApi/SCode.WebApi.csproj", "2. Services/SCode.WebApi/"]
COPY ["3. Shared/SCode.Shared/SCode.Shared.csproj", "3. Shared/SCode.Shared/"]
RUN dotnet restore "2. Services/SCode.WebApi/SCode.WebApi.csproj"
COPY . .
WORKDIR "/src/2. Services/SCode.WebApi"
RUN dotnet build "SCode.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SCode.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SCode.WebApi.dll"]