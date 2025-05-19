# Inventory Management Proof-of-Concept Application

## Overview

This project is a proof-of-concept application commissioned by a client, featuring an Angular front-end and a .NET 8 Web API back-end. The application enables users to register, log in, and manage inventory products through a secure and user-friendly interface.

## Features

- **User Authentication**: Secure user registration and login using .NET 8 API authentication mechanisms.
- **Product Management**: Ability to create and retrieve product records stored in a SQL Server 2019 database.
- **Front-end**: Developed using Angular with routing for seamless navigation.
- **Back-end**: Developed with .NET 8 Web API, handling all server-side logic and data persistence.

## Architecture

- **Front-end**: Angular framework for building responsive UI and routing.
- **Back-end**: .NET 8 Web API providing RESTful services.
- **Database**: SQL Server 2019 storing product and user data securely.

## Usage

- The application launches with the **Login** page as the landing screen.
- After authentication, users can navigate through the app using Angular routing.
- Authorized users can add new products and browse existing inventory.

## Setup Instructions

1. **Back-end Setup:**
   - Ensure SQL Server 2019 is installed and configured.
   - Update the connection string in `appsettings.json` to point to your database.
   - Build and run the .NET 8 Web API project.

2. **Front-end Setup:**
   - Navigate to the Angular project folder.
   - Run `npm install` to install dependencies.
   - Run `ng serve` to start the Angular development server.
   - Access the application at `http://localhost:4200`.

## Notes

- Ensure both the API and front-end are running concurrently for full functionality.
- The back-end API enforces authentication on protected endpoints.
- Angular routing manages navigation within the front-end application.

---

## Author
Thuso Khakhu
University of Pretoria: BCom informatics(student): u22586522
