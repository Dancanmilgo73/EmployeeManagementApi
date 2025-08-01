# Employee Management API

## Running with Docker
1. Install Docker and Docker Compose.
2. Navigate to the root directory, where you see this README.md file.
3. Run `docker-compose up --build -d`
4. Access the API at `http://localhost:8080/swagger/index.html`.

## Running the Project Locally
1. Install .NET 9 SDK and SQL Server.
2. Navigate to the root of the project, where you see this README.md file.
3. Build the app `dotnet build`
5. Start the API: `dotnet run --project src/EmployeeManagementApi`.
6. Access the endpoints from your browser using the URL http://localhost:5058/swagger/index.html and 
   start interacting with this awesome API