# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
# EXPOSE 5003 # Only if you configure and use HTTPS within the container

# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- Step 1: Copy ONLY the .csproj files needed for restore ---
# Copy the main project file
COPY ["AuctionHouse.WebSite/AuctionHouse.WebSite.csproj", "AuctionHouse.WebSite/"]
# Copy project file for its direct dependency
COPY ["AuctionHouse.ClassLibrary/AuctionHouse.ClassLibrary.csproj", "AuctionHouse.ClassLibrary/"]

# --- Step 2: Restore dependencies ---
# Restore the main project; it restores ClassLibrary transitively
RUN dotnet restore "AuctionHouse.WebSite/AuctionHouse.WebSite.csproj"

# --- Step 3: Copy ALL necessary source code ---
# Copy the main project's code
COPY ["AuctionHouse.WebSite/", "AuctionHouse.WebSite/"]
# Copy the code for its direct dependency
COPY ["AuctionHouse.ClassLibrary/", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.APIRequester/", "AuctionHouse.APIRequester/"]

# --- Step 4: Build and Publish ---
# Set WORKDIR to the project to be published
WORKDIR "/src/AuctionHouse.WebSite"
# Run publish - it handles the build implicitly and puts ALL necessary output
# (including ClassLibrary.dll) correctly into /app/publish
RUN dotnet publish "AuctionHouse.WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# --- Step 5: Final runtime image ---
FROM base AS final
WORKDIR /app
# Copy the successfully published output from the 'build' stage
COPY --from=build /app/publish .
# Ensure this DLL name matches your project's output Assembly Name
ENTRYPOINT ["dotnet", "AuctionHouse.WebSite.dll"]