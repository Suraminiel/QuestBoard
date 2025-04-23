# Project management tool (ASP.NET)

This project is a personal portfolio project to demonstrate my knowledge of C#, ASP.NET Core and Entity Framework Core. 
It was developed to implement typical functions of a project management system such as task management, progress display and user management.

## Project description

The tool offers the following functions:

- Creating and managing projects
- Creating and editing tasks (incl. subtasks)
- Progress bar for tasks
- Forum 
- File upload
- User management via ASP. NET Identity
- Data storage with Entity Framework Core and SQL Server
- HTML input security with HtmlSanitizer

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

## Licensing

This project is provided without a license. You are free to view, fork, and use it for personal, non-commercial purposes. 
However, you may not redistribute or modify the code for commercial or non-commercial purposes without obtaining permission from the author.
