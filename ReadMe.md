# Mindex Coding Challenge
## The Project
A simple [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) web application has been created and bootstrapped 
with data. The application contains information about all employees at a company. On application start-up, an in-memory 
database is bootstrapped with a serialized snapshot of the database. While the application runs, the data may be
accessed and mutated in the database without impacting the snapshot.

You can run this by executing `dotnet run` on the command line or in [Visual Studio Community Edition](https://www.visualstudio.com/downloads/).

## How to Use
### Employee Datatype
The following endpoints are available to use:
```
* CREATE
    * HTTP Method: POST 
    * URL: localhost:8080/api/employee
    * PAYLOAD: Employee
    * RESPONSE: Employee
* READ
    * HTTP Method: GET 
    * URL: localhost:8080/api/employee/{id}
    * RESPONSE: Employee
* UPDATE
    * HTTP Method: PUT 
    * URL: localhost:8080/api/employee/{id}
    * PAYLOAD: Employee
    * RESPONSE: Employee
```
The Employee has a JSON schema of:
```json
{
  "type":"Employee",
  "properties": {
    "employeeId": {
      "type": "string"
    },
    "firstName": {
      "type": "string"
    },
    "lastName": {
          "type": "string"
    },
    "position": {
          "type": "string"
    },
    "department": {
          "type": "string"
    },
    "directReports": {
      "type": "array",
      "items" : "string"
    }
  }
}
```
For all endpoints that require an "id" in the URL, this is the "employeeId" field.

### Compensation Datatype
The following endpoints are available to use:
```
* CREATE
    * HTTP Method: POST 
    * URL: localhost:8080/api/compensation
    * PAYLOAD: Compensation
    * RESPONSE: Compensation
* READ
    * HTTP Method: GET 
    * URL: localhost:8080/api/compensation/{id}
    * RESPONSE: Compensation
* UPDATE
    * HTTP Method: PUT 
    * URL: localhost:8080/api/compensation/{id}
    * PAYLOAD: Compensation
    * RESPONSE: Compensation
```
The Compensation has a JSON schema of:
```json
{
  "type":"Compensation",
  "properties": {
    "CompensationId": {
      "type": "string"
    },
    "Employee": {
      "type": "Employee"
    },
    "Salary": {
          "type": "double"
    },
    "EffectiveDate": {
          "type": "DateTime"
    }
  }
}
```
For all endpoints that require an "id" in the URL, this is the "CompensationId" field.
The Employee field is the id of an Employee object. This is required and unique for every Compensation.

### Reporting Structure Datatype
The following endpoints are available to use:
```
* READ
    * HTTP Method: GET 
    * URL: localhost:8080/api/reporting_structure/show/{employee_id}
    * RESPONSE: Fully Detailed Reporting Structure
```
The Compensation has a JSON schema of:
```json
{
  "type":"Reporting Structure",
  "properties": {
    "ReportingStructureId": {
      "type": "string"
    },
    "Employee": {
      "type": "Employee"
    },
    "NumberOfReports": {
          "type": "int"
    }
  }
}
```
The read endpoint returns a fully detailed reporting structure for an employee.
All of the direct reports are included with full details, and recurse down.
The output for an Employee's ReportingStructure will look as follow"
```json
{
    "reportingStructureId": null,
    "employee": {
        "employeeId": "16a596ae-edd3-4847-99fe-c4518e82c86f",
        "firstName": "John",
        "lastName": "Lennon",
        "position": "Development Manager",
        "department": "Engineering",
        "directReports": [
            {
                "employeeId": "b7839309-3348-463b-a7e3-5de1c168beb3",
                "firstName": "Paul",
                "lastName": "McCartney",
                "position": "Developer I",
                "department": "Engineering",
                "directReports": []
            },
            {
                "employeeId": "03aa1462-ffa9-4978-901b-7c001562cf6f",
                "firstName": "Ringo",
                "lastName": "Starr",
                "position": "Developer V",
                "department": "Engineering",
                "directReports": [
                    {
                        "employeeId": "62c1084e-6e34-4630-93fd-9153afb65309",
                        "firstName": "Pete",
                        "lastName": "Best",
                        "position": "Developer II",
                        "department": "Engineering",
                        "directReports": []
                    },
                    {
                        "employeeId": "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                        "firstName": "George",
                        "lastName": "Harrison",
                        "position": "Developer III",
                        "department": "Engineering",
                        "directReports": []
                    }
                ]
            }
        ]
    },
    "numberOfReports": 4
}
```


##Future Concerns
###Testing

Currently, testing creates an initial Context during a test suite, and does not
reset this for the duration of the tests. This can cause instability for future
tests, and reduces the readability of the current tests. For the time,
I've separated any destructive tests to run on their own dedicated entities.
This is a stopgap until further changes are necessary for this repo.

Ideally, we have the Context re-seed between unit tests, or we mock the repositories
so that no changes are saved.