# Restaurant Management System - Backend API 🍽️

Backend RESTful API for a restaurant management system built with **ASP.NET Core**.  
The project focuses on **clean architecture**, **business logic separation**, and **secure authentication & authorization** using **JWT, Roles, and Permissions**.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- JWT Authentication
- Role-based Authorization (Admin, Chef, Cashier)
- Permission-based access control (Claims)
- Secure Login flow

### 📦 Orders Management
- Create orders with multiple order items
- Validate inventory before order creation
- Automatic total price calculation
- Transaction handling (Commit / Rollback)
- Order linked to the authenticated user

### 🏪 Inventory Management
- Track inventory per menu item
- Restock inventory
- Prevent orders if stock is insufficient

### 🍔 Menu Items
- Manage restaurant menu items
- Inventory linked to menu items

### 🧠 Clean Architecture
- Controllers (Thin)
- Services (Business Logic)
- Repositories (Data Access)
- DTOs for request/response
- No business logic inside controllers

---

## 🛠️ Tech Stack

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **JWT Authentication**
- **Swagger / OpenAPI**
- **Dependency Injection**
- **Transactions**
- **Role & Permission System**

---

## 📂 Project Structure
