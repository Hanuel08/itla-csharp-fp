# Sistema de Gestión de Estudiantes

Proyecto final del curso **C# Básico** impartido en el **ITLA** (Instituto Tecnológico de Las Américas).

## Descripción

Aplicación de consola en C# que permite gestionar un registro de estudiantes. Los datos se persisten en un archivo JSON y la interfaz utiliza la librería **Spectre.Console** para una experiencia visual mejorada.

## Funcionalidades

- **Registrar estudiante** — ingreso de nombre, apellido, email, género, carrera, nota final y fecha de nacimiento con validaciones.
- **Mostrar estudiantes** — listado completo en formato tabular.
- **Buscar estudiante** — búsqueda por nombre, matrícula, edad, email o carrera.
- **Actualizar estudiante** — modificación de datos existentes (los campos vacíos mantienen su valor actual).
- **Eliminar estudiante** — borrado con confirmación previa.
- **Calcular promedio** — nota promedio de todos los estudiantes.
- **Mayor nota** — estudiante con la calificación más alta.
- **Estudiantes aprobados/reprobados** — filtrado por nota ≥ 70 (aprobado) o < 70 (reprobado).

## Requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) o superior.

## Cómo ejecutar

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run --project itla-csharp-fp
```

O desde la raíz de la solución:

```bash
dotnet run
```

## Estructura del proyecto

```
itla-csharp-fp/
├── Model/
│   └── Student.cs          # Entidad del estudiante
├── Service/
│   └── StudentService.cs   # Lógica de negocio y validaciones
├── Utils/
│   ├── JsonStorage.cs      # Persistencia en archivo JSON + logs
│   └── MyTable.cs          # Renderizador de tablas personalizado
├── Data/
│   ├── students.json       # Datos almacenados (se crea al registrar)
│   └── logs.txt            # Registro de operaciones (se crea al registrar)
└── Program.cs              # Punto de entrada con menú interactivo
```

## Tecnologías

- **.NET 10.0** — framework de ejecución.
- **Spectre.Console** — interfaz de consola con colores, menús y tablas.
- **System.Text.Json** — serialización/deserialización de datos.
