# ArchitectureDemo WebApi

This project provides a structure for creating a simple or complex web API application. It includes two example API controllers and operations for handling them. Additionally, it demonstrates unit testing using mock data as well as direct testing with data from a database.

When the project is run in development mode, a web browser will automatically open and navigate to the local host. This will display the Swagger UI, which provides detailed documentation of the API endpoints. You can find comprehensive descriptions of each endpoint, making it easy to test and interact with the API directly from the browser.

## Technical Details
- IDE: Visual Studio 2022
- Technologies: .NET 8.0, Entity Framework, MSSQL


## How to run
1. Clone the project to your local machine using Visual Studio 2022.
2. Create a database using Microsoft SQL Server Management Studio.
3. Create the Products table. The script can be found in ArchitectureDemo.DbContext (Scripts/Tables/Product.sql).
4. Insert test data from the ArchitectureDemo.Tests.Unit project (TestData/Products.sql).
5. Edit the connection strings in the appsettings files for both ArchitectureDemo.Tests.Unit and ArchitectureDemo.Web projects.
6. Run the ArchitectureDemo.Web project.

## Unit Testing with Mock or Database Data
1. Switch between using mock or database data by modifying the "UseInMemoryDatabase" setting in appsettings.Development.json within the ArchitectureDemo.Tests.Unit project.
2. Set the JSON file location for mock data in the BaseUnitTest.LoadMockData method (Sample data can be found in TestData/Products.json).
3. Execute some tests.

## API Specification
The project includes a total of four test endpoints: three for version 1.0 and one for version 2.0. Detailed documentation for these endpoints can be found in Swagger UI (accessible only in development mode).

**List all available products**

* **URL v1**

   `GET` /api/v1/products
* **URL v2**

   `POST` /api/v2/products

    **Required in body:**
 
   `page=[integer]`, `pageSize=[integer]`

**Retrieve one product by ID**

* **URL**

   `GET` /api/v1/productS/{product_id}

**Update product description**

* **URL**

   `PATCH` /api/v1/productS/{product_id}/description

  **Required in body:**
 
   `description=[string]`
