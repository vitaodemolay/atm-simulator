# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy solution and project files
COPY atm-simulator.sln .
COPY atm-executor/ ./atm-executor/
COPY atm-unitTests/ ./atm-unitTests/

# Restore and build
RUN dotnet restore
RUN dotnet publish atm-executor/atm-executor.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

# Copy built application from build stage
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["./atm-executor"]
