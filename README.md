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

---

1. Domain
- Designed **clean, encapsulated domain entities**:
  - `User`, `Role`, `Permission`, `Bank`, `Branch`, `Account`, `Transaction`
- Implemented **`BaseEntity`** with common audit fields.
- Added **value objects** and navigation properties for EF Core relationships.

---

 2. Infrastructure
- Configured **EF Core** with schema separation:
  - Default schema: `training`
  - Migration table moved under `training.__EFMigrationsHistory`
- Created **`ApplicationDbContext`** with full entity mapping.
- Added **Repository Pattern** support and dependency injection setup in `DependencyInjection.cs`.

---

3. Authentication
- Implemented **JWT Authentication using RSA (RS256)**:
  - Generates and validates **access tokens** and **refresh tokens**.
  - Configured via `JwtSettings` in `appsettings.json`:
    ```json
    "JwtSettings": {
      "Issuer": "UAMS",
      "Audience": "UAMSClient",
      "PrivateKeyPath": "Keys/private.key",
      "PublicKeyPath": "Keys/public.key",
      "AccessTokenExpirationMinutes": 30,
      "RefreshTokenExpirationDays": 7
    }
    ```
- `JwtTokenService` handles private key loading, token generation, and validation.
- Integrated with `UserManager<ApplicationUser>` for ASP.NET Identity.

---

4. Exception Handling
- Implemented **global exception middleware** to handle:
  - Validation errors
  - Unauthorized access
  - Database or application-level exceptions
- Returns structured JSON error responses:
  ```json
  {
    "statusCode": 400,
    "message": "Validation failed",
    "errors": [ ... ]
  }
5. Authorization (in progress)
Implemented Role-Based Access Control (RBAC):

Roles and permissions are persisted in the database.

UserSeeder seeds default admin and roles at startup
