MonthlyClaimManager
A .NET Core MVC application for managing lecturer claims and approvals. The system allows lecturers to submit claims, upload documents, and view claim statuses, while Programme Coordinators and Academic Managers can verify and approve or reject the claims.

Table of Contents
Introduction
Features
Technologies Used
Getting Started
Installation
Database Structure
Usage
Design Decisions
Assumptions and Constraints
Contributing
License
Introduction
This project is designed to streamline the claim submission and approval process for lecturers, Programme Coordinators, and Academic Managers in an academic institution. The system provides a user-friendly interface using MVC for easy navigation and use, with a solid .NET Core backend for handling business logic and data processing.

Features
Lecturer claim submission form with document upload
Real-time status tracking of claims (Pending, Approved, Rejected)
Role-based dashboards for Programme Coordinators and Academic Managers
Admin functionalities for approving or rejecting claims
Data stored in a SQL Server relational database
Notifications via SignalR for real-time updates across users
Unit tests for critical components
Technologies Used
.NET Core MVC - For building the backend and front-end web pages
SQL Server - For relational database management
Entity Framework Core - For ORM and database interactions
SignalR - For real-time updates between users
xUnit & Moq - For unit testing
Getting Started
These instructions will help you set up and run the project on your local machine for development and testing purposes.

Prerequisites
.NET Core SDK 6.0 or higher
Visual Studio 2019 or higher
SQL Server (local or remote)
Entity Framework Core

Link to Git Hub Repo:

https://github.com/skxnk-hxnt42/MontlyClaimManeger

json
Copy code
"ConnectionStrings": {
   "DefaultConnection": "Server=your-server;Database=MonthlyClaimManager;Trusted_Connection=True;"
}
Run Entity Framework migrations to create the database:

bash
Copy code
Update-Database
Build and run the application in Visual Studio by pressing F5 or running the command:

bash
Copy code
dotnet run
Running Unit Tests
To run the unit tests for this project, execute the following command:

bash
Copy code
dotnet test
This will execute the test cases written for the project using the xUnit framework.

Usage
Open the project in a web browser after starting the application.
Log in as a lecturer or an admin in separate browser tabs to observe the real-time updates.
Lecturer Login
Username: lecturer
Password: lecturer
Admin Login
Username: admin
Password: admin
Workflow
Lecturer Submission: Lecturers can log in and submit their claims using the claim form. They can upload supporting documents (PDF, DOCX, XLSX) and see the status of their claims in real-time.

Admin Review: Programme Coordinators and Academic Managers (admins) can log in to their dashboard and approve or reject pending claims. Once a claim is approved or rejected, the lecturer is notified in real-time of the status change.

Design Decisions
MVC Architecture: Used MVC for better separation of concerns, with controllers handling business logic, views handling presentation, and models handling data interaction.
Real-Time Updates: SignalR was integrated for real-time notifications, ensuring lecturers can immediately see when their claims are processed by admin.
File Uploads: Allowed document uploads with size and format validation, ensuring only appropriate files are accepted.
Assumptions and Constraints
Admins are responsible for verifying the authenticity of claims.
Lecturers can submit claims once per month, and admins must review these claims before processing.
The system should ensure that any claim modification triggers a real-time update for all users viewing the system.
Contributing
Feel free to submit pull requests to suggest improvements or fixes. Ensure unit tests are passing before submission.

License
This project is licensed under the MIT License.
