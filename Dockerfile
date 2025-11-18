# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# ←←← CHANGE THIS LINE TO YOUR REAL PROJECT NAME ←←←
RUN dotnet restore "Quizme-Api.csproj"    # ← fix this name only!

RUN dotnet publish "Quizme-Api.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "QuizmeBackend.dll"]   # ← also fix this line (same name, but .dll)
