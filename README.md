# KiwiApp

Full-stack flight booking application built with Angular and ASP.NET Core.

KiwiApp allows users to search for flights, choose seats, create bookings, pay through Stripe, and manage their trips from a personal account.

The project was built as a production-style full-stack application with authentication, external APIs, payment processing, background jobs, PostgreSQL persistence, and cloud deployment.

## Live Demo

### [Open KiwiApp](https://kiwi-application.vercel.app)

> The backend is hosted on a free Render instance.  
> The first request after a period of inactivity may take a few seconds while the server starts.

---

## Overview

KiwiApp covers a complete flight booking flow:

```text
Sign Up / Sign In
        ↓
Search Flights
        ↓
View Flight
        ↓
Choose Seats
        ↓
Create Booking
        ↓
Stripe Checkout
        ↓
Payment Confirmation
        ↓
My Bookings
```

The application consists of an Angular frontend, an ASP.NET Core REST API, and a PostgreSQL database.

Production infrastructure is hosted using Vercel, Render, and Neon.

---

## Screenshots

<!-- Add project screenshots here -->

<!--

### Home

![Home](docs/screenshots/home.png)

### Flight Search

![Flights](docs/screenshots/flights.png)

### Seat Selection

![Seat Selection](docs/screenshots/seat-selection.png)

### Stripe Checkout

![Stripe Checkout](docs/screenshots/stripe-checkout.png)

### My Bookings

![My Bookings](docs/screenshots/my-bookings.png)

### Admin Dashboard

![Admin Dashboard](docs/screenshots/admin-dashboard.png)

-->

---

## Features

### Flight Search

- Flight search and filtering
- Flight information and route details
- External flight data integration
- Weather data integration
- Production flight data stored in PostgreSQL

### Booking

- Interactive seat selection
- Automatic random seat assignment
- Multi-passenger bookings
- Seat availability tracking
- Dynamic booking price calculation
- Personal booking history
- Booking status tracking

### Payments

- Stripe Checkout
- Server-side checkout session creation
- Stripe webhook processing
- Payment status synchronization
- Successful and cancelled payment flows
- Background post-payment processing

### Authentication

- User registration
- Email/password authentication
- Password hashing
- JWT access tokens
- Refresh token flow
- Secure refresh-token cookies
- Google Sign-In
- Protected API endpoints
- Role-based authorization

### User Account

- User profile
- Personal bookings
- Authentication session restoration
- Protected account pages

### Administration

- Admin role
- Admin dashboard
- Booking management
- Protected administrative endpoints
- Flight import from external API

### Background Processing

- Hangfire background jobs
- Payment reconciliation
- Booking confirmation processing
- PDF booking confirmation generation
- Email confirmation pipeline with MailKit

---

## Tech Stack

### Frontend

- Angular
- TypeScript
- SCSS
- RxJS
- Angular Signals
- Reactive Forms
- Angular Router
- HttpClient

### Backend

- ASP.NET Core
- C#
- .NET 9
- Entity Framework Core
- REST API
- JWT Authentication
- FluentValidation
- MailKit
- Hangfire

### Database

- PostgreSQL
- Neon
- Entity Framework Core migrations
- Repository pattern
- Unit of Work

### Integrations

- Stripe Checkout
- Stripe Webhooks
- Google Identity Services
- Aviationstack API
- OpenWeather API

### Infrastructure

- Vercel
- Render
- Neon
- Docker
- GitHub

---

## Architecture

The backend is divided into logical layers with responsibilities separated between API, application logic, domain models, and infrastructure.

```text
src/KiwiApp
│
├── Api
│   ├── Endpoints
│   ├── Contracts
│   └── Mapping
│
├── Application
│   ├── Services
│   ├── UseCases
│   ├── Interfaces
│   ├── Validation
│   └── Jobs
│
├── Domain
│   └── Entities
│
└── Infrastructure
    ├── Persistence
    └── Repositories
```

### API

Contains HTTP endpoints, request and response contracts, authentication requirements, and API-specific mapping.

### Application

Contains business operations, services, validation, use cases, background jobs, and interfaces.

### Domain

Contains the core application entities and domain models.

### Infrastructure

Contains Entity Framework Core persistence, PostgreSQL integration, and repository implementations.

---

## Backend Design

The backend uses several patterns and practices commonly found in production applications:

- Dependency Injection
- Repository Pattern
- Unit of Work
- DTO / API contract separation
- Explicit entity mapping
- Centralized validation
- Centralized exception handling
- Role-based authorization
- Asynchronous database operations
- Background job processing
- Application-level caching
- Database migrations

API contracts are separated from persistence entities so the HTTP layer does not expose Entity Framework models directly.

---

## Authentication Flow

KiwiApp supports two authentication methods:

```text
Email + Password
       or
Google Sign-In
       ↓
ASP.NET Core API
       ↓
JWT Access Token
+
Refresh Token
       ↓
Authenticated Requests
```

Refresh tokens are stored separately and the browser uses secure cookies for the refresh flow.

Authorization policies protect endpoints that require authenticated users or administrator permissions.

---

## Payment Flow

Stripe Checkout is used for booking payments.

```text
Create Booking
      ↓
ASP.NET Core API
      ↓
Create Stripe Checkout Session
      ↓
Stripe Checkout
      ↓
Payment
      ↓
Stripe Webhook
      ↓
Booking marked as Paid
      ↓
Background confirmation processing
```

The application does not rely only on the browser redirect after payment.

Stripe sends a `checkout.session.completed` webhook directly to the backend, which updates the booking status on the server.

---

## Database

Production data is stored in PostgreSQL hosted on Neon.

Entity Framework Core migrations are applied when the backend starts.

Main entities include:

```text
Users
Flights
Seats
Bookings
Passengers
RefreshTokens
```

Relationships between flights, seats, passengers, bookings, and users are persisted in PostgreSQL.

This allows application data to survive backend restarts and redeployments.

---

## External APIs

### Aviationstack

Used to import real flight information such as:

- Departure airport
- Arrival airport
- IATA codes
- Airline
- Flight number
- Departure time
- Arrival time
- Flight status

Imported flights are persisted in PostgreSQL.

### OpenWeather

Used to provide weather information related to flight destinations.

### Google Identity Services

Provides Google Sign-In alongside regular email/password authentication.

### Stripe

Handles hosted checkout and payment events.

---

## Deployment

KiwiApp is deployed as separate cloud services.

```text
User
 │
 ▼
Angular Frontend
Vercel
 │
 │ HTTPS
 ▼
ASP.NET Core API
Render
 │
 │ EF Core
 ▼
PostgreSQL
Neon
```

External services communicate with the backend:

```text
             ┌───────────────┐
             │ Aviationstack │
             └───────┬───────┘
                     │
                     ▼
┌────────┐      ┌───────────┐      ┌────────────┐
│ Angular│ ───► │ ASP.NET   │ ───► │ PostgreSQL │
│ Vercel │      │ Render    │      │ Neon       │
└────────┘      └───────────┘      └────────────┘
                     ▲
                     │
          ┌──────────┼──────────┐
          │          │          │
       Stripe      Google   OpenWeather
```

---

## Production Flow

The main application flow has been tested in the deployed environment:

- User registration
- Email/password login
- Google Sign-In
- Authentication restoration
- Flight loading
- Flight search
- Seat selection
- Booking creation
- Stripe Checkout
- Stripe webhook processing
- Payment confirmation
- Personal booking history

---

## Testing & Validation

The project includes automated frontend tests and build validation.

Production builds have been verified for both applications:

```text
Angular production build
ASP.NET Core Release build
```

The deployed application has also been tested through the complete user booking and payment flow.

---

## Email Confirmation

The backend contains a booking confirmation pipeline using:

- Hangfire
- MailKit
- PDF generation

After payment confirmation, the backend can generate a PDF booking confirmation and process email delivery as a background job.

Production SMTP provider configuration is being finalized.

---

## Project Structure

```text
KiwiApplication
│
├── frontend
│   └── Angular application
│
├── src
│   └── KiwiApp
│       └── ASP.NET Core API
│
├── Dockerfile
├── KiwiApp.sln
└── README.md
```

---

## Project Status

KiwiApp is deployed and the main end-to-end user flow is working in production.

Current production functionality includes authentication, Google Sign-In, flight search, seat selection, booking creation, Stripe payments, webhook processing, PostgreSQL persistence, and personal booking management.

The project is now focused on final UI polish and production email delivery configuration.
