FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
ARG PROJECT_PORT
WORKDIR /app
EXPOSE $PROJECT_PORT

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["ParsingProject.DAL/ParsingProject.DAL.csproj", "ParsingProject.DAL/ParsingProject.DAL.csproj"]
COPY ["ParsingProject.BLL/ParsingProject.BLL.csproj", "ParsingProject.BLL/ParsingProject.BLL.csproj"]
COPY ["ParsingProject/ParsingProject.csproj", "ParsingProject/ParsingProject.csproj"]


RUN dotnet restore "ParsingProject/ParsingProject.csproj"
COPY . .
WORKDIR "/src/ParsingProject"
RUN dotnet build "ParsingProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParsingProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5050
ENTRYPOINT ["dotnet", "ParsingProject.dll"]