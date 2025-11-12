# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# # Copy csproj and restore
# COPY *.csproj ./
# RUN dotnet restore

# # Copy everything else and build
# COPY . ./
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# WORKDIR /app
# COPY --from=build /app/out .

# # Expose port
# EXPOSE 8080

# # Set environment to production
# ENV ASPNETCORE_ENVIRONMENT=Production
# ENV ASPNETCORE_URLS=http://+:8080

# ENTRYPOINT ["dotnet", "SmartBudgetTracker.dll"]

# # Stage 1: Build Vue.js frontend
# FROM node:18-alpine AS frontend-build
# WORKDIR /client

# # Copy package files
# COPY ../client/package*.json ./
# RUN npm install

# # Copy client source and build
# COPY ../client/ ./
# RUN npm run build

# # Stage 2: Build .NET backend
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
# WORKDIR /app

# # Copy csproj and restore
# COPY api/*.csproj ./
# RUN dotnet restore

# # Copy everything else and build
# COPY api/ ./
# RUN dotnet publish -c Release -o out

# # Copy Vue.js build to wwwroot
# COPY --from=frontend-build /client/dist ./out/wwwroot

# # Stage 3: Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# WORKDIR /app
# COPY --from=backend-build /app/out .

# # Expose port
# EXPOSE 8080

# # Set environment to production
# ENV ASPNETCORE_ENVIRONMENT=Production
# ENV ASPNETCORE_URLS=http://+:8080

# ENTRYPOINT ["dotnet", "SmartBudgetTracker.dll"]


# Stage 1: Build Vue.js frontend
FROM node:20-alpine AS frontend-build
WORKDIR /client

# Copy package files
COPY client/package*.json ./
RUN npm install

# Copy client source and build
COPY client/ ./
RUN npm run build

# Stage 2: Build .NET backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app

# Copy csproj and restore
COPY api/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY api/ ./
RUN dotnet publish -c Release -o out

# Copy Vue.js build to wwwroot
COPY --from=frontend-build /client/dist ./out/wwwroot

# Stage 3: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/out .

# Expose port
EXPOSE 8080

# Set environment to production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SmartBudgetTracker.dll"]