# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- Step 1: Copy ONLY the .csproj files needed for restore ---
# Copy the main project file
COPY ["AuctionHouse.WebSite/AuctionHouse.WebSite.csproj", "AuctionHouse.WebSite/"]
# Copy project files for ALL dependencies (adjust paths as needed)
COPY ["AuctionHouse.ClassLibrary/AuctionHouse.ClassLibrary.csproj", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/AuctionHouse.DataAccessLayer.csproj", "AuctionHouse.DataAccessLayer/"]
# IMPORTANT: Do NOT copy the AuctionHouse.WebAPI.csproj here if WebSite doesn't directly reference WebAPI code.
# If WebSite *does* reference WebAPI (unlikely for a front-end), you would copy it too.

# --- Step 2: Restore dependencies ---
# Restore only the main project; it will restore its dependencies transitively
RUN dotnet restore "AuctionHouse.WebSite/AuctionHouse.WebSite.csproj"

# --- Step 3: Copy the source code for the project and its dependencies ---
# Copy the main project's code
COPY ["AuctionHouse.WebSite/", "AuctionHouse.WebSite/"]
# Copy the code for its dependencies
COPY ["AuctionHouse.ClassLibrary/", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/", "AuctionHouse.DataAccessLayer/"]
# Again, do NOT copy the WebAPI source code unless WebSite directly depends on it.

# --- Step 4: Build ---
# Set the working directory to the project being built
WORKDIR "/src/AuctionHouse.WebSite"
RUN dotnet build "AuctionHouse.WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# --- Step 5: Publish ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publish from the project directory
RUN dotnet publish "AuctionHouse.WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# --- Step 6: Final runtime image ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionHouse.WebSite.dll"]