FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first to cache restore
COPY ["src/LearnIT.API/LearnIT.API.csproj", "src/LearnIT.API/"]
COPY ["src/LearnIT.Application/LearnIT.Application.csproj", "src/LearnIT.Application/"]
COPY ["src/LearnIT.Domain/LearnIT.Domain.csproj", "src/LearnIT.Domain/"]
COPY ["src/LearnIT.Infrastructure/LearnIT.Infrastructure.csproj", "src/LearnIT.Infrastructure/"]

RUN dotnet restore "src/LearnIT.API/LearnIT.API.csproj"

# Copy the rest
COPY . .

WORKDIR "/src/src/LearnIT.API"
RUN dotnet build "LearnIT.API.csproj" -c Release -o /app/build
RUN dotnet publish "LearnIT.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "LearnIT.API.dll"]
