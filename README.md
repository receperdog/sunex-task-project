
---

# Sunex Task Project

This project is a simple task management application built using ASP.NET Core 8.0. It demonstrates a clean architecture pattern, separating the Data Access Layer (DAL), Business Logic Layer (BLL), and Presentation Layer (PL) into distinct components. Unit tests have been written for each layer to ensure the correctness and robustness of the application.

## Table of Contents

- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [CI/CD Pipeline](#ci-cd-pipeline)
- [Contributing](#contributing)
- [License](#license)

## Project Structure

The project is organized into the following directories:

### 1. **Data (DAL)**
   - **Location:** `sunex-task-project/Data`
   - **Description:** This folder contains the Data Access Layer, which is responsible for interacting with the database. It includes:
     - `AppDbContext.cs`: Entity Framework Core context for managing database operations.
     - `ITaskRepository.cs`: Interface defining the contract for data operations.
     - `TaskRepository.cs`: Implementation of the repository pattern for data access.

### 2. **Services (BLL)**
   - **Location:** `sunex-task-project/Services`
   - **Description:** This folder contains the Business Logic Layer, which implements the core business logic of the application. It includes:
     - `ITaskService.cs`: Interface defining the contract for business logic operations.
     - `TaskService.cs`: Implementation of the business logic, including validation and mapping between DTOs and entities.

### 3. **Controllers (PL)**
   - **Location:** `sunex-task-project/Controllers`
   - **Description:** This folder contains the Presentation Layer, which handles HTTP requests and responses. It includes:
     - `TasksController.cs`: The API controller for managing task-related operations such as creating, updating, retrieving, and deleting tasks.

### 4. **Tests**
   - **Location:** `tests/sunex_task_project.Tests`
   - **Description:** This folder contains unit tests for the various layers of the application. It includes:
     - `Services`: Tests for the Business Logic Layer (`TaskServiceTests.cs`).
     - `Data`: Tests for the Data Access Layer (`TaskRepositoryTests.cs`).

## Technologies Used

- **ASP.NET Core 8.0:** The framework used to build the web API.
- **Entity Framework Core:** ORM for database operations.
- **xUnit:** Testing framework for writing unit tests.
- **Moq:** Mocking framework for isolating dependencies in unit tests.
- **AutoMapper:** Library for mapping objects, particularly useful for converting between entities and DTOs.
- **GitHub Actions:** CI/CD tool for automated testing and deployment.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- A text editor or IDE (e.g., Visual Studio Code, Visual Studio)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/receperdog/sunex-task-project.git
   cd sunex-task-project
   ```

2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

## Running the Application

To run the application locally:

1. **Build the project:**

   ```bash
   dotnet build
   ```

2. **Run the application:**

   ```bash
   dotnet run --project sunex-task-project
   ```

3. **Access the API:**

   Open your browser and navigate to `https://localhost:5001/swagger` to explore the API using Swagger.

## Running Tests

To run the unit tests:

1. **Navigate to the `tests` directory:**

   ```bash
   cd tests/sunex_task_project.Tests
   ```

2. **Run the tests:**

   ```bash
   dotnet test
   ```

## CI/CD Pipeline

This project uses GitHub Actions for Continuous Integration and Continuous Deployment (CI/CD). The pipeline is triggered on every push or pull request to the `master` branch. It automatically:

- Restores dependencies
- Builds the project
- Runs the unit tests

The configuration for this pipeline is located in `.github/workflows/dotnet.yml`.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or fixes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---
