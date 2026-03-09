🚌 BuStop API
BuStop is a robust backend API designed to manage urban bus routes and stops. Built with modern .NET 8, it leverages spatial data (GIS) to provide real-time proximity searches, allowing users to find the nearest bus routes based on their geographic coordinates.

Key Features

Role-Based Access Control (RBAC): Hierarchical permission system, encouragint a big community.

Optimized Performance: * Strategic Caching: Reduces database overhead for frequently requested route data.

Global Latency Management: Integrated CDN support to ensure fast response times for static assets and geographic data regardless of the user's location.

🏗️ Architecture
This project strictly follows the Clean Architecture principles, divided into four highly decoupled layers:

Domain: Contains the fundamental entities, core logic, and rules that govern the bus system.. 

Application: Contains the business logic, Use Cases (CQRS Commands and Queries), DTOs, Interfaces and so on.

Infrastructure: Handles all technical details and external communications, including database persistence (EF Core), Identity management, and spatial library configurations.

WebApi (Presentation): The external interface. Manages the RESTful API endpoints, JWT authentication, and request middleware.

🛠️ Tech Stack
Framework: .NET 8 Web API

Language: C#

Database: MySQL (via Pomelo.EntityFrameworkCore.MySql)

ORM: Entity Framework Core

Geospatial Library: NetTopologySuite (NTS)

Mediator Pattern: MediatR (LuckyPennySoftware)

Validation: FluentValidation

Frontend & UI
Framework: Vue.js

Styling: Modern HTML5 & CSS3 for responsive transit dashboards.

Data & Security
Database: MySQL with Spatial Extensions.

ORM: Entity Framework Core.

Identity: ASP.NET Core Identity with JWT Bearer tokens for secure, stateless sessions.