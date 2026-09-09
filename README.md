# Examination System
English | [Polski](README_PL.md)

Examination System is a desktop application designed for creating,
assigning and conducting tests, quizzes, assessments, midterm exams, final exams and other forms of examination.

The application was developed in C# using .NET 8
and WPF. Entity Framework Core, LINQ and Microsoft SQL Server
are used for data access and management.

## Project status

**v0.9.0-beta**

The project is currently in beta. The core functionality
of the system has been implemented and is operational, while the application
will continue to be developed towards version 1.0.

## Main features

The system supports three user roles: Administrator, Teacher and Student.
The available functionality depends on the role of the logged-in user.

### Administrator

- user management – adding, editing and viewing users,
- question and answer management, including deletion,
- creating and editing tests, including test deletion,
- assigning questions to tests,
- assigning tests to students,
- grading scale management,
- viewing user login logs.

### Teacher

- creating and editing their own questions and answers,
- creating, editing and archiving their own tests,
- assigning questions to tests,
- assigning tests to students,
- configuring test parameters, including duration and availability.

### Student

- viewing assigned tests together with information about their availability,
- taking tests within a specified availability period and according to the configured number of attempts,
- support for single-choice, multiple-choice and open-ended questions,
- automatic result calculation after completing a test,
- displaying the result according to the settings of a given test.

## Technologies

- C#
- .NET 8
- WPF (Windows Presentation Foundation)
- Entity Framework Core 8
- LINQ
- Microsoft SQL Server
- BCrypt.Net-Next – password hashing
- ClosedXML – exporting data to Excel files
- QuestPDF – generating PDF documents
- JSON – database connection configuration

### Development environment

- Visual Studio 2026 Community
- SQL Server Management Studio (SSMS)

## Project architecture

The solution is divided into three main projects:

### SystemEgzaminow.Core

Contains the core elements shared by the other parts of the system:

- entity models,
- DTOs (Data Transfer Objects),
- working objects (Drafts),
- enumeration types (enums),
- information about the logged-in user's session.

### SystemEgzaminow.Data

Responsible for data access and communication with the database:

- Entity Framework Core,
- `AppDbContext`,
- database context configuration,
- migrations,
- services responsible for data operations.

### SystemEgzaminow.WPF

The desktop application layer responsible for the user interface and application operation:

- WPF views and user controls,
- separate Administrator, Teacher and Student panels,
- login view,
- views related to users, questions, tests, assignments and logs,
- additional windows used for question and test management,
- login and user session handling,
- helper services responsible for password hashing, Excel export and PDF generation,
- database connection configuration based on a JSON file,
- application resources stored in the `Assets` directory,
- communication with the data layer through services.

## Database

The database was designed specifically for the Examination System application.

The system uses Microsoft SQL Server and Entity Framework Core 8 with the Code First approach. The database structure is developed and versioned using EF Core migrations.

The database consists of 16 application tables covering:

- users (`Uzytkownicy`), roles (`Role`) and classes (`Klasy`),
- tests (`Testy`) and test types (`TypyTestow`),
- questions (`Pytania`) and answers (`Odpowiedzi`),
- the many-to-many relationship between tests and questions (`TestPytania`),
- test assignments to students (`PrzypisaneTesty`),
- test results (`WynikiTestow`),
- records of answered questions (`RozwiazanePytania`) and answers (`RozwiazaneOdpowiedzi`),
- grading scales (`SkaleOcen`) and their corresponding thresholds (`ProgiOcen`),
- login logs (`LogiLogowan`),
- test-taking activity logs (`LogiRozwiazywaniaTestu`).

The repository contains two demonstration database scripts:

- `EgzaminyTestyDb_PL.sql` – demonstration data in Polish,
- `EgzaminyTestyDb_EN.sql` – demonstration data in English.

A database relationship diagram is also included in the repository:

`Database/DatabaseDiagram.png`

## Installation and configuration

### Requirements

The following are required to run the project:

- Windows,
- .NET 8,
- Microsoft SQL Server,
- Visual Studio 2022 or Visual Studio 2026 with .NET Desktop development support,
- SQL Server Management Studio (SSMS) – optional, for creating and inspecting the database.

### Database configuration

1. Clone or download the repository.
2. Open the `SystemEgzaminow` solution in Visual Studio 2026.
3. Create the database using one of the included scripts:
   - `Database/EgzaminyTestyDb_PL.sql` – Polish demonstration data,
   - `Database/EgzaminyTestyDb_EN.sql` – English demonstration data.
4. In the `SystemEgzaminow.WPF` project, create a `databaseSettings.json` file based on `databaseSettings.example.json`.
5. Configure your own Microsoft SQL Server connection details in `databaseSettings.json`.

Example configuration:

```json
{
  "ConnectionString": "Server=YOUR_SERVER;Database=EgzaminyTestyDb_PL;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
}
```

### Alternatively – EF Core migrations

The database structure can also be created using Entity Framework Core migrations.
This requires a correctly configured database connection and running the migrations from Visual Studio.

## Demo accounts

After creating the database using one of the included scripts, you can log in using the following demo accounts.

### Polish database – `EgzaminyTestyDb_PL`

| Role | Login | Password |
|---|---|---|
| Administrator | `admin` | `Admin123!` |
| Teacher | `nauczyciel` | `Teacher123!` |
| Student | `uczen` | `Student123!` |

### English database – `EgzaminyTestyDb_EN`

| Role | Login | Password |
|---|---|---|
| Administrator | `admin` | `Admin123!` |
| Teacher | `teacher` | `Teacher123!` |
| Student | `student` | `Student123!` |

Demo account passwords are stored in the database as BCrypt hashes.

## Application presentation

Selected views of the Examination System application are presented below.

### Login

The system allows users to log in and automatically redirects them to the appropriate panel based on their assigned role.

![System login](Screenshots/Login.gif)

### Administrator panel

The Administrator has access to user, question, test, assignment and grading scale management, as well as system login logs.

![Administrator panel](Screenshots/AdminPanel.gif)

### Teacher panel

The Teacher can manage their own questions and tests and assign tests to students.

![Teacher panel](Screenshots/TeacherPanel.gif)

### Student panel

The Student can view assigned tests together with information about their availability, remaining attempts and status. The Student can start an available test and view results when they are made available.

![Student panel](Screenshots/StudentPanel.gif)

## Development plan

The project is still under development. The current `v0.9.0-beta` version contains the core functionality of the system.

Further development will include extending existing modules,
improving the user interface and further testing of the application.

A more detailed development plan is available in [ROADMAP.md](ROADMAP.md).

## Author

**Andrzej Dejnas Vel Denek**

The project was designed and developed as a portfolio application to further develop skills in C#, .NET, WPF, LINQ, Entity Framework Core and SQL Server.

GitHub: `Dejnas24`

## License

Copyright © 2026 Andrzej Dejnas Vel Denek. All rights reserved.

The source code is made publicly available for portfolio presentation purposes.
Use of the source code in other projects requires the author's permission.

For more information, see the [LICENSE](LICENSE) file.
