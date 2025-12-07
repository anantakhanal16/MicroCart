[microcart-readme.md](https://github.com/user-attachments/files/24016634/microcart-readme.md)
# 🛒 MicroCart E-Commerce Platform

A production-ready microservices e-commerce backend built with .NET Core, Docker, and event-driven architecture.

![Microservices](https://img.shields.io/badge/Architecture-Microservices-blue)
![.NET](https://img.shields.io/badge/.NET-Core-purple)
![Docker](https://img.shields.io/badge/Docker-Compose-blue)
![RabbitMQ](https://img.shields.io/badge/Message%20Broker-RabbitMQ-orange)

## 📖 About

MicroCart demonstrates modern microservices architecture with .NET Core. It features customer management, product catalog, order processing, JWT authentication, and event-driven communication through RabbitMQ.

**Perfect for:**
- Learning microservices architecture
- Understanding event-driven design
- Building scalable e-commerce systems
- Portfolio projects

## ✨ Key Features

| Feature | Description |
|---------|-------------|
| 🏗️ **Microservices** | Independently deployable services |
| 🌐 **Ocelot Gateway** | Intelligent API routing |
| 🔐 **JWT Auth** | Secure token-based authentication |
| 📨 **Event-Driven** | Async messaging with RabbitMQ |
| 💾 **Polyglot Persistence** | SQL Server + MySQL + MongoDB |
| 🐳 **Docker** | One-command deployment |
| ⚡ **Async/Await** | Non-blocking operations |

## 🚀 Quick Start

### Prerequisites
- Docker Desktop ([Download](https://www.docker.com/products/docker-desktop))
- 4GB RAM available
- Git

### Setup (3 Steps)

**1. Clone and Navigate**
```bash
git clone https://github.com/your-username/microcart.git
cd microcart
```

**2. Start All Services**
```bash
docker-compose up -d
```

**3. Verify**
```bash
docker-compose ps
```

All services should show "Up" status. Wait 30 seconds for databases to initialize.

### Access Points

- **API Gateway:** http://localhost:8085
- **RabbitMQ UI:** http://localhost:15672 (guest/guest)

### Test the API

**Login:**
```bash
curl -X POST http://localhost:8085/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

**Create Customer:**
```bash
curl -X POST http://localhost:8085/api/customer \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com"}'
```

**Create Order (triggers RabbitMQ event):**
```bash
curl -X POST http://localhost:8085/api/order \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"items":[{"productId":"p1","quantity":2,"price":29.99}],"totalAmount":59.98}'
```

## 🏗️ Architecture

```
┌─────────────┐
│   Clients   │
└──────┬──────┘
       │
       ▼
┌─────────────────┐
│  API Gateway    │ (Ocelot - Port 8085)
│                 │
└────────┬────────┘
         │
    ┌────┴────┬─────────┬──────────┐
    ▼         ▼         ▼          ▼
┌─────────┐ ┌────────┐ ┌────────┐ ┌──────┐
│Customer │ │Product │ │ Order  │ │ Auth │
│   API   │ │  API   │ │  API   │ │ API  │
└────┬────┘ └───┬────┘ └───┬────┘ └──────┘
     │          │           │
     ▼          ▼           ▼
┌─────────┐ ┌────────┐ ┌──────────┐
│MS SQL   │ │ MySQL  │ │ MongoDB  │
└─────────┘ └────────┘ └──────────┘
                │
                ▼
         ┌──────────────┐
         │  RabbitMQ    │ (Event Bus)
         └──────────────┘
```

### Services

**Customer API** - Customer management (MS SQL Server, Port 1434)  
**Product API** - Product catalog (MySQL, Port 3307)  
**Order API** - Order processing (MongoDB, Port 27018)  
**Auth API** - JWT authentication  
**API Gateway** - Ocelot routing (Port 8085)  
**RabbitMQ** - Message broker (Port 5672, UI: 15672)

## 🔄 Event Flow

1. Client creates order → Order API
2. Order API saves to MongoDB
3. Order API publishes `order.created` event to RabbitMQ
4. Customer API consumes event and processes notification

## 🛠️ Tech Stack

- **.NET Core** - Web APIs
- **Ocelot** - API Gateway
- **MS SQL Server** - Customer data
- **MySQL** - Product data
- **MongoDB** - Order data
- **RabbitMQ** - Message broker
- **Docker** - Containerization
- **JWT** - Authentication

## 📁 Project Structure

```
MicroCart/
├── ApiGateWay/          # Ocelot gateway + ocelot.json
├── AuthApi/             # JWT authentication
├── CustomerWebApi/      # Customer service (SQL Server)
├── OrderApi/            # Order service (MongoDB)
├── ProductApi/          # Product service (MySQL)
├── JwtAuthenticationManager/  # Shared auth library
├── Messaging/           # RabbitMQ integration
├── Contracts/           # Shared DTOs
└── docker-compose.yml
```

## 🔧 Useful Commands

```bash
# Stop services
docker-compose down

# View logs
docker-compose logs -f

# Rebuild after changes
docker-compose up -d --build

# Remove all data
docker-compose down -v
```

## 🐛 Troubleshooting

**Services not starting?**
```bash
docker-compose logs [service-name]
```

**Port already in use?**
Change port mappings in `docker-compose.yml`

**Database connection issues?**
Wait 30-60 seconds for databases to fully initialize

## 🎯 API Routes (via Gateway)

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/account/login` | POST | Get JWT token |
| `/api/customer` | GET, POST | Customer list/create |
| `/api/customer/{id}` | GET, PUT, DELETE | Customer operations |
| `/api/products` | GET, POST | Product list/create |
| `/api/products/{id}` | GET, PUT, DELETE | Product operations |
| `/api/order` | GET, POST | Order list/create |
| `/api/order/{id}` | GET, PUT, DELETE | Order operations |

## 🔐 Security Note

⚠️ **Default passwords are for development only!**
- SQL Server: `yourStrong(!)Password`
- MySQL: `yourStrong(!)Password`
- RabbitMQ: `guest/guest`

**Change these before production deployment!**

## 📈 Future Enhancements

- API versioning
- Rate limiting
- Circuit breaker pattern (Polly)
- Redis caching
- Distributed tracing
- Kubernetes deployment

## 📄 License

MIT License

## 🤝 Contributing

Contributions welcome! Please open an issue or submit a pull request.

---

**⭐ Star this repo if you find it helpful!**
