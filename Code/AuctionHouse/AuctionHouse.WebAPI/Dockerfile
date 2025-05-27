# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
# EXPOSE 5001 # Only if you configure and use HTTPS within the container

# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- Step 1: Copy ONLY the .csproj files needed for restore ---
# Copy the main project file
COPY ["AuctionHouse.WebAPI/AuctionHouse.WebAPI.csproj", "AuctionHouse.WebAPI/"]
# Copy project files for direct dependencies
COPY ["AuctionHouse.ClassLibrary/AuctionHouse.ClassLibrary.csproj", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/AuctionHouse.DataAccessLayer.csproj", "AuctionHouse.DataAccessLayer/"]

# --- Step 2: Restore dependencies ---
# Restore the main project; it restores dependencies transitively
RUN dotnet restore "AuctionHouse.WebAPI/AuctionHouse.WebAPI.csproj"

# --- Step 3: Copy ALL necessary source code ---
# Copy the main project's code
COPY ["AuctionHouse.WebAPI/", "AuctionHouse.WebAPI/"]
# Copy the code for its dependencies
COPY ["AuctionHouse.ClassLibrary/", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/", "AuctionHouse.DataAccessLayer/"]

# --- Step 4: Build and Publish ---
# Set WORKDIR to the project to be published
WORKDIR "/src/AuctionHouse.WebAPI"
# Run publish - it handles the build implicitly and puts ALL necessary output
# (including dependency DLLs) correctly into /app/publish
RUN dotnet publish "AuctionHouse.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# --- Step 5: Final runtime image ---
FROM base AS final
WORKDIR /app
# Copy the successfully published output from the 'build' stage
COPY --from=build /app/publish .
# Ensure this DLL name matches your project's output Assembly Name
ENTRYPOINT ["dotnet", "AuctionHouse.WebAPI.dll"]