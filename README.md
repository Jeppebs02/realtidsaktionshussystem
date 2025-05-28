# Project Launch Instructions

---

### 📥 Clone the Repository

First, clone the repository or download it manually:

```bash
git clone https://github.com/Jeppebs02/realtidsaktionshussystem.git
```

Then navigate to the following project directory:

```bash
realtidsaktionshussystem/Code/AuctionHouse
```

This is where the Docker Compose files (`docker-compose-db-local-release.yml` and `docker-compose-db-remote-release.yml`) are located.

---

This project can be launched using Docker Compose with either a **local** or **remote** database configuration.

---

## 🚀 Getting Started

### Option 1: Using a Remote Database

1. **Update Environment Variable**  
   Set the `DatabaseConnectionString` environment variable to point to the correct remote database host.

2. **Prepare the Database**  
   Connect to your remote database and execute the following SQL scripts from the `SQL-Script` folder **in this order**:
   
   - `CreateDBAuction_V.2.sql`
   - `GenerateTestData.sql`

3. **Launch the Application**  
   Start the containers with the following command:
   ```bash
   docker compose -f docker-compose-db-remote-release.yml up -d
   ```

---

### Option 2: Using a Local Database

1. **No Configuration Needed**  
   The local database connection string is already pre-configured.

2. **Launch the Application**  
   Start the containers with:
   ```bash
   docker compose -f docker-compose-db-local-release.yml up -d
   ```

3. **Prepare the Database**  
   Run the same SQL scripts as the remote setup:
   
   - `CreateDBAuction_V.2.sql`
   - `GenerateTestData.sql`

---

### 🖥️ Desktop App Setup (Local Machine)

To run the **desktop application**, you need to set the following **user-level environment variables** on your PC (not system-level):

| Variable Name              | Value                                  |
| -------------------------- | -------------------------------------- |
| `DatabaseConnectionString` | Your own DB connection string          |
| `api-key`                  | `8ea0cd87-f2aa-4c82-9ba9-5e9508f6e0ad` |
| `AuctionApiBaseUrl`        | `http://localhost:5002`              |

#### ✅ How to Set User Environment Variables (Windows)

1. Press `Win + S`, search for `Environment Variables`.
2. Click **Environment Variables**.
3. Under "User variables", click **New\...**.
4. Add each variable above.

---

## 📂 Folder Structure

- `SQL-Script/`: Contains the SQL scripts needed for initial database setup.

---

## 🌐 Access the Website and desktopapp
To access the website after starting the Docker containers, open your browser and go to:

```
http://localhost:5000
```
You may start the desktop app from visual studio. Make sure to run it in normal mode and NOT debugging mode. 
