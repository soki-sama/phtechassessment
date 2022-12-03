# Soki's PH Tech Assessment

__*This service can be deployed at any time*__

## Repository Contents

* Propeller.API - Contains the main WebAPI application
* Propeller.DALC - Data Access Layer
* Propeller.DALC.Sqlite - Data Implementation for SQLite
* Propeller.Entities - Data Entities
* Propeller.Integration.Tests - Integration Tests
* Propeller.Mappers - Model/Entities Mapper configuration for AutoMapper
* Propeller.Models - DTO Models
* Propeller.Shared - Shared Functionality

__*PropellerHeadTechAssessment.sln:*__ Contains the main Application
__*PropellerHeadTechAssessment.Tests.sln:*__ Contains Integration Tests

## How to Run

After cloning the GitHub repository

1.- Open an Administrator Power Shell Console  
2.- Navigate to the location of the Propeller.API.csproj (Main Application)
3.- Run the following command: dotnet run

Alternative:
1.- Open the PropellerHeadTechAssessment.sln file on Visual Studio 2022
2.- Verify the Propeller.API is set to run as Startup project
3.- Press F5
4.- Be amazed :)

## API Documentation

A basic documentation for the API endpoints and it's usage can be found at the following address (when run locally):

https://localhost:port/swagger/index.html

You will need to set port number to whatever port your application is running in
.
x
