GUÍA DE INSTALACIÓN Y CONFIGURACIÓN
====================================

## Requisitos Previos

1. Visual Studio 2019 o superior (con soporte ASP.NET)
2. .NET Framework 4.7.2 o superior
3. SQL Server (DESKTOP-7VTD5RN con usuario mi_usuario_admin)
4. Paquetes NuGet: EntityFramework 6.0.0+

## Pasos de Instalación

### Paso 1: Preparación del Proyecto en Visual Studio

1. Abre Visual Studio como Administrador
2. Selecciona "Abrir un proyecto o una solución"
3. Navega a: c:\Users\Tre3m\OneDrive\Desktop\Prog III\Proyecto final x\SistemaEmpleados
4. Abre la carpeta del proyecto

### Paso 2: Crear Solución y Proyecto

1. Crea una nueva solución ASP.NET MVC
2. Copia los archivos generados a la solución
3. O abre directamente desde Visual Studio con los archivos .csproj

### Paso 3: Instalación de NuGet Packages

En Package Manager Console, ejecuta:

```
Install-Package EntityFramework
```

### Paso 4: Configuración de la Base de Datos

1. Abre Web.config
2. Verifica la cadena de conexión:
   ```xml
   <add name="SistemaEmpleadosConnection" 
        connectionString="Data Source=DESKTOP-7VTD5RN;Initial Catalog=SistemaEmpleados;Integrated Security=true;" 
        providerName="System.Data.SqlClient" />
   ```

3. Si necesitas usar autenticación de usuario:
   ```xml
   <add name="SistemaEmpleadosConnection" 
        connectionString="Data Source=DESKTOP-7VTD5RN;Initial Catalog=SistemaEmpleados;User Id=mi_usuario_admin;Password=tu_contraseña;" 
        providerName="System.Data.SqlClient" />
   ```

### Paso 5: Crear la Base de Datos

En Package Manager Console, ejecuta:

```
Enable-Migrations
Add-Migration InitialCreate
Update-Database
```

Esto creará automáticamente:
- Base de datos: SistemaEmpleados
- Tablas: Departamentos, Cargos, Empleados
- Índices y relaciones

### Paso 6: Ejecutar la Aplicación

1. Presiona F5 o selecciona Debug > Iniciar depuración
2. Visual Studio abrirá la aplicación en el navegador predeterminado
3. La URL será: http://localhost/SistemaEmpleados (o similar)

## Solución de Problemas Comunes

### Error: "Could not connect to SQL Server"
- Verifica que SQL Server esté ejecutándose
- Comprueba el nombre del servidor (DESKTOP-7VTD5RN)
- Intenta con SQL Server Management Studio para probar la conexión

### Error: "No se encontró EntityFramework"
```
Update-Package EntityFramework -Reinstall
```

### Error: "File not found on localhost"
- Asegúrate de que IIS Express está ejecutándose
- Reinicia Visual Studio
- Limpia la solución: Build > Clean Solution

### Error de Migraciones
```
Drop-Database
Enable-Migrations -Force
Add-Migration InitialCreate
Update-Database
```

## Primeros Pasos en la Aplicación

1. Accede a la página de Inicio desde http://localhost/SistemaEmpleados
2. Crea 2-3 Departamentos (p.ej.: IT, Recursos Humanos, Ventas)
3. Crea 2-3 Cargos (p.ej.: Desarrollador, Gerente, Analista)
4. Crea algunos Empleados (asigna los departamentos y cargos)
5. Prueba la exportación a CSV desde el botón "Exportar a CSV"

## Validaciones Implementadas

### Al crear/editar Empleados:
- ID único (no duplicados)
- Nombre obligatorio
- Departamento y Cargo obligatorios
- Fecha de inicio no puede ser en el futuro
- Salario debe ser mayor a 0
- La fecha no puede ser inexistente (31 de febrero, etc.)

### Al eliminar:
- No se pueden eliminar departamentos con empleados
- No se pueden eliminar cargos con empleados

### Cálculos Automáticos:
- Tiempo en empresa: Se calcula en años y meses desde la fecha de inicio
- AFP: 3% del salario
- ARS: 3.04% del salario
- ISR: Según escala de impuestos dominicana
- Salario Neto: Salario - AFP - ARS - ISR

## Funcionalidades de Exportación

Los archivos CSV se generan en: Proyecto\Exports\

Nombre de archivo: [Tabla]_YYYYMMDD_HHMMSS.csv

Ejemplo: Empleados_20241112_143022.csv

Los archivos contienen:
- Encabezados con nombres descriptivos
- Todos los campos de la tabla
- Formato UTF-8
- Separador: coma

## Estructura Base de Datos

### Tabla: Empleados
- EmpleadoID (PK, Int, Único)
- Nombre (Varchar 100)
- DepartamentoID (FK)
- CargoID (FK)
- FechaInicio (DateTime)
- Salario (Decimal 10,2)
- Estado (Varchar)
- AFP (Decimal 10,2, Calculado)
- ARS (Decimal 10,2, Calculado)
- ISR (Decimal 10,2, Calculado)
- SalarioNeto (Decimal 10,2, Calculado)
- TiempoEmpresa (Varchar, Calculado)
- FechaCreacion (DateTime)

### Tabla: Departamentos
- DepartamentoID (PK, Int, Único)
- Nombre (Varchar 100)
- Descripcion (Varchar 500)
- FechaCreacion (DateTime)

### Tabla: Cargos
- CargoID (PK, Int, Único)
- Nombre (Varchar 100)
- Descripcion (Varchar 500)
- NivelExperiencia (Varchar)
- FechaCreacion (DateTime)

## Soporte

Para problemas o preguntas:
1. Revisa los mensajes de error en la aplicación
2. Consulta el README.md
3. Verifica que la conexión a SQL Server sea correcta
4. Comprueba que los paquetes NuGet estén instalados
