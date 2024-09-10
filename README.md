# MonthlyClaimManager

A .NET Core application with a WPF front-end for managing lecturer claims and approvals. The system allows lecturers to submit claims, upload documents, and view claim status, while Programme Coordinators and Academic Managers can verify and approve or reject the claims.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Installation](#installation)
- [Database Structure](#database-structure)
- [Usage](#usage)
- [Design Decisions](#design-decisions)
- [Assumptions and Constraints](#assumptions-and-constraints)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project is designed to streamline the claim submission and approval process for lecturers, Programme Coordinators, and Academic Managers in an academic institution. The system includes a user-friendly WPF interface for easy navigation and use, with a solid .NET Core backend for handling business logic and data processing.

## Features

- Lecturer claim submission form
- Document upload feature for supporting claim submissions
- Real-time status tracking of claims
- Role-based dashboards for Programme Coordinators and Academic Managers
- Claim verification and approval system
- Data stored in a SQL Server relational database

## Technologies Used

- **.NET Core** - For building the backend business logic and APIs
- **Windows Presentation Foundation (WPF)** - For building the desktop GUI
- **SQL Server** - For relational database management
- **Entity Framework Core** - For ORM and database interactions

## Getting Started

These instructions will help you set up and run the project on your local machine for development and testing purposes.

### Prerequisites

- .NET Core SDK 3.1 or higher
- Visual Studio 2019 or higher
- SQL Server (local or remote)
- Entity Framework Core
