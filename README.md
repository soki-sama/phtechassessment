# Soki's PH Tech Assessment

__*This service can be deployed at any time*__

## Table of Contents
* [Quickstart](#quickstart)  
* [Repository Contents](#repository-contents)  
* [How to Run](#how-to-run)  
* [How to Test](#how-to-test)  
* [API Documentation](#api-documentation)
* [Features](#features) 
* [Features I wanted to implement but ran out of time](#features-i-wanted-to-implement-but-ran-out-of-time)
* [Things I wish I would have done different](#things-i-wish-i-would-have-done-different)
* [Full Disclosure](#full-disclosure)
* [Final Words to the Reviewer](#final-words-to-the-reviewer)

## Quickstart  
### Requirements
To run this service you need to have .Net Core 6 or Higher installed

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

After cloning the GitHub repository:

1.- Open an Administrator Power Shell Console  
2.- Navigate to the location of the Propeller.API.csproj (Main Application)  
3.- Run the following command: dotnet run  

Alternative:  
1.- Open the PropellerHeadTechAssessment.sln file on Visual Studio 2022  
2.- Verify the Propeller.API is set to run as Startup project  
3.- Press F5  

4.- Be amazed :)  

## How to Test  
In the code base you can find a Folder named "Postman", here you will find sample calls for all endpoints and it's usage. Import the PropellerH.postman_collection file into PostMan. Variables can be used to setup the Auth Tokens across the calls, just remember 3 roles exist, so make sure to use the proper authorization

You can also open the PropellerHeadTechAssessment.Tests.sln solution file on Visual Studio and build. This will generate a set of tests to be run from the Test Explorer. It is suggested you install the SpecFlow extension to debug and inspect the Gherkin code easier.

Make sure you specify the proper path for BASEAPIURL to your local instance on the [specflow.json](PropellerHeadTechAssessment/Propeller.Integration.Tests/specflow.json) configuration file, 

## API Documentation

A basic documentation for the API endpoints and it's usage can be found at the following address (when run locally):

https://localhost:port/swagger/index.html

You will need to set port number to whatever port your application is running in
.

## Features
In this solution you will notice pretty interesting implementations

* Roles  
There exist 3 different Roles for users in the ecosystem: Admin, Power and Regular User


* Authentication using Bearer Token  
A users Table was created with sample seed data for each Role type, the Role and Country of the User is embedded into the Bearer Token to be used in the application  


* Localization  
Three locales supported (en-NZ, es-MX, fr-FR). The User's token contains a locale claim which is used to set the locale of the API (via a Request Culture Provider: TokenBasedRequestCultureProvider), if the token doesn't contain locale, it can be injected as an Accept-Language header  


* Role Access Policy  
This is implemented using Net Core's intrinsic RBAC approach. Regular User only has ReadOnly access, Power User has limited write access and full read access but deletions are restricted, Admin User has full access including deletion


* Attribute Access Policy  
Also an implementation for attribute based policies was made to demonstrate the use of middleware, a IsNZLUser policy was created and can restrict access to certain endpoints to users whose Country code is set to NZL


* Integration Tests
A set of integration tests was created using Specflow and Gherkin, I chose this technology because I like the Scenario approach since it allows for non developers (Product) to create their test scenarios and test them in an easily readable format.


* DB Cardinality
The DB contains approaches to use one-to-many and many-to-many relations. The Customer/Contacts association works in a many-to-many relation to demonstrate it's use with EF and how 

* Minimal use of 3rd party packages
I try to stick as much as possible to Native libraries and custom code whenever possible. You'll notice the application uses only a few 3rd party packages and they are used on the Integration Tests project (Specflow, FluentAssertions and NUnit) and one on the main app (AutoMapper). I prefer to use 3rd party libraries only when they provide a clear advantage (Security, Performance) over custom implementations, exploits on 3rd party libraries are very risky and I've seen many companies don't have a proper culture of active monitoring and updating of libraries to diminish risk


## Features I wanted to implement but ran out of time
- OAuth2
- Unit Tests
- DALC Factory: I wanted to implement a SQL Server alternative to the SQLite approach, if you see the project structure you'll notice I use Entity Framework (Code First) and the Schemas are separated from the actual DB Context, this was created so I could generate the migrations for other DB providers and my intent was to implement a factory or strategy pattern to allow switching DB's for development and production environments.
- One way user/pwd encryption
- User lost password / recovery email feature
- Custom Model Binding for Contact setup to validate on binding for Email and/or Phone existance, currently this is performed at implementation level, I wanted to change this to binding level.
- Refactoring: Most code adheres to the Single Responsibility principle, so code duplication should be minimal, still I think some code can be moved over to Base classes and shared across derived classes.
- More Integration Tests

## Things I wish I would have done different
- AutoMapper, I wanted to give this library a try since I had it recommended by a colleage, I got to say I'm not quite convinced, it seems it's prone to errors and I belive it might have some security issues. Setting it up seemed a bit complicated for complex types and debugging mapping errors is quite hard. For a small application like this one it might not be the best idea
- You'll also notice a few TODO's left on the code, I addressed as many as I could but I ran out of time to address them all
- Start this earlier. We had a pretty busy last week at my current job and couldn't dedicate time until last saturday.  

## Full Disclosure  
I had to refresh my knowledge of Entity Frameworks because for the last 4 years I've only worked on NonSQL environments (Mongo) and we use the MongoDriver approach to query the collections so I wasn't familiar with the current .Net Core EF implementation, the last time I used it was back on .Net FW 4.5

## Final Words to the Reviewer  
Please reach out to me if you need any further info regarding the solution, It was really fun to work on this project and I appreciate you dedicating time to review it, if you have any input on what could be performed better please feel free to leave comments on the code or the repo I'd love to get feedback on this. I hope you have as much fun reviewing this as I had coding it, have a good one and Thank you!

![image](https://user-images.githubusercontent.com/119035054/205457742-4efbdf7f-94d2-47b9-917b-e2e440efcaa2.png)
