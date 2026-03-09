# 🚌 BuStop API

**BuStop** is a robust backend API designed to manage urban **bus routes and stops**.

Built with **.NET 8** and **geospatial technologies**, it allows users to find the **nearest bus routes based on their geographic coordinates** using spatial queries.

---

## ✨ Key Features

### 🔐 Role-Based Access Control (RBAC)
Hierarchical permission system that enables the creation of a scalable community where administrators can manage user permissions.

### ⚡ Optimized Performance
- **Strategic Caching** – Reduces database overhead for frequently requested route data.
- **Efficient Spatial Queries** – Uses geospatial indexing to quickly find nearby routes.

### 🌍 Global Latency Management
Integrated **CDN support** ensures fast delivery of static assets and geographic data regardless of the user’s location.

---

# 🏗️ Architecture

This project follows **Clean Architecture**, separating responsibilities into four independent layers:

| Layer | Responsibility |
|------|------|
| **Domain** | Core entities, business rules, and domain logic |
| **Application** | Use cases, CQRS commands/queries, DTOs, interfaces |
| **Infrastructure** | Database persistence, Identity, EF Core, spatial configuration |
| **WebAPI** | REST endpoints, authentication, middleware |

---

# 🛠️ Tech Stack

## Backend

| Technology | Description |
|-----------|-------------|
| **.NET 8 Web API** | Backend framework |
| **C#** | Programming language |
| **Entity Framework Core** | ORM |
| **MySQL (Pomelo)** | Relational database |
| **NetTopologySuite** | Geospatial spatial queries |
| **MediatR** | CQRS and Mediator pattern |
| **FluentValidation** | Request validation |

---

## Frontend

| Technology | Purpose |
|-----------|-----------|
| **Vue.js** | Frontend framework |
| **HTML5 / CSS3** | Responsive transit dashboards |

---

# 🔐 Security

- **ASP.NET Core Identity**
- **JWT Bearer Authentication**
- **Role-Based Authorization**
- **Stateless sessions**

---

# 🗄️ Data Layer

- **MySQL with Spatial Extensions**
- Geospatial queries with **NetTopologySuite**
- Optimized indexing for route proximity searches

---

# 📌 Future Improvements

- Redis distributed caching
- Route prediction with machine learning
- Real-time bus tracking
- Mobile API support

---

# 👨‍💻 Author

Developed by **Yael Castillo**
