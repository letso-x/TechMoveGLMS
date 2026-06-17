FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TechMoveGLMS.csproj", "TechMoveGLMS/"]
RUN dotnet restore "TechMoveGLMS/TechMoveGLMS.csproj"
COPY . TechMoveGLMS/
WORKDIR "/src/TechMoveGLMS"
RUN dotnet publish "TechMoveGLMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechMoveGLMS.dll"]
