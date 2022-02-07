# HeyURL! Code Challenge - Complete solution with unit tests

This repository has been created with the entire application working properly and unit tests created in the challenge.

In web application, all views are working. The "Error" action in "Home" controller was added por exception handling of the application.

The connection string is pointing to an SQL Express instance, but you can change it to use an "In Memory Database" if you want:

>services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase(databaseName: "HeyUrl"));

In the "Test" folder the unit test for web and application layers were created by using NUnit and Moq libraries.
  
To run the application do the following:
  1. Restore nuget packages.
  2. Change the "default" connection string in the "appsetting.development.json" or
  3. Change HeyUrlDbContext to use in memory database into Startup.cs file.
  4. Build and run the application web. 
  
NOTE: The bonus task was not done.
