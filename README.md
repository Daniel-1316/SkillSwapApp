# SkillSwap – Skill Exchange Platform

**Faculty Number:** 2301261011  
**Student:** Daniel Atanasov

**Course:** Distributed Applications  
**Academic Year:** 2025/2026  

---

## 📖 Project Description

SkillSwap is a web-based platform that allows users to exchange skills with each other. Users can publish skills they can teach, search for skills they want to learn, and leave reviews after a successful skill exchange.

The project demonstrates a distributed web application built with ASP.NET Core MVC and a relational database.

---

## 🛠️ Technologies Used

- **Backend:** ASP.NET Core MVC (.NET 8 / .NET 9)
- **Database:** Microsoft SQL Server (LocalDB)
- **ORM:** Entity Framework Core 8.0.11
- **Frontend:** Razor Views, Bootstrap 5, HTML, CSS, JavaScript
- **Authentication:** Session-based authentication
- **Architecture Pattern:** MVC (Model–View–Controller)

---

## ✨ Main Features

### 📊 CRUD Operations
- **Users:** Create, edit, delete, and view users
- **Skills:** Manage skills available for exchange
- **Reviews:** Rating and review system (1–5 stars)

### 🔍 Search & Filter
- **Users:** Search by username or email
- **Skills:** Search by title or category
- **Reviews:** Search by reviewer username

### 📊 Sorting
- **Users:** By username and rating
- **Skills:** By title and category
- **Reviews:** By rating and date

### 📄 Pagination
- 10 records per page
- Navigation with Previous / Page Numbers / Next

### 🔐 Access Levels
- **Guest (Level 0):** View list pages only
- **User (Level 1):** Guest access + Details, Search, Filter, Sort
- **Admin (Level 2):** Full CRUD access

### 🎯 Additional Features
- Automatic calculation of user average rating
- Modern responsive design using Bootstrap 5
- Session-based authentication
- Mobile-friendly layout

---

## 🚀 Installation and Run Guide

### ✅ Requirements

- **Visual Studio 2022 or newer**
- **.NET 8 SDK or .NET 9 SDK**
- **SQL Server LocalDB**
- **Git**

---

### 📥 Step 1: Get the Project

Clone the repository:
```bash
git clone https://github.com/Daniel-1316/distributed-applications-cs.git
cd distributed-applications-cs/course-work/implementations/SkillSwapApp
