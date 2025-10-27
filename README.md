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
    ## 🧩 Entities and Relationships

### Core Entities
- **User**
  - Has many **Accounts**
  - Has many **Roles** (via `UserRole`)
  - Can act as **Power of Attorney** for other Accounts

- **Role**
  - Has many **Users** (via `UserRole`)
  - Has many **Permissions** (via `RolePermission`)

- **Permission**
  - Belongs to many **Roles** (via `RolePermission`)

- **Bank**
  - Has many **Branches**

- **Branch**
  - Belongs to one **Bank**
  - Has many **Accounts**

- **Account**
  - Belongs to one **User**
  - Belongs to one **Branch**
  - Can have a **Power of Attorney User**
  - Has many **Transactions**

- **Transaction**
  - Linked between two **Accounts** (`FromAccount`, `ToAccount`)

---

### Relationship Summary
- User ↔ Role → Many-to-Many (`UserRole`)
- Role ↔ Permission → Many-to-Many (`RolePermission`)
- Bank → Branch → Account → Transaction → Account (chain)
- User → Account → Transaction (activity flow)
- PowerOfAttorneyUser → Account (optional association)
