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

## Features
In this solution you will notice pretty interesting implementations

* Roles: There exist 3 different Roles for users in the ecosystem: Admin, Power and Regular User
* Authentication using Bearer Token
A users Table was created with sample seed data for each Role type, the Role and Country of the User is embedded into the Bearer Token to be used in the application  
* Localization
The User's token contains a locale claim which is used to set the locale of the API (via a Request Culture Provider: TokenBasedRequestCultureProvider), if the token doesn't contain locale, it can be injected as an Accept-Language header  
* Role Access Policy
This is implemented using Net Core's intrinsic RBAC approach. Regular User only has ReadOnly access, Power User has limited write access and full read access but deletions are restricted, Admin User has full access including deletion 
* Attribute Access Policy
Also an implementation for attribute based policies was implemented to demonstrate the use of middleware, a IsNZLUser policy was created and can restrict access to certain endpoints to users whose Country code is set to NZL

## Stuff I wanted to implement but didn't have the time to:
- OAuth2
- 
x
