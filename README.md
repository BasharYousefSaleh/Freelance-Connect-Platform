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

- 🔐 Secure user authentication and authorization
- 👤 Multi-tier user and admin dashboards
- 📋 Service listing and order management
- 💳 Integration with local and international payment gateways
- 📊 Admin control panel for managing users, services, and orders
- 📱 Fully responsive UI/UX across devices
- 🛡️ Protection against common web vulnerabilities
- 📦 Clean and organized code architecture

---

## 🛠️ Technologies Used

| Layer | Technology |
|-------|------------|
| **Backend** | ASP.NET, C# |
| **ORM** | Entity Framework |
| **Database** | SQL Server |
| **Frontend** | HTML5, CSS3, SCSS, JavaScript |
| **Architecture** | MVC, Repository Pattern |
| **Security** | Authentication, Authorization, Secure Payment Integration |

---

## 🏗️ Architecture

The project follows a **layered architecture**:

┌─────────────────────────────────────┐
│ Presentation Layer │
│ (ASP.NET MVC Views) │
├─────────────────────────────────────┤
│ Business Logic Layer │
│ (Services Layer) │
├─────────────────────────────────────┤
│ Data Access Layer │
│ (Entity Framework + Repos) │
├─────────────────────────────────────┤
│ Database Layer │
│ (SQL Server) │
└─────────────────────────────────────┘


---

## 📂 Project Structure
Freelance-Connect-Platform/
│
├── src/
│ ├── Data/ # Database context and migrations
│ ├── Hoor.Common/ # Shared utilities and helpers
│ ├── Services/ # Business logic layer
│ ├── Tests/ # Unit tests
│ ├── Web/ # ASP.NET MVC web application
│ ├── Hoor.sln # Main solution file
│ ├── src.sln # Source solution file
│ └── Rules.ruleset # Code analysis rules
│
└── .gitignore

---

## 🚀 How to Run

### Prerequisites

- Visual Studio 2022 or later
- .NET Framework / .NET SDK
- SQL Server (LocalDB or full instance)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/BasharYousefSaleh/Freelance-Connect-Platform.git

 2. Open the solution
   Open src/Hoor.sln in Visual Studio

3 Configure the database

  Update the connection string in Web.config or appsettings.json.
  Run Entity Framework migrations:
  Update-Database
  
4. Build and run
    Press F5 in Visual Studio
   
🎓 Academic Context
  This project was developed as a graduation project for the Bachelor of Science in Information Technology at Al-Hikma University, Dhamar, Yemen.
  
  Project Grade: 95% (Excellent)
  
  Final Semester GPA: 93% (Excellent)
  
  Duration: November 2024 – February 2025

👤 Author
  Bashar Yousef Saleh Al-Kharbi
  
  🎓 B.Sc. in Information Technology — Al-Hikma University (2025)
  📧 bsharalkhrby92@gmail.com  
  🔗 GitHub: @BasharYousefSaleh
  
📄 License
  This project is intended for academic and personal use. For commercial use, please contact the author.
  
  
⭐ If you find this project useful, please consider giving it a star!





