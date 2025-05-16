# 🏡 SmartHome

**SmartHome** es una solución completa para la gestión de hogares inteligentes y dispositivos IoT, desarrollada como parte del curso *Diseño de Aplicaciones 2* (Universidad ORT Uruguay). El proyecto se enfoca en crear una arquitectura robusta, extensible y bien testeada, aplicando principios de diseño modernos, desarrollo orientado a pruebas (TDD), y tecnologías actuales tanto en el frontend como en el backend.

## 📌 Tabla de contenidos

- [Funcionalidades](#funcionalidades)
- [Tecnologías](#tecnologías)
- [Arquitectura y diseño](#arquitectura-y-diseño)
- [API REST](#api-rest)
- [Testing](#testing)
- [Instalación y ejecución](#instalación-y-ejecución)
- [Autores](#autores)

---

## ✅ Funcionalidades

- Gestión de hogares, usuarios, habitaciones y dispositivos inteligentes.
- Soporte para múltiples roles (Administrador, Dueño de Empresa, Dueño de Hogar).
- Importación extensible de dispositivos desde archivos (por ejemplo JSON).
- Validaciones dinámicas de dispositivos mediante plugins (.dll externos).
- Agrupación de dispositivos por habitación.
- Asignación de roles dinámicos a usuarios.
- Frontend web para usuarios finales.
- Comunicación cliente-servidor vía servicios HTTP.

---

## 🧰 Tecnologías

### Backend:
- **.NET 6** con **ASP.NET Core**
- **Entity Framework Core** para acceso a base de datos relacional
- **Swagger / OpenAPI** para documentación y pruebas de endpoints
- **xUnit** para testing
- **C#** como lenguaje principal

### Frontend:
- **Angular** (framework SPA moderno)
- **TypeScript** para componentes, servicios y vistas
- Arquitectura basada en componentes y servicios
- Consumo de API RESTful vía `HttpClient`

### Herramientas:
- **Docker** (usado en el desarrollo)
- **Postman** (para pruebas de endpoints)
- **Git + GitHub** para control de versiones
- **NDepend** (para métricas de diseño)
- **PlantUML / diagrams.net** (para documentación de arquitectura)

---

## 🧠 Arquitectura y diseño

La aplicación está organizada en **módulos independientes y altamente cohesivos**, siguiendo una arquitectura en capas:

```
WebApi
│
├── Controllers (REST endpoints)
├── BusinessLogic / IBusinessLogic
├── DataAccess / IDataAccess
├── Domain (entidades de negocio)
├── Model (DTOs de entrada/salida)
├── ImportersLogic (estrategias de importación)
└── Validators (validación desacoplada)
```

### 🧱 Principios y buenas prácticas aplicadas:

- **TDD (Test Driven Development):** Todo el backend fue desarrollado escribiendo primero las pruebas unitarias.
- **RESTful Design:** Endpoints bien definidos por recursos y operaciones HTTP (`GET`, `POST`, `PUT`, `PATCH`, `DELETE`).
- **Clean Architecture:** Separación clara entre capas (presentación, lógica, datos).
- **SOLID Principles:**
    - **S**: Single Responsibility (cada clase tiene un rol claro)
    - **O**: Open/Closed (la app se extiende sin modificar código existente)
    - **L**: Liskov Substitution (jerarquías de roles y dispositivos)
    - **I**: Interface Segregation (interfaces específicas como `IUserValidator`, `IDeviceImport`)
    - **D**: Dependency Inversion (todas las dependencias son inyectadas)
- **GRASP** y otros patrones de diseño:
    - `Factory`: para creación dinámica de roles (`RoleFactory`)
    - `Strategy`: validadores (`IModelValidator`, `IUserValidator`) e importadores (`IDeviceImport`)
    - `Dependency Injection`: centralizado en `ServiceFactory`

---

## 🌐 API REST

La API está completamente documentada y expone múltiples endpoints organizados por recurso:

### Ejemplos de endpoints:
- `POST /api/v1/users`
- `GET /api/v1/homes/{id}/devices`
- `POST /api/v1/imports`
- `PATCH /api/v1/devices/{id}`
- `POST /api/v1/users/{id}/roles`

---

## 🧪 Testing

- Uso extensivo de **xUnit** para pruebas unitarias de todas las capas.
- Separación de proyectos de test (`TestDomain`, `TestWebApi`, etc.).
- Cobertura reportada por capa, excluyendo código no testeable.
- Validación de métricas de diseño con **NDepend**.
- Desarrollo siguiendo TDD, con pruebas escritas antes de la implementación.

---

## 🛠️ Instalación y ejecución (backend)

1. Clonar el repositorio:

```bash
git clone https://github.com/IngSoft-DA2/301178_231810_280070.git
cd SmartHome
```

2. Configurar base de datos en `appsettings.json` (por defecto usa SQL Server).

3. Restaurar dependencias y ejecutar:

```bash
dotnet restore
dotnet ef database update
dotnet run --project WebApi
```

---

## 👨‍💻 Autores

Proyecto desarrollado por:

- Angelina Maverino
- Yliana Otero
- María Belén Rywaczuk