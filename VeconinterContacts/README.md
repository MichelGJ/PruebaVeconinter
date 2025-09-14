# Veconinter — Gestión de Clientes y Subclientes

Aplicación web en **ASP.NET Core 8 MVC** para la gestión diaria de datos personales de **clientes y subclientes**, diseñada para centralizar, organizar y proteger la información.

---

## **Requisitos previos**
- **.NET 8 SDK**
- **SQL Server LocalDB** (o SQL Server estándar)
- Visual Studio 2022+ (recomendado)

---

## **Configuración de la base de datos**
1. Abre el archivo `appsettings.json` y configura la cadena de conexión según tu entorno.  
   Ejemplo usando LocalDB:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=VeconinterContacts;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```
2. Ejecuta migraciones para crear la base de datos:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## **Ejecución del proyecto**
En la carpeta raíz del proyecto, ejecuta:
```bash
dotnet run
```
Luego abre en el navegador:  
[https://localhost:5001] o [http://localhost:5000]

**Credenciales de prueba:**
- Email: `admin@veconinter.com`
- Password: `Admin!123`

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

## **Patrones de software utilizados**

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

## **Resumen**
Este proyecto cumple con los requerimientos de:
- CRUD completo para Clientes y Subclientes.
- Autenticación básica con cookies y usuario fijo.
- Arquitectura limpia utilizando patrones de diseño estándar para aplicaciones empresariales.
- Base de datos SQL Server con migraciones y configuración sencilla.
