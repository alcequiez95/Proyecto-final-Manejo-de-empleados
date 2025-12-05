REQUERIMIENTOS DEL PROYECTO - SISTEMA DE MANEJO DE EMPLEADOS
============================================================

## Descripción General
Aplicación Web desarrollada en ASP.NET MVC que permite administrar empleados, departamentos y cargos 
de una empresa, con cálculos automáticos de deducciones (AFP, ARS, ISR) y exportación de datos en CSV.

## Funcionalidades Implementadas

### 1. Gestión de Empleados
- ✓ Crear empleados con validación de ID único
- ✓ Editar información de empleados
- ✓ Eliminar empleados con confirmación
- ✓ Mostrar lista de empleados con filtros
- ✓ Cálculo automático de:
  - Tiempo en la empresa (años y meses)
  - AFP (Aporte para Pensiones - 3%)
  - ARS (Aporte para Salud - 3.04%)
  - ISR (Impuesto sobre la Renta)
  - Salario Neto
- ✓ Validaciones:
  - Fecha de inicio no puede ser en el futuro
  - Salario debe ser mayor a 0
  - ID único no duplicado
  - Campos obligatorios requeridos

### 2. Gestión de Departamentos
- ✓ Crear departamentos
- ✓ Editar información de departamentos
- ✓ Eliminar departamentos (solo si no tienen empleados)
- ✓ Mostrar lista de departamentos
- ✓ Validaciones de integridad referencial

### 3. Gestión de Cargos
- ✓ Crear cargos con niveles de experiencia
- ✓ Editar información de cargos
- ✓ Eliminar cargos (solo si no tienen empleados)
- ✓ Mostrar lista de cargos
- ✓ Validaciones de integridad referencial

### 4. Exportación de Datos
- ✓ Exportar Empleados a CSV
- ✓ Exportar Departamentos a CSV
- ✓ Exportar Cargos a CSV
- ✓ Archivos generados con timestamp único

### 5. Manejo de Errores
- ✓ Try-catch en todos los controladores
- ✓ Validación de integridad de datos
- ✓ Mensajes de error claros al usuario
- ✓ Prevención de eliminación de registros relacionados
- ✓ Validación de fechas y valores numéricos

## Estructura del Proyecto

```
SistemaEmpleados/
├── Models/
│   ├── Empleado.cs
│   ├── Departamento.cs
│   └── Cargo.cs
├── Controllers/
│   ├── EmpleadoController.cs
│   ├── DepartamentoController.cs
│   ├── CargoController.cs
│   └── HomeController.cs
├── Views/
│   ├── Home/
│   │   └── Index.cshtml
│   ├── Empleado/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   ├── Departamento/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   ├── Cargo/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   └── Shared/
│       └── _Layout.cshtml
├── Data/
│   └── EmpleadoContext.cs
├── Utilities/
│   └── ExportadorCSV.cs
├── Global.asax
├── Global.asax.cs
├── RouteConfig.cs
├── Web.config
└── SistemaEmpleados.csproj
```

## Configuración de la Base de Datos

1. En Web.config, actualiza la cadena de conexión:
   ```
   <add name="SistemaEmpleadosConnection" connectionString="Data Source=DESKTOP-7VTD5RN;Initial Catalog=SistemaEmpleados;Integrated Security=true;" providerName="System.Data.SqlClient" />
   ```

2. Usa las siguientes credenciales (según lo indicado):
   - Server: DESKTOP-7VTD5RN
   - Usuario: mi_usuario_admin
   - Base de datos: SistemaEmpleados (crear nueva)

3. Entity Framework generará las tablas automáticamente en la primera ejecución

## Pasos para Ejecutar el Proyecto

1. Abre Visual Studio
2. Abre el proyecto SistemaEmpleados
3. Restaura los paquetes NuGet (EntityFramework, etc.)
4. Actualiza la conexión en Web.config con tus credenciales de SQL Server
5. En Package Manager Console, ejecuta:
   - Update-Database
6. Presiona F5 para ejecutar la aplicación
7. La aplicación abrirá en http://localhost/SistemaEmpleados

## Características de Seguridad

- ✓ Validación de CSRF con AntiForgeryToken
- ✓ Validación de entrada en todos los formularios
- ✓ Manejo seguro de excepciones sin exposición de información sensible
- ✓ Validación de integridad referencial antes de operaciones

## Notas Importantes

- Las fechas pasadas o futuras se validan automáticamente
- Los cálculos de descuentos se hacen en tiempo real
- No se pueden eliminar departamentos o cargos si tienen empleados asociados
- Todos los archivos CSV se generan con timestamp para evitar sobrescrituras
- La aplicación implementa patrones MVC estándar de ASP.NET

## Paquetes NuGet Requeridos

- EntityFramework (6.0.0 o superior)
- System.Data.SqlClient
- System.ComponentModel.DataAnnotations

## Autor
Proyecto Final - INF-4313
