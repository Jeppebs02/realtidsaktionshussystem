# Project Launch Instructions

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

2. **Prepare the Database**  
   Run the same SQL scripts as the remote setup:
   
   - `CreateDBAuction_V.2.sql`
   - `GenerateTestData.sql`

3. **Launch the Application**  
   Start the containers with:
   ```bash
   docker compose -f docker-compose-db-local-release.yml up -d
   ```

---

## 📂 Folder Structure

- `SQL-Script/`: Contains the SQL scripts needed for initial database setup.

---

## ✅ Notes

- Ensure Docker is installed and running on your system.
- Use `docker compose logs -f` to monitor logs if needed.

## 🌐 Access the Website
To access the website after starting the Docker containers, open your browser and go to:

```
http://localhost:5000
```
