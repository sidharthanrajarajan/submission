 🧭 User and Account Management System (UAMS)

 📘 Overview
    The **User and Account Management System (UAMS)** is a backend service built on **ASP.NET Core (.NET 9)**. 

Folder Structure:
 ---
    UserAndAccountManagementSystem/
    │
    ├── UAMS.API/              → Presentation Layer (Controllers, Swagger, Auth)
    ├── UAMS.Application/      → Application Core (CQRS, MediatR, Validation, Mapping)
    ├── UAMS.Domain/           → Enterprise Core (Entities, Value Objects, Interfaces)
    └── UAMS.Infrastructure/   → Data & External Services (EF Core, Repos, Providers)
