# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Quizme-Api.csproj"
RUN dotnet publish "Quizme-Api.csproj" -c Release -o /app/publish --no-restore

# Runtime stage → use the slim runtime that works on Render
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# These 2 lines are the magic fix for Render
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Quizme-Api.dll"]
