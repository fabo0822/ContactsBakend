# 📞 Contacts API

Una API REST simple para gestionar contactos, desarrollada con .NET 9 y Entity Framework Core.

## 🎯 Características

- **4 endpoints esenciales** para operaciones CRUD básicas
- **Base de datos SQL Server** con Entity Framework Core
- **Arquitectura limpia** con separación de capas
- **Validaciones** de datos y email único
- **CORS configurado** para frontend
- **Datos de ejemplo** incluidos

## 🏗️ Arquitectura del Proyecto

```
ContactsApi/
├── Api/                    # Capa de presentación (Controllers)
├── BusinessLogic/          # Capa de lógica de negocio (Services)
├── DataAccess/            # Capa de acceso a datos (Repository, DbContext)
└── Domain/                # Capa de dominio (Entities)
```

### Capas explicadas:

- **Domain**: Contiene las entidades (Contact)
- **DataAccess**: Maneja la base de datos y repositorios
- **BusinessLogic**: Contiene la lógica de negocio y validaciones
- **Api**: Controladores REST y configuración

## 🚀 Endpoints Disponibles

### 1. **GET /api/contacts**
Obtiene todos los contactos.

**Respuesta:**
```json
[
  {
    "id": 1,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan.perez@example.com",
    "isFavorite": true,
    "imageUrl": "https://example.com/photo.jpg"
  }
]
```

### 2. **POST /api/contacts**
Crea un nuevo contacto.

**Request:**
```json
{
  "firstName": "María",
  "lastName": "García",
  "email": "maria.garcia@example.com",
  "isFavorite": false,
  "imageUrl": "https://example.com/photo.jpg"
}
```

**Respuesta:** El contacto creado con su ID asignado.

### 3. **PUT /api/contacts/{id}**
Actualiza un contacto existente (acepta cambios parciales).

**Request:**
```json
{
  "firstName": "María Elena",
  "isFavorite": true
}
```

**Respuesta:** El contacto actualizado.

### 4. **DELETE /api/contacts/{id}**
Elimina un contacto.

**Respuesta:** 204 No Content (éxito)

### 5. **POST /api/contacts/upload-image** 🆕
Sube una imagen desde el PC y retorna la URL pública.

**Request:** FormData con archivo de imagen
- **Content-Type:** `multipart/form-data`
- **Campo:** `file` (archivo de imagen)

**Validaciones:**
- ✅ Tipos permitidos: JPG, JPEG, PNG, GIF, BMP, WEBP
- ✅ Tamaño máximo: 5MB
- ✅ Nombres únicos generados automáticamente

**Respuesta exitosa:**
```json
{
  "message": "Imagen subida exitosamente.",
  "imageUrl": "/images/12345678-1234-1234-1234-123456789abc.jpg",
  "fileName": "12345678-1234-1234-1234-123456789abc.jpg",
  "fileSize": 245760
}
```

**Errores posibles:**
- `400 Bad Request`: Archivo no enviado, tipo no permitido, o tamaño excedido
- `500 Internal Server Error`: Error del servidor al guardar el archivo

## 🛠️ Tecnologías Utilizadas

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **Swagger/OpenAPI** (para documentación)

## 📋 Prerrequisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

## ⚙️ Configuración

### 1. Clonar el repositorio
```bash
git clone <tu-repositorio>
cd ContactsApi
```

### 2. Configurar la base de datos
Edita `Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ContactsDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Restaurar dependencias
```bash
dotnet restore
```

### 4. Ejecutar migraciones
```bash
cd DataAccess
dotnet ef database update
```

### 5. Ejecutar la aplicación
```bash
cd Api
dotnet run
```

La API estará disponible en:
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`
- **Swagger**: `https://localhost:7000/swagger`

## 🗄️ Estructura de la Base de Datos

### Tabla: `Contacts`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Id` | int | Clave primaria (auto-incremento) |
| `FirstName` | nvarchar(100) | Primer nombre (requerido) |
| `LastName` | nvarchar(100) | Apellido (requerido) |
| `Email` | nvarchar(255) | Email único (requerido) |
| `IsFavorite` | bit | Marca si es favorito (default: false) |
| `ImageUrl` | nvarchar(2048) | URL de la imagen (opcional) |

### Índices:
- **PK_Contacts**: Clave primaria en `Id`
- **IX_Contacts_Email**: Índice único en `Email`

## 🧪 Pruebas

### Usando el archivo de pruebas incluido:
1. Abre `Api/test-endpoints.http` en Visual Studio Code
2. Instala la extensión "REST Client"
3. Haz clic en "Send Request" en cada endpoint

### Usando Postman:
Importa la colección desde `Api/test-endpoints.http` o crea requests manuales.

### Usando curl:
```bash
# Obtener todos los contactos
curl -X GET "https://localhost:7000/api/contacts"

# Crear un contacto
curl -X POST "https://localhost:7000/api/contacts" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Test","lastName":"User","email":"test@example.com"}'
```

## 📊 Datos de Ejemplo

La aplicación incluye 30 contactos de ejemplo que se crean automáticamente la primera vez que se ejecuta.

## 🔧 Comandos Útiles

### Entity Framework:
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project DataAccess --startup-project Api

# Actualizar base de datos
dotnet ef database update --project DataAccess --startup-project Api

# Eliminar última migración
dotnet ef migrations remove --project DataAccess --startup-project Api
```

### Desarrollo:
```bash
# Limpiar y reconstruir
dotnet clean
dotnet build

# Ejecutar con hot reload
dotnet watch run --project Api
```

## 🐛 Solución de Problemas

### Error de conexión a la base de datos:
1. Verifica que SQL Server esté ejecutándose
2. Revisa la cadena de conexión en `appsettings.json`
3. Asegúrate de que la base de datos existe

### Error de migraciones:
```bash
# Eliminar base de datos y recrear
dotnet ef database drop --project DataAccess --startup-project Api
dotnet ef database update --project DataAccess --startup-project Api
```

## 📚 Conceptos Aprendidos

Este proyecto demuestra:
- **Arquitectura en capas** (Domain, DataAccess, BusinessLogic, Api)
- **Entity Framework Core** para ORM
- **Inyección de dependencias**
- **RESTful API** design
- **Validaciones** de datos
- **Migraciones** de base de datos
- **CORS** para frontend
- **Swagger** para documentación

## 🤝 Contribuciones

Este es un proyecto de aprendizaje. Si encuentras errores o tienes sugerencias:
1. Crea un issue
2. Fork el proyecto
3. Crea una rama para tu feature
4. Envía un pull request

## 📄 Licencia

Este proyecto es para fines educativos.

---

**Desarrollado con ❤️ para aprender .NET y ASP.NET Core**
