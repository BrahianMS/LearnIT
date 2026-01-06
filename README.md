# LearnIT Platform

Bienvenido al repositorio de la plataforma de educación **LearnIT**.

Este proyecto consta de:
- **Backend**: API REST en .NET 8 con PostgreSQL.
- **Frontend**: Aplicación SPA en React + TypeScript + Vite.
- **Infraestructura**: Docker Compose para orquestar todo el entorno.

---

## Inicio Rápido con Docker (Recomendado)

La forma más sencilla de ejecutar el proyecto es utilizando **Docker Compose**. Esto levantará la base de datos, el backend y el frontend automáticamente.

### 1. Requisitos
- Docker y Docker Compose instalados.

### 2. Ejecutar
En la raíz del proyecto, ejecuta:

```bash
docker-compose up -d --build
```

Esto iniciará:
- **Frontend**: [http://localhost](http://localhost) (Puerto 80)
- **Backend API**: [http://localhost:5110](http://localhost:5110) (Puerto 5110 para acceso directo, interna en 8080)
- **Base de Datos**: Puerto **5455** del host (internamente 5432).

> **Nota:** La base de datos se inicializa automáticamente con las migraciones y datos semilla (usuario admin) al arrancar el backend.

### 3. Detener
```bash
docker-compose down
```

---

## Ejecución Manual (Desarrollo)

Si prefieres ejecutar los servicios individualmente sin Docker.

### 1. Configuración de Base de Datos
Necesitas una instancia de PostgreSQL corriendo.

1.  Asegúrate de tener PostgreSQL instalado y corriendo.
2.  Configura la cadena de conexión en `backend/src/LearnIT.API/appsettings.json` o usa variables de entorno en el `docker-compose.yml`.

### 2. Ejecutar Migraciones (Backend)
Desde la carpeta raíz del proyecto:

```bash
# Restaurar dependencias
dotnet restore backend/src/LearnIT.API/LearnIT.API.csproj

# Ejecutar Migraciones (Crear BD y Tablas)
dotnet run --project backend/src/LearnIT.API/LearnIT.API.csproj
```
*El proyecto está configurado para ejecutar migraciones automáticamente al iniciar (`Program.cs`), por lo que basta con correr la aplicación.*

Si deseas ejecutar migraciones manualmente con Entity Framework Tooling:
```bash
dotnet ef database update --project backend/src/LearnIT.Infrastructure --startup-project backend/src/LearnIT.API
```

### 3. Levantar API y Frontend

**Backend (.NET 8):**
```bash
dotnet run --project backend/src/LearnIT.API/LearnIT.API.csproj
```
Disponible en `http://localhost:5110`.

**Frontend (React/Vite):**
```bash
cd frontend
npm install
npm run dev
```
Disponible en `http://localhost:5173`.

---

## 🔑 Credenciales y Accesos

### Usuario Administrador (Seed Data)
El sistema crea automáticamente un usuario administrador al iniciar la base de datos por primera vez.

- **Email**: `admin@learnit.com`
- **Contraseña**: `Password123!`

### Documentación API (Swagger)
Si ejecutas en entorno de desarrollo (o configures Docker para ello):
- [http://localhost:5110/swagger](http://localhost:5110/swagger)

---

## 📦 Estructura del Proyecto

```
/
├── backend/            # Solución .NET 8 (Clean Architecture)
│   ├── src/LearnIT.API
│   ├── src/LearnIT.Application
│   ├── src/LearnIT.Domain
│   └── src/LearnIT.Infrastructure
├── frontend/           # Proyecto React + Vite
├── docker-compose.yml  # Orquestación de contenedores
└── README.md           # Estás aquí
```
