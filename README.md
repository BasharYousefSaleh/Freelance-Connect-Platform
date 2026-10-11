 # 🚀 Freelance Connect Platform

A secure full-stack web platform that connects freelancers with project owners — enabling service listing, order management, and secure payments.

![ASP.NET](https://img.shields.io/badge/ASP.NET-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat&logo=microsoft-sql-server&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=flat&logo=dotnet&logoColor=white)

---

## 📌 Overview

**Freelance Connect Platform** is a graduation project that provides a digital ecosystem where freelancers and clients can interact securely. It enables users to create accounts, publish services, submit and receive orders, and complete payments through integrated local and international payment gateways.

The platform is built with a focus on **security**, **scalability**, and **user experience**, following modern software engineering practices such as the MVC pattern, Repository pattern, and Entity Framework ORM.

---

## ✨ Key Features

The platform provides a comprehensive set of features designed to deliver a complete freelance experience:

### 👤 User Management
- **Secure Registration:** Users can create accounts with verified email and secure password storage.
- **Profile Management:** Freelancers can build detailed profiles showcasing their skills and services.
- **Multi-Role System:** Separate dashboards for Users, Freelancers, and Admins.

### 📋 Service & Order Management
- **Service Listing:** Freelancers can publish and manage their services.
- **Order Workflow:** Clients can browse, order, and track services.
- **Status Tracking:** Real-time updates on order progress and delivery.

### 💳 Payments
- **Local & International Gateways:** Integration with trusted payment providers.
- **Secure Transactions:** End-to-end encrypted payment flow.
- **Transaction History:** Complete audit trail for all payments.

### 📊 Admin Control
- **User Management:** Admins can manage users, freelancers, and permissions.
- **Content Moderation:** Review and approve service listings.
- **Analytics Dashboard:** Monitor platform activity and performance.

### 📱 User Experience
- **Responsive Design:** Works seamlessly on desktop, tablet, and mobile.
- **Intuitive Interface:** Clean, modern UI focused on usability.
- **Fast Performance:** Optimized for speed and reliability.

---

## 🛠️ Technologies Used

The platform is built using a modern, secure, and scalable technology stack:

### Backend
- **ASP.NET:** Core web framework for building robust server-side applications.
- **C#:** Primary programming language for business logic.
- **Entity Framework:** ORM for efficient and secure database operations.

### Frontend
- **HTML5 & CSS3:** Semantic markup and modern styling.
- **SCSS:** Advanced CSS preprocessing for maintainable styles.
- **JavaScript:** Interactive client-side functionality.

### Database
- **SQL Server:** Relational database for structured data storage.
- **Entity Framework Migrations:** Version-controlled database schema.

### Architecture & Patterns
- **MVC Pattern:** Clean separation of concerns.
- **Repository Pattern:** Abstraction of data access logic.
- **Layered Architecture:** Modular and maintainable codebase.

### Security
- **Authentication & Authorization:** Secure user access control.
- **Data Encryption:** Protection of sensitive information.
- **Secure Payment Integration:** PCI-compliant payment handling.

---

## 🔐 Security Implementation

Security was a core focus throughout the development of this platform. The following measures were implemented to ensure data integrity, user privacy, and protection against common web vulnerabilities:

### 🛡️ Authentication & Authorization
- **Secure Registration:** Passwords are hashed using strong cryptographic algorithms before storage.
- **Role-Based Access Control (RBAC):** Separate roles for Users, Freelancers, and Admins with restricted permissions.
- **Session Management:** Secure session tokens with proper expiration and renewal.
- **Account Verification:** Email and identity verification before account activation.
- **Brute Force Protection:** Account lockout after multiple failed login attempts.

### 🔒 Data Protection
- **SQL Injection Prevention:** All database queries use **Entity Framework ORM** with parameterized queries.
- **XSS Protection:** Input sanitization and output encoding to prevent Cross-Site Scripting attacks.
- **CSRF Protection:** Anti-forgery tokens on all forms and state-changing requests.
- **Encrypted Sensitive Data:** Personal and payment-related data encrypted at rest.
- **Secure Connection Strings:** Connection strings stored securely, never hard-coded.

### 💳 Payment Security
- **PCI-Compliant Gateways:** Integration with trusted local and international payment providers.
- **No Card Data Storage:** Sensitive payment details are never stored on the platform.
- **Secure Payment Flow:** Encrypted communication between the platform and payment gateways.
- **Transaction Logging:** Full audit trail for every payment transaction.
- **Refund & Dispute Handling:** Built-in mechanisms for secure refunds and disputes.

### 🌐 Network & Infrastructure
- **HTTPS Enforcement:** All traffic encrypted using SSL/TLS.
- **Secure Headers:** HTTP security headers (HSTS, X-Frame-Options, CSP).
- **Rate Limiting:** Protection against DDoS and abuse.
- **Logging & Monitoring:** Detailed logs for security events and suspicious activities.

### 🧪 Security Testing
- **Penetration Testing:** Manual testing of authentication, authorization, and payment flows.
- **Vulnerability Scanning:** Regular scans for OWASP Top 10 vulnerabilities.
- **Code Review:** Security-focused review of all critical components.

### 📋 Security Compliance
- Follows **OWASP Top 10** best practices.
- Adheres to **secure coding standards** for ASP.NET applications.
- Implements **least privilege principle** across all system components.
- Ensures **GDPR-inspired** data handling for user privacy.

---

## 🏗️ Architecture

The project follows a **layered architecture**:

### 📐 System Layers
- **Presentation Layer:** ASP.NET MVC Views and Controllers.
- **Business Logic Layer:** Services handling core application logic.
- **Data Access Layer:** Entity Framework and Repository implementations.
- **Database Layer:** SQL Server with versioned migrations.

### 🔄 Design Patterns
- **MVC Pattern:** Separation of model, view, and controller.
- **Repository Pattern:** Centralized data access abstraction.
- **Dependency Injection:** Loose coupling between components.
- **Unit of Work:** Transaction management across repositories.

### 📊 Data Flow

User Request → Controller → Service → Repository → Database
↓
User Response ← View ← Model ← Service ← Repository

---

## 📂 Project Structure

The repository is organized for clarity and maintainability:

### 📁 Root Directory
- **src/**: Main source code folder.
- **.gitignore**: Git ignore rules for build artifacts.

### 📁 src/ Directory
- **Data/**: Database context, migrations, and configurations.
- **Hoor.Common/**: Shared utilities, helpers, and extensions.
- **Services/**: Business logic and service implementations.
- **Tests/**: Unit and integration tests.
- **Web/**: ASP.NET MVC web application (controllers, views, models).
- **Hoor.sln**: Main Visual Studio solution file.
- **src.sln**: Alternative solution file for source-only builds.
- **Rules.ruleset**: Code analysis and style rules.

---

## 🚀 How to Run

Follow these steps to run the project locally:

### 📋 Prerequisites
- **Visual Studio 2022** or later.
- **.NET Framework** or **.NET SDK** installed.
- **SQL Server** (LocalDB or full instance).

 ### ⚙️ Setup Steps

- **Step 1 — Clone the repository:**
  ```bash
  git clone https://github.com/BasharYousefSaleh/Freelance-Connect-Platform.git
  ```

- **Step 2 — Open the solution:**
  Open `src/Hoor.sln` in Visual Studio.

- **Step 3 — Configure the database:**
  Update the connection string in `Web.config` or `appsettings.json`.

- **Step 4 — Run migrations:**
  ```
  Update-Database
  ```

- **Step 5 — Build and run:**
  Press `F5` in Visual Studio.

---

## 🎓 Academic Context

This project was developed as a **graduation project** for the Bachelor of Science in Information Technology.

### 🏫 University
- **Al-Hikma University**, Dhamar, Yemen.

### 📊 Academic Performance
- **Project Grade:** 95% (Excellent).
- **Final Semester GPA:** 93% (Excellent).
- **Overall GPA:** 77.06% (Good).

### 📅 Timeline
- **Duration:** November 2024 – February 2025.

---

## 👤 Author

**Bashar Yousef Saleh Al-Kharbi**

### 🎓 Education
- B.Sc. in Information Technology — Al-Hikma University (2025).

### 📬 Contact
- 📧 Email: bsharalkhrby92@gmail.com
- 🔗 GitHub: [@BasharYousefSaleh](https://github.com/BasharYousefSaleh)
- 📱 Phone: +967 778014486

### 🎯 Interests
- Full-Stack Web Development
- Mobile Application Development (Flutter)
- Artificial Intelligence & Big Data
- Cloud Computing & Cybersecurity

---

## 📄 License

### ✅ Permitted Use
- Academic and personal use.

### ⚠️ Commercial Use
- For commercial use, please contact the author.

### 📧 Contact for Licensing
- Email: bsharalkhrby92@gmail.com

---

⭐ If you find this project useful, please consider giving it a star!
