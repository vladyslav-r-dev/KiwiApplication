# KiwiApp

Full-stack flight booking application built with Angular and ASP.NET Core.

Users can search for flights, choose seats, create bookings, pay through Stripe and manage their bookings. The application also includes authentication, Google Sign-In and an admin dashboard.

## Live Demo

[Open KiwiApp](YOUR_DEPLOYED_URL)

## Screenshots

<!-- Home -->
<!-- Flights -->
<!-- Flight details / seat selection -->
<!-- My Bookings -->
<!-- Admin Dashboard -->

## Features

- Flight search and filtering
- Interactive seat selection
- Multi-passenger bookings
- Stripe Checkout and webhook processing
- JWT authentication with refresh flow
- Google Sign-In
- User profile and personal bookings
- Role-based authorization
- Admin dashboard
- Redis caching
- Background jobs with Hangfire
- Booking confirmation emails
- External flight and weather API integrations

## Tech Stack

**Frontend**
- Angular
- TypeScript
- SCSS
- Reactive Forms
- Angular Signals
- Angular Router

**Backend**
- ASP.NET Core
- C#
- Entity Framework Core
- JWT Authentication
- FluentValidation

**Infrastructure & Integrations**
- PostgreSQL / your actual database
- Redis
- Docker
- Stripe
- Hangfire
- Google Authentication
- MailKit
- External Flight API
- Weather API

## Architecture

The backend is organized into logical layers:

- `Api` — endpoints and HTTP contracts
- `Application` — use cases, services and application models
- `Domain` — domain entities
- `Infrastructure` — database, repositories and external integrations

API contracts are separated from Entity Framework entities and mapped explicitly.

## Testing

- Backend Release build verified
- Angular production build verified
- 18/18 frontend tests passing

## Status

KiwiApp is functionally complete and currently undergoing final repository cleanup.
