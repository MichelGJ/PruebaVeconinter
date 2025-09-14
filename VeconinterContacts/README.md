
# **Veconinter Contacts**

Este proyecto es una aplicación **ASP.NET Core MVC** para gestionar **Clientes** y **SubClientes**, aplicando principios de arquitectura limpia y patrones como **Repository** y **Dependency Injection**.

---

## **Requisitos previos**
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express / Developer Edition](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)

---

## **Configuración de la base de datos (con autenticación SQL Login)**

> Recomendado para entornos compartidos/servidor. Si sigues usando **LocalDB**, mantén Windows Auth y omite esta sección.

### 1) Preparar SQL Server
1. Asegúrate de usar una **instancia real** (p. ej. `SQLEXPRESS`), no LocalDB.  
2. Habilita **modo mixto** (Windows + SQL) durante la instalación o desde las propiedades del servidor, y **reinicia** el servicio.  
3. Habilita **TCP/IP** y fija **puerto 1433** para poder usar `Server=localhost,1433`.

---

### 2) Crear login y usuario de base de datos
Conéctate como administrador (Windows Auth o `sa`) y ejecuta:

```sql
-- Crear base si no existe
IF DB_ID('VeconinterContacts') IS NULL
BEGIN
    CREATE DATABASE VeconinterContacts;
END
GO

-- Crear login a nivel servidor
IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name='vecon_user')
BEGIN
    CREATE LOGIN vecon_user WITH PASSWORD = '[TUCONTRASEÑA]',
        CHECK_POLICY = ON, CHECK_EXPIRATION = ON;
END
GO

USE VeconinterContacts;
GO

-- Mapear login a usuario en la DB y otorgar permisos mínimos de CRUD
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='vecon_user')
BEGIN
    CREATE USER vecon_user FOR LOGIN vecon_user;
END
EXEC sp_addrolemember N'db_datareader', N'vecon_user';
EXEC sp_addrolemember N'db_datawriter', N'vecon_user';

-- (Temporal) Solo para correr migraciones desde la app:
EXEC sp_addrolemember N'db_ddladmin', N'vecon_user';
ALTER USER vecon_user WITH DEFAULT_SCHEMA = dbo;
```

> Tras crear el esquema (paso 4), retira `db_ddladmin` para endurecer permisos.

---

### 3) Configurar la cadena de conexión (sin exponer secretos)
Usa **User Secrets** en desarrollo para no subir la contraseña al repositorio:

```bash
cd <carpeta-del-proyecto-con-el-.csproj>
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=VeconinterContacts;User ID=vecon_user;Password=[TUCONTRASEÑA];MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
```

Deja un **placeholder** en `appsettings.json` (opcional):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=VeconinterContacts;User ID=vecon_user;Password=<SECRET>;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
}
```

> **Importante:** Los **User Secrets** **sobrescriben** `appsettings.json`.  
> Si alguna vez cambias el string en el archivo y “no surte efecto”, revisa con:
> ```bash
> dotnet user-secrets list
> ```

---

### 4) Crear/actualizar el esquema con EF Core
Desde la carpeta del proyecto:

```bash
# Si no tienes migraciones aún:
# dotnet ef migrations add InitialCreate

dotnet ef database update
```

> Si obtienes “permission denied” al crear tablas, confirma que **temporalmente** diste `db_ddladmin` al usuario `vecon_user` (ver paso 2).

---

### 5) Endurecer permisos (tras migraciones)
Remueve el rol de DDL y deja solo CRUD:

```sql
USE VeconinterContacts;
EXEC sp_droprolemember N'db_ddladmin', N'vecon_user';
EXEC sp_addrolemember N'db_datareader', N'vecon_user';
EXEC sp_addrolemember N'db_datawriter', N'vecon_user';
```

---

### (Alternativa) Seguir con LocalDB y Windows Auth (solo desarrollo)
Si prefieres **LocalDB** en tu máquina local, usa:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=VeconinterContacts;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

> LocalDB **no admite SQL Login** (usuario/contraseña).

---

## **Ejecución de la aplicación**
1. Restaurar paquetes:
   ```bash
   dotnet restore
   ```
2. Compilar:
   ```bash
   dotnet build
   ```
3. Ejecutar:
   ```bash
   dotnet run
   ```
4. Abrir en el navegador: [http://localhost:5000](http://localhost:5000) o el puerto indicado en consola.

---

## **Gestión de Clientes y Subclientes**

### **1. Agregar Cliente**
1. En el menú principal, selecciona la opción **Clientes**.
2. Haz clic en **Nuevo**.
3. Completa los campos obligatorios:
   - **Nombre**
   - **Email**
   - **Teléfono**
4. Presiona **Guardar**.  
   > Si la validación falla, los campos mostrarán mensajes en tiempo real.

---

### **2. Editar Cliente**
1. En la lista de clientes, selecciona el cliente que deseas modificar.
2. Haz clic en el botón **Editar**.
3. Actualiza la información necesaria.
4. Presiona **Guardar cambios**.  
   > El sistema actualizará los datos sin crear un nuevo registro.

---

### **3. Eliminar Cliente**
1. En la lista de clientes, selecciona el cliente a eliminar.
2. Haz clic en **Eliminar**.
3. Aparecerá una ventana de confirmación mostrando:
   - Datos básicos del cliente.
   - Lista de subclientes relacionados.
4. Confirma la eliminación.  
   > Esta acción eliminará también a todos los subclientes vinculados.

---

### **4. Agregar Subcliente**
1. En la vista **Detalles** de un cliente, dirígete a la sección **Subclientes**.
2. Usa el formulario al pie de la lista para agregar un subcliente.
3. Completa los campos:
   - **Nombre**
   - **Email**
   - **Teléfono**
4. Presiona **+** para agregar.  
   > La validación en tiempo real asegurará que los datos sean correctos antes de enviarse.

---

### **5. Editar Subcliente**
1. En la lista de subclientes, haz clic en el botón **Editar** junto al registro deseado.
2. Actualiza los campos requeridos.
3. Presiona **Guardar cambios**.
4. Puedes cancelar y volver a la vista del cliente usando el botón **Cancelar**.

---

### **6. Eliminar Subcliente**
1. En la lista de subclientes, haz clic en **Eliminar**.
2. Confirma la acción en el cuadro emergente.
3. El subcliente será eliminado sin afectar a los demás.

---

## **Patrones de diseño utilizados**

### 1) **Repository Pattern**
- Desacopla la lógica de negocio del acceso a datos, proporcionando una capa intermedia entre la base de datos y los controladores.
- Interfaces y clases en la carpeta `Repositories`.
- Ejemplo:  
  - `IClienteRepository` y `ClienteRepository` encapsulan todas las operaciones sobre la entidad **Cliente**, incluyendo la carga de subclientes.

**Beneficios:**
- Código más limpio y mantenible al centralizar la lógica de acceso a datos.
- Permite **cambiar la tecnología de persistencia** (por ejemplo, cambiar EF Core por otro ORM) sin modificar los controladores.
- Facilita la **prueba unitaria** al permitir usar repositorios falsos (mocks o stubs).
- Aplica el principio **Single Responsibility**, separando responsabilidades claras.

---

### 2) **Dependency Injection (DI)**
- Registra y gestiona la creación de repositorios en `Program.cs`.
- Inyección en controladores a través de sus constructores.  
  Ejemplo:
  ```csharp
  public ClientesController(IClienteRepository repo, ISubClienteRepository subRepo) 
  { 
      _repo = repo; 
      _subRepo = subRepo; 
  }
  ```

**Beneficios:**
- Promueve la **inversión de control (IoC)**, reduciendo el acoplamiento.
- Facilita la **sustitución de implementaciones**, por ejemplo, usar mocks en pruebas unitarias.
- Manejo automático del **ciclo de vida de las dependencias**, asegurando que cada request tenga sus propios objetos (`Scoped`).
- Código más **organizado, modular y mantenible**.

---

### 3) **MVC (Model-View-Controller)**
- **Models:** Entidades `Cliente` y `SubCliente` en la carpeta `Models`.
- **Views:** Razor Views en la carpeta `Views`.
- **Controllers:** `ClientesController` y `SubClientesController` manejan la lógica de negocio y flujo de datos.

---

## **Estructura de carpetas**

```
/Controllers
  ClientesController.cs
  SubClientesController.cs

/Data
  AppDbContext.cs

/Models
  Cliente.cs
  SubCliente.cs

/Repositories
  Interfaces/
  Implementations/

/Views
  /Clientes
  /SubClientes
  /Shared

wwwroot/
```

---


## **Resumen**
Este proyecto cumple con los requerimientos de:
- CRUD completo para Clientes y Subclientes.
- Autenticación básica con cookies y usuario fijo.
- Arquitectura limpia utilizando patrones de diseño estándar para aplicaciones empresariales.
- Base de datos SQL Server con migraciones y configuración sencilla.
- Se utilizo **User Secrets** en desarrollo para proteger credenciales.  
- En producción, se puede evaluar utilizar **variables de entorno** o un servicio seguro como **Azure Key Vault** para mayor proteccion de credenciales.
