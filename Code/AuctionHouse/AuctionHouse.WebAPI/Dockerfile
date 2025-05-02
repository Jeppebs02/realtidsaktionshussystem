# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- Step 1: Copy ONLY the .csproj files needed for restore ---
COPY ["AuctionHouse.WebAPI/AuctionHouse.WebAPI.csproj", "AuctionHouse.WebAPI/"]
COPY ["AuctionHouse.ClassLibrary/AuctionHouse.ClassLibrary.csproj", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/AuctionHouse.DataAccessLayer.csproj", "AuctionHouse.DataAccessLayer/"]

# --- Step 2: Restore dependencies ---
RUN dotnet restore "AuctionHouse.WebAPI/AuctionHouse.WebAPI.csproj"

# --- Step 3: Copy the source code for the project and its dependencies ---
COPY ["AuctionHouse.WebAPI/", "AuctionHouse.WebAPI/"]
COPY ["AuctionHouse.ClassLibrary/", "AuctionHouse.ClassLibrary/"]
COPY ["AuctionHouse.DataAccessLayer/", "AuctionHouse.DataAccessLayer/"]


# --- Step 4: Build ---
WORKDIR "/src/AuctionHouse.WebAPI"
RUN dotnet build "AuctionHouse.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# --- Step 5: Publish ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuctionHouse.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# --- Step 6: Final runtime image ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionHouse.WebAPI.dll"]