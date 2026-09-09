# Vault Electrónico de Contactos

Aplicación web para administrar una agenda personal mediante una interfaz inspirada en un vault digital. El proyecto combina páginas MVC para usuarios y una API REST para integraciones, con almacenamiento persistente en SQLite.

## Funcionalidades

- Registrar, listar, consultar, editar y eliminar contactos.
- Buscar parcialmente por nombre o apellido.
- Validar en español los campos obligatorios y el formato del correo.
- Mostrar iniciales, empresa, teléfono y correo en una tabla responsive.
- Confirmar explícitamente antes de eliminar un registro.
- Exponer un API REST con respuestas HTTP apropiadas.
- Explorar y probar los endpoints desde Swagger UI.
- Crear la base, aplicar migraciones y cargar tres contactos de demostración al primer inicio.
- Persistir SQLite en un volumen cuando se ejecuta con Docker Compose.

## Tecnologías

- ASP.NET Core MVC y ASP.NET Core Web API
- C# y .NET 10
- Entity Framework Core
- SQLite
- Bootstrap 5 y CSS personalizado
- Docker y Docker Compose
- Swagger / OpenAPI
- Git y GitHub

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) para ejecutar contenedores
- [Git](https://git-scm.com/)

## Ejecución local

Desde la raíz del proyecto:

```bash
dotnet tool restore
dotnet restore
dotnet ef database update
dotnet run
```

La aplicación también ejecuta `Database.Migrate()` al arrancar, por lo que actualizar manualmente la base es útil para verificar la migración, pero no es indispensable. Abre la URL HTTP que muestra la terminal; con el perfil incluido es:

```text
http://localhost:5202
```

La base local se crea como `contactos.db` en la raíz y se ignora en Git.

## Ejecución con Docker

```bash
docker compose up --build
```

Abre:

```text
http://localhost:8080
```

La cadena de conexión del contenedor apunta a `/app/data/contactos.db`. El volumen nombrado `vaultcontactos_data` conserva la información aunque se vuelva a crear el contenedor.

### Detener contenedores

```bash
docker compose down
```

El comando anterior conserva el volumen. Para borrarlo deliberadamente junto con sus datos se puede usar `docker compose down -v`.

## API REST

Ruta base: `/api/contactos`

La documentación interactiva está disponible en:

```text
http://localhost:5202/swagger
```

Con Docker se encuentra en `http://localhost:8080/swagger`.

| Método | Endpoint | Descripción | Respuesta esperada |
| --- | --- | --- | --- |
| GET | `/api/contactos` | Lista todos los contactos | `200 OK` |
| GET | `/api/contactos/{id}` | Obtiene un contacto | `200 OK` o `404 Not Found` |
| POST | `/api/contactos` | Crea un contacto | `201 Created` o `400 Bad Request` |
| PUT | `/api/contactos/{id}` | Actualiza un contacto | `204 No Content`, `400` o `404` |
| DELETE | `/api/contactos/{id}` | Elimina un contacto | `204 No Content` o `404` |
| GET | `/api/contactos/buscar?texto=ben` | Busca por nombre o apellido | `200 OK` o `400 Bad Request` |

Ejemplo para crear un contacto:

```bash
curl -X POST http://localhost:5202/api/contactos \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Ana","apellido":"Rojas","telefono":"+591 70000000","correo":"ana@ejemplo.com","empresa":"Ejemplo SRL"}'
```

El campo `fechaCreacion` se establece en el servidor.

## Estructura del proyecto

```text
VaultContactos/
├── Controllers/
│   ├── ContactosController.cs       # Flujo MVC
│   └── Api/ContactosApiController.cs # Endpoints REST
├── Data/
│   ├── AppDbContext.cs              # Contexto de EF Core
│   └── DbSeeder.cs                  # Datos de demostración
├── Migrations/                      # Historial de la base
├── Models/Contacto.cs               # Entidad y validaciones
├── Views/Contactos/                 # Vistas del CRUD
├── wwwroot/                         # Bootstrap, CSS y recursos web
├── Program.cs                       # Servicios, rutas y migración automática
├── Dockerfile                       # Imagen multi-stage
└── compose.yaml                     # Aplicación y volumen persistente
```

## Capturas

Guarda aquí las capturas para la entrega, por ejemplo en `docs/capturas/`:

- Pantalla principal con la tabla y los tres contactos de demostración.
- Resultado de una búsqueda parcial.
- Formulario de creación mostrando validaciones.
- Detalle de un contacto.
- Confirmación de eliminación.
- Aplicación ejecutándose en Docker y respuesta JSON del API.

## Git y GitHub

Después de crear un repositorio vacío en GitHub, ejecuta:

```bash
git init
git add .
git commit -m "feat: crear Vault Electrónico de Contactos"
git branch -M main
git remote add origin https://github.com/TU-USUARIO/VaultContactos.git
git push -u origin main
```

Reemplaza `TU-USUARIO` por tu nombre de usuario. Si `origin` ya existe, usa `git remote set-url origin URL` en vez de `git remote add origin URL`.

## Autor

**Estudiante:** _Escribe aquí tu nombre completo_
