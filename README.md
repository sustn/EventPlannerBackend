## Backend & Testing

### Running the Test Project
1. Open the solution in Visual Studio.
2. Set the test project as the startup project.
3. Run the tests using the built-in Test Explorer.

### The testing project includes:
1. Unit and integration tests for event creation.
2. Validation of tenant ID and required fields.
3. Assertion of correct behavior under different input scenarios.

### Email Mocking
In this project, emails are **mocked** by storing them in a database instead of sending them out to real email addresses. This approach is particularly useful during development, testing, or when performing tasks like debugging email functionality without actually sending emails to users.

### Default Connection String Configuration
To ensure that the application connects to the correct database, the connection string in the appsettings.json file must be configured appropriately. By default, the application uses the DefaultConnection string for database connectivity.

### Steps to Update Connection String
1. Open appsettings.json.
2. Locate the ConnectionStrings section.
3. Update the DefaultConnection string with the correct server, database name, username, and password for your environment.
4. Save the file.

### Additional Notes
- All events are stored and retrieved based on the tenantId field to ensure multi-tenant separation.
- Ensure that the backend API is running and accessible from the specified base URL.
- When deploying to production, make sure to configure the .env file appropriately based on the environment.
- Always verify that the database connection string points to the correct database instance for the environment you're working in.
- Make sure to regularly apply any new migrations to ensure the database schema remains in sync with your application's data model.
