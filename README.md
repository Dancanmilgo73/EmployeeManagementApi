# Employee Management API

## Running the Project Locally
1. Install .NET 9 SDK and SQL Server.
2. Build the app `dotnet build`
3. Run migrations: `dotnet ef database update --project src/EmployeeManagementApi/`.
4. Start the API: `dotnet run --project src/EmployeeManagementApi`.

## Running with Docker
1. Install Docker and Docker Compose.
2. Navigate to the root directory
2. Run `docker-compose up --build -d`.
3. Access the API at `http://localhost:8080/swagger/index.html`.
4. Run migrations inside the container:
   ```bash
   docker exec -it comply-web-1 dotnet ef database update