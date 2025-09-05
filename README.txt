
below is the git link to the RestAPI 

https://github.com/Giftthane551/OrdersApi.git

Orders API
Description

A .NET 8 Orders API for managing e-commerce orders and order items.
Supports CRUD operations, order state management, logging, validation, and Docker deployment.

Features

CRUD endpoints for orders (POST, GET, PUT, DELETE)

Validation: Orders must have at least one item

Incremental order numbers

Logging via Serilog

OpenAPI/Swagger support

Dockerized with PostgreSQL

Optional caching (in-memory or Redis)

Event publishing on order status change (via MassTransit/RabbitMQ)

Dependency composition using Docker Compose (API, database, messaging, caching)

How to Run the Project
Prerequisites

Docker & Docker Compose installed

Git installed

Steps

Clone the repository:

git clone <repo-url>
cd <repo-folder>


Ensure Docker is running:

docker --version
docker-compose --version


Run the project using Docker Compose:

docker-compose up --build


This builds and starts all containers: API, PostgreSQL, and any additional services (Redis, RabbitMQ).

To stop the containers:

docker-compose down


Apply database migrations (if not automatically applied):

docker exec -it <api-container-name> dotnet ef database update


Access the API:

this end point will depend on the port that the docker is listening to 

Swagger UI: http://localhost:8080/swagger

Example endpoints:

GET /orders → list all orders

POST /orders → create a new order

PUT /orders/{id} → update an order

DELETE /orders/{id} → delete an order

Assumptions

API runs on .NET 8

PostgreSQL is used for persistence

Order numbers increment sequentially

Event messaging is published but not consumed

Database starts empty unless seeded

Additional Features
Caching

Can be implemented using in-memory cache (IMemoryCache) or Redis for distributed caching.

Example: caching GET /orders/{id} for improved performance.

Messaging / Event Publishing

Events are published when order state changes (Draft → Submitted → Completed).

Implemented with MassTransit and RabbitMQ in Docker.

Events are published but not consumed (ready for future consumers).

Dependency Composition

Docker Compose manages multiple dependencies:

API

PostgreSQL

Redis (optional caching)

RabbitMQ (optional messaging)

Ensures all services start in correct order and network together.

Notes

Database schema is managed via EF Core migrations.

Initial sample data can be seeded using a seeder class (optional).

Swagger provides interactive API testing.

Additionally you can run the system locally so by opening the project with visual studion code and run dotnet run command on the terminal
or you can click run on the http green play button