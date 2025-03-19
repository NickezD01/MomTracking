# MomTracking - Maternal and Child Health Monitoring Platform

## Overview

MomTracking is a comprehensive health monitoring platform designed for mothers and children. The application enables users to track children's health metrics, compare growth data against WHO standards, manage appointments through a scheduling system, and access premium features through a subscription-based model.

## Key Features

- **Child Health Tracking**: Monitor and record key health metrics including weight, height, head circumference, and other growth parameters
- **Growth Analysis**: Compare children's health data against WHO standards to identify potential health concerns
- **Appointment Scheduling**: Create and manage healthcare appointments with reminder notifications
- **Community Forum**: Post and comment functionality for community support and information sharing
- **Subscription Management**: Tiered subscription plans (Bronze, Silver, Gold) with different features and durations
- **PDF Export**: Generate reports of children's health data for sharing with healthcare providers
- **User Management**: Registration, authentication, and profile management with email verification
- **Payment Processing**: Integration with VNPay for subscription payments

## Tech Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Clean Architecture pattern with Domain, Application, Infrastructure, and API layers
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Validation**: FluentValidation
- **Object Mapping**: AutoMapper
- **Background Processing**: Hangfire for scheduled tasks
- **API Documentation**: Swagger/OpenAPI

### Infrastructure
- **Docker**: Containerization support with docker-compose
- **Email Service**: SMTP email service for notifications and verification

## Architecture

The application follows Clean Architecture principles with four distinct layers:

### Domain Layer
Contains enterprise business rules and entities representing the core business concepts:
- User accounts and children profiles
- Health metrics and WHO standards
- Subscriptions and payment processing
- Community posts and comments
- Scheduling and notifications

### Application Layer
Contains application business rules and use cases:
- Service interfaces and implementations
- Request/Response DTOs
- Validation rules
- Mapping configurations
- Custom exceptions

### Infrastructure Layer
Contains frameworks and implementation details:
- Database context and configurations
- Repository implementations
- Unit of Work pattern implementation
- Email service implementation
- External service integrations

### API Layer
Contains the presentation layer and entry points:
- REST API controllers
- Middleware components
- Authentication configuration
- Exception handling
- API documentation

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Docker (optional)

### Installation

1. Clone the repository
```bash
git clone <repository-url>
cd MomTracking
```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance

3. Apply database migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run --project API
```

5. Access the Swagger documentation at `https://localhost:5001/swagger`

### Docker Deployment

The application can be deployed using Docker:

```bash
docker-compose up -d
```

## API Endpoints

The API provides the following main endpoints:

- **Authentication**: `/api/Auth` - Registration, login, and email verification
- **User Management**: `/api/UserAccount` - User profile management
- **Children**: `/api/Children` - Child profile management
- **Health Metrics**: `/api/HealthMetric` - Health data tracking and analysis
- **WHO Standards**: `/api/WHOStandard` - Access to growth standards
- **Scheduling**: `/api/Schedule` - Appointment management
- **Community**: `/api/Post` and `/api/Comment` - Forum functionality
- **Subscriptions**: `/api/Subscription` and `/api/SubscriptionPlan` - Subscription management
- **Payments**: `/api/Payment` - Payment processing
- **PDF Export**: `/api/PdfExport` - Report generation

## Background Jobs

The application uses Hangfire to run the following scheduled tasks:

- Daily appointment reminders (runs at 00:00)
- Subscription expiration checks (runs at 01:00)

## Security

- JWT authentication with role-based authorization
- Password hashing with HMACSHA512
- Email verification for new accounts
- Subscription-based access control for premium features

## Project Structure
