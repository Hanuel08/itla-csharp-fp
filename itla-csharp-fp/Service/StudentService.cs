using itla_csharp_fp.Utils;
using itla_csharp_fp.Model;

using Spectre.Console;


namespace itla_csharp_fp.Service;

using System.Collections.Generic;
using System.Linq;


public class StudentService
{
    public static void Register()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold yellow]Registrar estudiante[/]:\n");
            
            Console.Write("Nombre: ");
            string name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name))
                throw new FormatException("Por favor ingrese un nombre válido");
            
            if (name.Any(char.IsDigit))
                throw new FormatException("El nombre no debe contener números");

            name = name.ToLower();

            Console.Write("Apellido: ");
            string lastname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lastname))
                throw new FormatException("Por favor ingrese un apellido válido");
            
            if (lastname.Any(char.IsDigit))
                throw new FormatException("El apellido no debe contener números");

            lastname = lastname.ToLower();

            Console.Write("Correo electrónico: ");
            string email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email))
                throw new FormatException("Por favor ingrese un correo electrónico válido");

            if (!email.Contains('@') || !email.Contains('.'))
                throw new FormatException("Por favor ingrese un correo electrónico con formato válido (ej: usuario@dominio.com)");

            email = email.ToLower();

            Console.Write("Género (M = Masculino, F = Femenino): ");
            string gender = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrWhiteSpace(gender) || (gender != "M" && gender != "F"))
                throw new FormatException("Por favor ingrese un género válido (M o F)");

            gender = gender.ToLower();

            
            Console.Write("Carrera: ");
            string? career = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(career))
                throw new FormatException("Por favor ingrese una carrera válida");

            career = career.ToLower();

            Console.Write("Nota Final (0-100): ");
            string? finalNoteInput = Console.ReadLine();

            if (!double.TryParse(finalNoteInput, out double finalNote))
                throw new FormatException("Por favor ingrese una nota válida");

            if (finalNote < 0 || finalNote > 100)
                throw new FormatException("La nota debe estar entre 0 y 100");

            
            Console.Write("Fecha de nacimiento:\n\n");
            
            Console.Write("Año (ej: 2000): ");
            string? yearInput = Console.ReadLine();
            
            if (!int.TryParse(yearInput, out int year))
                throw new FormatException("Por favor ingrese un año válido");
            
            if (year < 1900 || year > (DateTime.Now.Year - 17))
                throw new FormatException($"El año debe estar entre 1900 y {DateTime.Now.Year - 17}");

            
            Console.Write("Mes (1-12): ");
            string monthInput = Console.ReadLine();
            
            if (!int.TryParse(monthInput, out int month) || month < 1 || month > 12)
                throw new FormatException("Por favor ingrese un mes válido (1-12)");

            
            Console.Write("Día (1-31): ");
            string dayInput = Console.ReadLine();
            
            if (!int.TryParse(dayInput, out int day) || day < 1 || day > 31)
                throw new FormatException("Por favor ingrese un día válido (1-31)");

            
            if (!DateTime.TryParse($"{year}-{month}-{day}", out DateTime dateOfBirth))
                throw new FormatException("Por favor ingrese una fecha de nacimiento válida");

            if (dateOfBirth > DateTime.Now)
                throw new FormatException("La fecha de nacimiento no puede ser futura");

            
            int calculatedAge = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                calculatedAge--;
            
            if (calculatedAge < 17 || calculatedAge > 120)
                throw new FormatException("La edad calculada no es válida (1-120 años)");

            
            Random random = new Random();
            string registrationNumber = $"{DateTime.Now.Year}-{random.Next(1, 999999)}";

            Student student = new Student
            {
                FirstName = name,
                LastName = lastname,
                Email = email,
                Gender = gender,
                Career = career,
                FinalNote = finalNote,
                DateOfBirth = dateOfBirth,
                Age = calculatedAge,
                RegistrationNumber = registrationNumber
            };

            JsonStorage.Save(student);

            Console.WriteLine();
            AnsiConsole.MarkupLine($"[bold green]✓ Estudiante registrado exitosamente![/]");
            AnsiConsole.MarkupLine($"[cyan]Matrícula asignada:[/] {registrationNumber}");
            AnsiConsole.MarkupLine($"[cyan]Edad:[/] {calculatedAge} años");
        }
        catch (FormatException e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] {e.Message}");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
        
    }

    public static List<Student> GetAll()
    {
        return JsonStorage.Load();
    }
    
    public static void Show(List<Student> students = null)
    {
        if (students == null)
        {
            students = GetAll();
            AnsiConsole.MarkupLine($"[bold green]✓ Total de estudiantes: {students.Count}[/]\n");
        }
        MyTable studentsTable = new MyTable("NOMBRE", "EMAIL", "CARRERA", "NOTA FINAL", "MATRICULA", "EDAD");
        foreach (Student student in students)
        {
            studentsTable.AddRow(ToList(student));
        }
        studentsTable.Write();
    }

    public static void Find()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold cyan]Buscar estudiante[/]\n");

            string option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Buscar por: [/]")
                    .PageSize(10)
                    .AddChoices(
                        "Nombre",
                        "Matrícula",
                        "Edad",
                        "Email",
                        "Carrera"
                    ));

            List<Student> foundStudents = option switch
            {
                "Nombre" => FindByName(),
                "Matrícula" => FindByRegistrationNumber(),
                "Edad" => FindByAge(),
                "Email" => FindByEmail(),
                "Carrera" => FindByCareer(),
                _ => new List<Student>()
            };

            Console.WriteLine();

            if (foundStudents.Count == 0)
                AnsiConsole.MarkupLine("[bold yellow]⚠ Warning:[/] No se encontraron estudiantes");
            else
            {
                AnsiConsole.MarkupLine($"[bold green]✓ {foundStudents.Count} estudiante(s) encontrado(s)![/]");
                Console.WriteLine();
                Show(foundStudents);
            }
        }
        catch (FormatException e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] {e.Message}");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }

    private static List<Student> FindByName()
    {
        Console.Write("Nombre: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException("Por favor ingrese un nombre válido");

        return GetAll()
            .Where(s =>
                (s.FirstName?.Contains(input, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (s.LastName?.Contains(input, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    private static List<Student> FindByRegistrationNumber()
    {
        Console.Write("Matrícula: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException("Por favor ingrese una matrícula válida");

        return GetAll()
            .Where(s => string.Equals(s.RegistrationNumber, input, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private static List<Student> FindByAge()
    {
        Console.Write("Edad: ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int age))
            throw new FormatException("Por favor ingrese una edad válida");

        return GetAll()
            .Where(s => s.Age == age)
            .ToList();
    }

    private static List<Student> FindByEmail()
    {
        Console.Write("Email: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException("Por favor ingrese un email válido");

        return GetAll()
            .Where(s => s.Email?.Contains(input, StringComparison.OrdinalIgnoreCase) ?? false)
            .ToList();
    }

    private static List<Student> FindByCareer()
    {
        Console.Write("Carrera: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            throw new FormatException("Por favor ingrese una carrera válida");

        return GetAll()
            .Where(s => s.Career.Contains(input, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static void GetAverage()
    {
        try
        {
            List<Student> students = GetAll();
            
            if (students.Count == 0) AnsiConsole.MarkupLine($"[bold yellow]⚠ Warning:[/] No hay estudiantes registrados");
            else
            {
                double sum = 0;
                foreach (Student student in students) sum += student.FinalNote;
                
                AnsiConsole.MarkupLine($"[bold green]✓ La nota promedio de todos los estudiantes es: {sum / students.Count} [/]");
            }
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }

    public static void ShowHighestGrade()
    {
        try
        {
            List<Student> students = GetAll();

            if (students.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold yellow]⚠ No hay estudiantes registrados[/]");
                return;
            }

            Student topStudent = students.OrderByDescending(s => s.FinalNote).First();

            AnsiConsole.MarkupLine("[bold green]✓ Estudiante con mayor nota:[/]\n");
            AnsiConsole.MarkupLine($"[cyan]Nombre:[/] {Capitalize(topStudent.FullName)}");
            AnsiConsole.MarkupLine($"[cyan]Carrera:[/] {Capitalize(topStudent.Career)}");
            AnsiConsole.MarkupLine($"[cyan]Nota:[/] {topStudent.FinalNote:0.00}");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }

    public static void GetApproved()
    {
        try
        {
            
            List<Student> students = GetAll();
            
            
            List<Student> approvedStudents = students
                .Where(s => s.FinalNote >= 70)
                .ToList();

           
            Console.WriteLine();
            
            if (approvedStudents.Count == 0) AnsiConsole.MarkupLine($"[bold yellow]⚠ Warning:[/] No hay ningún estudiante aprobado");
            else
            {
                AnsiConsole.MarkupLine($"[bold green]✓ Hay {approvedStudents.Count} estudiantes aprobados![/]");
                Console.WriteLine();
            
                Show(approvedStudents);
            }
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }

    public static void GetRejected()
    {
        try
        {
            List<Student> students = GetAll();
            
            List<Student> rejectedStudents = students
                .Where(s => s.FinalNote < 70)
                .ToList();
            
            
            Console.WriteLine();
            
            if (rejectedStudents.Count == 0) AnsiConsole.MarkupLine($"[bold green]✓ No hay estudiantes reprobados![/]");
            else
            {
                AnsiConsole.MarkupLine($"[bold yellow]⚠ Warning:[/] Hay {rejectedStudents.Count} estudiante(s) reprobado(s)!");
                Console.WriteLine();
            
                Show(rejectedStudents);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
        }
    }

    public static void Remove()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold yellow]Eliminar estudiante[/]\n");

            Console.Write("Matrícula del estudiante a eliminar: ");
            string regInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(regInput))
                throw new FormatException("Por favor ingrese una matrícula válida");

            List<Student> students = GetAll();
            Student student = students.FirstOrDefault(s => string.Equals(s.RegistrationNumber, regInput, StringComparison.OrdinalIgnoreCase));

            if (student == null)
                throw new FormatException($"No se encontró ningún estudiante con la matrícula '{regInput}'");

            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]Estudiante encontrado:[/]\n");
            Show(new List<Student> { student });

            Console.Write("\n¿Está seguro de eliminar este estudiante? (y/N): ");
            string confirm = Console.ReadLine()?.ToLower();

            if (confirm != "y")
            {
                AnsiConsole.MarkupLine("[bold yellow]Operación cancelada[/]");
                return;
            }

            JsonStorage.Remove(regInput, Capitalize(student.FullName));
            AnsiConsole.MarkupLine($"[bold green]✓ Estudiante eliminado exitosamente![/]");
        }
        catch (FormatException e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] {e.Message}");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }

    public static void Update()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold yellow]Actualizar estudiante[/]\n");

            Console.Write("Matrícula del estudiante a actualizar: ");
            string regInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(regInput))
                throw new FormatException("Por favor ingrese una matrícula válida");

            List<Student> students = GetAll();
            Student existing = students.FirstOrDefault(s => string.Equals(s.RegistrationNumber, regInput, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
                throw new FormatException($"No se encontró ningún estudiante con la matrícula '{regInput}'");

            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold yellow]Datos actuales:[/]\n");
            Show(new List<Student> { existing });

            Console.WriteLine("\n--- Ingrese los nuevos datos (deje vacío para mantener el valor actual) ---\n");

            Console.Write($"Nombre ({Capitalize(existing.FirstName)}): ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = existing.FirstName;
            else
            {
                if (name.Any(char.IsDigit))
                    throw new FormatException("El nombre no debe contener números");
                name = name.ToLower();
            }

            Console.Write($"Apellido ({Capitalize(existing.LastName)}): ");
            string lastname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastname)) lastname = existing.LastName;
            else
            {
                if (lastname.Any(char.IsDigit)) throw new FormatException("El apellido no debe contener números");
                lastname = lastname.ToLower();
            }

            Console.Write($"Email ({existing.Email}): ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) email = existing.Email;
            else
            {
                if (!email.Contains('@') || !email.Contains('.'))
                    throw new FormatException("Por favor ingrese un correo electrónico con formato válido");
                email = email.ToLower();
            }

            Console.Write($"Género ({existing.Gender.ToUpper()}): ");
            string gender = Console.ReadLine()?.ToUpper();
            if (string.IsNullOrWhiteSpace(gender)) gender = existing.Gender;
            else
            {
                if (gender != "M" && gender != "F") throw new FormatException("Por favor ingrese un género válido (M o F)");
                gender = gender.ToLower();
            }

            Console.Write($"Carrera ({Capitalize(existing.Career)}): ");
            string career = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(career)) career = existing.Career;
            else career = career.ToLower();

            Console.Write($"Nota Final ({existing.FinalNote:0.00}): ");
            string finalNoteInput = Console.ReadLine();
            double finalNote;
            if (string.IsNullOrWhiteSpace(finalNoteInput)) finalNote = existing.FinalNote;
            else
            {
                if (!double.TryParse(finalNoteInput, out finalNote))
                    throw new FormatException("Por favor ingrese una nota válida");
                if (finalNote < 0 || finalNote > 100)
                    throw new FormatException("La nota debe estar entre 0 y 100");
            }

            Console.Write($"Fecha de nacimiento ({existing.DateOfBirth:yyyy-MM-dd}):\n");
            
            Console.Write($"Año ({existing.DateOfBirth.Year}): ");
            string yearInput = Console.ReadLine();
            int year;
            if (string.IsNullOrWhiteSpace(yearInput))
                year = existing.DateOfBirth.Year;
            else
            {
                if (!int.TryParse(yearInput, out year))
                    throw new FormatException("Por favor ingrese un año válido");
                if (year < 1900 || year > DateTime.Now.Year - 17)
                    throw new FormatException($"El año debe estar entre 1900 y {DateTime.Now.Year - 17}");
            }

            Console.Write($"Mes ({existing.DateOfBirth.Month}): ");
            string monthInput = Console.ReadLine();
            int month;
            if (string.IsNullOrWhiteSpace(monthInput))
                month = existing.DateOfBirth.Month;
            else
            {
                if (!int.TryParse(monthInput, out month) || month < 1 || month > 12)
                    throw new FormatException("Por favor ingrese un mes válido (1-12)");
            }

            Console.Write($"Día ({existing.DateOfBirth.Day}): ");
            string dayInput = Console.ReadLine();
            int day;
            if (string.IsNullOrWhiteSpace(dayInput))
                day = existing.DateOfBirth.Day;
            else
            {
                if (!int.TryParse(dayInput, out day) || day < 1 || day > 31)
                    throw new FormatException("Por favor ingrese un día válido (1-31)");
            }

            if (!DateTime.TryParse($"{year}-{month}-{day}", out DateTime dateOfBirth))
                throw new FormatException("Por favor ingrese una fecha de nacimiento válida");

            if (dateOfBirth > DateTime.Now)
                throw new FormatException("La fecha de nacimiento no puede ser futura");

            int calculatedAge = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                calculatedAge--;

            if (calculatedAge < 17 || calculatedAge > 120)
                throw new FormatException("La edad calculada no es válida");

            Student updatedStudent = new Student
            {
                FirstName = name,
                LastName = lastname,
                Email = email,
                Gender = gender,
                Career = career,
                FinalNote = finalNote,
                DateOfBirth = dateOfBirth,
                RegistrationNumber = existing.RegistrationNumber,
                Age = calculatedAge
            };

            JsonStorage.Update(updatedStudent);

            Console.WriteLine();
            AnsiConsole.MarkupLine($"[bold green]✓ Estudiante actualizado exitosamente![/]");
            Show(new List<Student> { updatedStudent });
        }
        catch (FormatException e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] {e.Message}");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[bold red]✗ Error:[/] Ocurrió un error inesperado");
        }
    }


    private static string Capitalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input ?? "";

        return string.Join(" ", input.Split(' ').Select(word =>
            word.Length > 0
                ? char.ToUpper(word[0]) + word.Substring(1)
                : word
        ));
    }

    private static List<string> ToList(Student student)
    {
        List<string> studentList = new List<string>
        {
            Capitalize(student.FullName) ?? "",
            student.Email ?? "",
            Capitalize(student.Career) ?? "",
            student.FinalNote.ToString("0.00") ?? "",
            student.RegistrationNumber ?? "",
            student.Age.ToString() ?? ""
        };
        
        return studentList;
    }
}