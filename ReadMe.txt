Acme Microservice Demo – Production‑Grade Azure Microservices Architecture

Author
Anita Sure – Lead Engineer (.NET | Cloud | Microservices)

________________________________________
📌 Project Objective
Build a real‑world, production‑ready microservice system using:

• ASP.NET Core Web APIs
• Clean Architecture & SOLID principles
• API Gateway with Azure API Management
• MassTransit messaging
• RabbitMQ (local) → Azure Service Bus (cloud)
• Azure App Service hosting
• Serilog + Application Insights logging
• Distributed tracing with CorrelationId
• CI/CD pipelines with automated build, test & artifact generation
• Multi-layer testing framework (Unit + Integration)
• Cloud-ready, scalable, and maintainable design

This project demonstrates enterprise microservice engineering practices.
________________________________________

🧩 Microservices in the System
Service	Responsibility
Product Service	Product CRUD, publish ProductCreated
UserProfile Service	User CRUD, publish UserCreated
Recommendation Service	Consumes product & user events
________________________________________

🏗 Architecture Overview

Client / Browser
      │
      ▼
API Gateway (optional)
      │
      ▼
Product API ───▶ MassTransit ───▶ Message Broker
User API    ───▶               │
                              ▼
                    Recommendation Service

Logging:
Serilog → Application Insights → Log Analytics

Tracing:
CorrelationId flows across HTTP + Messaging
________________________________________

⚙ Technology Stack
•	.NET 9
•	ASP.NET Core Web API
•	MassTransit
•	RabbitMQ (local)
•	Serilog
•	Azure Application Insights
•	Azure App Service (Linux)
•	Entity Framework InMemory DB

________________________________________
🧠 Key Design Decisions
1.	Messaging provider is configurable:
 	Messaging:Provider = RabbitMQ | InMemory | AzureServiceBus
2.	MassTransit is always registered — transport changes only.
3.	No direct broker dependency in controllers.
4.	Observability is first‑class.
________________________________________

🚀 Getting Started - local setup

1. Prerequisites
• .NET 9 SDK
• IDE: Visual Studio 2022 / VS Code
• RabbitMQ (for local messaging)
• Node.js
• Angular CLI

2. Clone the Repository

git clone
git clone https://github.com/avsure/Acme-Microservice-Demo.git
cd Acme-Microservice-Demo

3. Install, Setup and Run RabbitMQ Locally

4. Configure Messaging Provider
In appsettings.json, set:
"Messaging": {
    "Provider": "RabbitMQ"
}

5. Build and Run the Solution

dotnet build

dotnet build
dotnet run --project src/ApiGateway/ApiGateway
dotnet run --project src/Acme.ProductService/Acme.ProductService.Api
dotnet run --project src/Acme.UserProfileService/Acme.UserProfileService.Api
dotnet run --project src/Acme.RecommendationsService/Acme.RecommendationsService.Api


6. Access APIs
Product API: http://localhost:5047/api/products
UserProfile API:  https://localhost:7149/api/users
Recommendation API: https://localhost:7191/api/recommendations
________________________________________

🚀 Getting Started - Azure Cloud Setup

1. Prerequisites
• .NET 9 SDK
• IDE: Visual Studio 2022 / VS Code
• RabbitMQ (for local messaging)
• Azure Subscription (for cloud deployment)
• Node.js
• Angular CLI

2. Fallow local setup first and make it ready.

3. Create Azure Resources

• Resource Group
• App Service Plan
• Azure App Service for each service (Linux)
• Azure API Management (APIM)
• Azure Application Insights
• Log Analytics Workspace

Configure all the resources and APIM

API Gateway Responsibilities

•	Single public endpoint
•	Route:
o	/products → Product Service
o	/users → UserProfile Service
o	/recommendations → Recommendation Service

•	Add:
o	API keys
o	Rate limits
o	Versioning
o	Central logging

flowchart LR
    User[Browser / Angular UI]

    APIM[Azure API Management<br/>Gateway]

    Product[Product Service<br/>Azure App Service]
    UserProfile[User Profile Service<br/>Azure App Service]
    Recomm[Recommendation Service<br/>Azure App Service]

    Rabbit[(RabbitMQ<br/>Docker / VM)]

    AppInsights[Application Insights]
    LogAnalytics[Log Analytics Workspace]

    GitHub[GitHub Repository]
    CI[GitHub Actions CI/CD]

    ACR[Azure Container Registry]

    User --> APIM

    APIM --> Product
    APIM --> UserProfile
    APIM --> Recomm

    Product --> Rabbit
    UserProfile --> Rabbit
    Rabbit --> Recomm

    Product --> AppInsights
    UserProfile --> AppInsights
    Recomm --> AppInsights
    APIM --> AppInsights

    AppInsights --> LogAnalytics

    GitHub --> CI
    CI --> ACR
    CI --> Product
    CI --> UserProfile
    CI --> Recomm

4. Once Azure setup is run apis using APIM urls using Postman.

________________________________________

⚠ Issues Solved During Project
Issue	Solution
IPublishEndpoint DI failure	Always register MassTransit
Ambiguous routing	Fixed duplicate routes
CorrelationId null	Middleware storage fixed
RabbitMQ Azure issues	Switched to transport abstraction
Logs not visible	Application Insights integration
________________________________________

📌 Learning Outcomes
•	Cloud‑ready microservice design
•	Messaging abstraction
•	Observability architecture
•	Production troubleshooting
•	Azure hosting behavior
________________________________________

🏆 Portfolio Value
This project demonstrates:
•	Enterprise microservices
•	Distributed tracing
•	Messaging architecture
•	Azure observability
•	Clean architecture principles
________________________________________

📦 Future Enhancements
•	Azure Service Bus transport
•	API Gateway
•	Docker + Kubernetes
•	Circuit breakers
•	Retry policies
•	Event versioning
•	Schema registry
________________________________________

🧠 Final Note
This project is a complete enterprise‑grade microservice reference implementation built for learning,
portfolio, and real‑world application.
________________________________________







   

