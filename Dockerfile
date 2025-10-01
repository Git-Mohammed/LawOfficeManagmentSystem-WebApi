# -------------------------------
# Stage 1: Build
# -------------------------------
# Use the full .NET SDK image (includes compilers, build tools, etc.)
# This stage is only for compiling and publishing the app.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory inside the container
WORKDIR /build

# Copy only project files first (to leverage Docker layer caching).
# This way, if source code changes but project files don't, restore won't re-run.
COPY ["src/LOMs.Api/LOMs.Api.csproj", "src/LOMs.Api/"]
COPY ["src/LOMs.Application/LOMs.Application.csproj", "src/LOMs.Application/"]
COPY ["src/LOMs.Domain/LOMs.Domain.csproj", "src/LOMs.Domain/"]
COPY ["src/LOMs.Contract/LOMs.Contract.csproj", "src/LOMs.Contract/"]
COPY ["src/LOMs.Infrastructure/LOMs.Infrastructure.csproj", "src/LOMs.Infrastructure/"]

# Restore NuGet dependencies (done once, cached if csproj files unchanged)
RUN dotnet restore "src/LOMs.Api/LOMs.Api.csproj"

# Copy the rest of the source code into the container
COPY . .

# Build and publish the app in Release mode into /app
# --no-restore avoids re-restoring since we already did it above
RUN dotnet publish "src/LOMs.Api/LOMs.Api.csproj" -c Release -o /app 


# -------------------------------
# Stage 2: Final Runtime Image
# -------------------------------
# Use the smaller ASP.NET Core runtime image (no SDK, just runtime)
# This keeps the final image lightweight and secure.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Install timezone data so TimeZoneInfo works correctly inside the container
# Set it to Asia/Riyadh (KSA timezone)
RUN apt-get update && apt-get install -y tzdata && \
    ln -fs /usr/share/zoneinfo/Asia/Riyadh /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    rm -rf /var/lib/apt/lists/*

# Set timezone environment variable for the container
ENV TZ=Asia/Riyadh

# Set working directory for the app
WORKDIR /app

# Copy published output from the build stage
COPY --from=build /app .

# Expose port 80 for HTTP traffic
EXPOSE 80

# Define the entrypoint for the container
ENTRYPOINT ["dotnet", "LOMs.Api.dll"]
