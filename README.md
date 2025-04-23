# Project management tool (ASP.NET)

This project is a personal portfolio project designed to demonstrate my knowledge of C#, ASP.NET Core, and Entity Framework Core. 
It was developed to implement typical project management system features, such as task management, progress tracking, and user management.

## Project description

This tool offers the following features:

- Creating and managing projects
- Creating and editing tasks (including subtasks)
- Progress bars for tasks
- Forum functionality
- File upload capabilities
- User management with ASP.NET Identity
- Data storage using Entity Framework Core and SQL Server
- HTML input sanitization with HtmlSanitizer

## Third-party libraries

This project uses the following open-source libraries, all licensed under the MIT license:

- [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)
- [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)


## Frontend libraries

- [Bootstrap](https://getbootstrap.com/) – included locally for styling (MIT License)
- [jQuery](https://jquery.com/) – included as a Bootstrap dependency (MIT License)
- [TinyMCE](https://www.tiny.cloud/) – loaded via CDN for rich text editing (core under MIT License, other parts under proprietary license)

## Getting Started

To run this project locally, follow these steps:

1. Clone the repository.
2. Open the solution in Visual Studio.
3. In `appsettings.json`, configure the database connection strings under `"QuestboardDbConnectionString" & "QuestboardAuthDbConnectionString"`.
4. Run the database migrations via the Package Manager Console:
	- Tools -> NuGetPackageManager -> Package Manager Console: "Update-Database"
5. Run the project (F5 or `dotnet run`).

The project uses SQL Server for data storage. Make sure SQL Server is installed and running locally or adjust the connection string accordingly.


