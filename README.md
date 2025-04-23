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
