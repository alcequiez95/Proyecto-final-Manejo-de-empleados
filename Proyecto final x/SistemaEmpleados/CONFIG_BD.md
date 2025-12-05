GUÍA DE CONFIGURACIÓN DE BASE DE DATOS
======================================

## Paso 1: Preparación en Visual Studio

1. Abre Visual Studio como Administrador
2. Ve a Tools > NuGet Package Manager > Package Manager Console
3. Asegúrate de que el Default Project sea "SistemaEmpleados"

## Paso 2: Instalar Entity Framework

En Package Manager Console, ejecuta:

```
Install-Package EntityFramework -Version 6.4.4
```

Espera a que complete la instalación.

## Paso 3: Habilitar Migraciones

```
Enable-Migrations
```

Esto creará una carpeta "Migrations" en tu proyecto.

## Paso 4: Crear Migración Inicial

```
Add-Migration InitialCreate
```

Esto genera un archivo con las instrucciones para crear las tablas.

## Paso 5: Actualizar Base de Datos

```
Update-Database
```

Esto ejecuta la migración y crea:
- Base de datos: SistemaEmpleados
- Tablas: Departamentos, Cargos, Empleados
- Relaciones y índices

## Paso 6: Verificar la Creación

Abre SQL Server Management Studio:
1. Conéctate a DESKTOP-7VTD5RN
2. Ve a Databases
3. Deberías ver "SistemaEmpleados"
4. Expande para ver las tablas

## Si hay problemas...

### Problema: Base de datos ya existe

```
Drop-Database -Force
Enable-Migrations -Force
Add-Migration InitialCreate
Update-Database
```

### Problema: Migraciones conflictivas

```
Remove-Migration
Update-Database
Add-Migration InitialCreate
Update-Database
```

### Problema: EntityFramework no funciona

```
Update-Package EntityFramework -Reinstall
```

## Verificar la Base de Datos

Después de ejecutar Update-Database, verifica en SQL Server:

```sql
-- Ver tablas creadas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo'

-- Ver estructura de Empleados
EXEC sp_columns 'Empleados'

-- Ver relaciones
SELECT CONSTRAINT_NAME, TABLE_NAME 
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
```

## Datos de Conexión

Servidor: DESKTOP-7VTD5RN
Base de Datos: SistemaEmpleados
Usuario: (Integrated Security / tu usuario de Windows)
Contraseña: No requerida (si usas Integrated Security)

Cadena de conexión en Web.config:
```xml
<add name="SistemaEmpleadosConnection" 
     connectionString="Data Source=DESKTOP-7VTD5RN;Initial Catalog=SistemaEmpleados;Integrated Security=true;" 
     providerName="System.Data.SqlClient" />
```

## Próximos Pasos

1. Ejecuta la aplicación (F5)
2. Ve a la sección de Departamentos y crea algunos
3. Ve a la sección de Cargos y crea algunos
4. Ve a la sección de Empleados y crea algunos
5. Prueba la exportación a CSV

¡La base de datos está lista para usar!
