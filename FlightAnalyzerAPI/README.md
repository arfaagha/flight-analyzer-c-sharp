# Flight Quality Analysis

## Overview

This project automates the data quality analysis of airline flight records. This rest API is a .NET Core application that reads flight data from CSV files, converts the data into a JSON model, and provides REST APIs to analyze the data and detect inconsistencies. This solution helps the airline streamline operations by quickly identifying and resolving issues in flight records.

## Features

1. **REST API for Flight Data**:
   - Provides a REST API to read flight data from a CSV file stored on the server.
   - Converts CSV data into a JSON model and returns it to the client.
   - This API has swagger/OpenAPI integrated so running it in dev mode would open swagger UI for easy API documentation.
   - Clients can access this API using tools like `curl` to retrieve flight data in JSON format.

2. **REST API for Flight Chain Analysis**:
   - Analyzes flight chains to detect inconsistencies.
   - A flight chain is defined by the logical sequence of flights, where the arrival airport of one flight should be the departure airport of the next flight for the same aircraft.
   - Identifies and returns any flights that break this logical sequence.

## API Endpoints 
### 1. GET /api/flights 
- **Description**: Fetches all flight data from the server in JSON format. 
- **Response**: JSON array of flights. 
### 2. GET api/flights/inconsistent 
- **Description**: Analyzes flight chains to find inconsistencies in the sequence of flights. 
- **Response**:  JSON array of flights that are inconsistent in their chaining logic. 

## CSV File Structure 
For this API, we are assuming the file provided is in CSV format and contains the following fields (and in the same order): 
- **Id**: Identifier for each row. 
- **AircraftRegistrationNumber**: The unique identifier of the air craft. 
- **AircraftType**: The type of air craft. 
- **FlightNumber**: The (almost) unique identifier of the flight. 
- **DepartureAirport**: 3-letter code of the departure airport.
- **DepartureDateTime**: The scheduled departure time.
- **ArrivalAirport**: 3-letter code of the arrival airport.
- **ArrivalDateTime**: The scheduled arrival time.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 3.1 or higher)
- A CSV file containing flight data, structured with the necessary fields (e.g., flight number, departure airport, arrival airport, etc.)

## Project Structure

- **Controllers**: Contains the REST API controllers for handling HTTP requests.
- **Models**: Defines the data models used in the application, including the flight model.
- **Services**: Contains the business logic for reading, converting, and analyzing flight data.
- **Helper**: Utility classes for parsing CSV files and other extension functions.
- **Tests**: Unit tests to ensure code quality and correctness.

## Running the API

### 1. Unzip the `FlightAnalyzerAPI.rar` file. 
You can do this using your preferred unzip tool or the command line.

##### Windows Command Line:
```sh
tar -xf FlightAnalyzerAPI.rar
```

### 2. Navigate to the Project Directory
After unzipping, navigate to the project directory where the .csproj file is located.

##### Windows Command Line:
```sh
cd FlightAnalyzerAPI
```

### 3. Update file path 
Make sure the path of the file to be evaluated is placed is correct. In the `appsettings.json` file:

```json
  "FlightData": {
    "CsvFilePath": "flights.csv"
  }
```

### 4. Running the API
After unzipping, navigate to the project directory where the .csproj file is located.

##### Windows Command Line:
```sh
dotnet run
```

This command builds and runs the API. By default, the API will be available at `https://localhost:7073`.

### 5. Running the Tests
Change the directory to tests using the following command:

#### Windows Command Line:
```sh
cd FlightAnalyzerAPITests
```

To execute the test project and ensure everything is functioning correctly, use the following command:

##### Windows Command Line:
```sh
dotnet test
```

This command builds and runs the API. By default, the API will be available at `https://localhost:7073`.

### 6. Swagger UI
API documentation for the REST API is available [swagger](https://localhost:7037/swagger/index.html).


## Limitations & Future Improvements

- **Scalability**: The current implementation works for small to medium-sized data. For larger datasets, consider implementing pagination or streaming.
- **Error Handling**: The application currently has basic error handling. Enhanced error handling mechanisms could be added for better fault tolerance.
