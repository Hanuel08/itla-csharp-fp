using System.Text.Json;
using itla_csharp_fp.Model;
using Spectre.Console;

namespace itla_csharp_fp.Utils;

public class JsonStorage
{
    // para que pueda ser leido en windows y linux
    private static string BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data"));
    private static string PATH = Path.Combine(BasePath, "students.json");
    private static string LOG_PATH = Path.Combine(BasePath, "logs.txt");
    
    private static void Log(string message)
    {
        try
        {
            File.AppendAllText(LOG_PATH, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
        }
    }

    public static void Save(Student student)
    {
        try
        {
            List<Student> students = Load();
            students.Add(student);

            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(PATH, json);

            Log($"SAVE - Estudiante registrado: {student.RegistrationNumber} ({student.FullName})");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
        }
    }

    public static List<Student> Load()
    {
        try
        {
            if (!File.Exists(PATH)) return new List<Student>();

            string json = File.ReadAllText(PATH);

            return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
            return new List<Student>();
        }
    }

    public static void Remove(string registrationNumber, string studentName)
    {
        try
        {
            List<Student> students = Load();
            students.RemoveAll(s => string.Equals(s.RegistrationNumber, registrationNumber, StringComparison.OrdinalIgnoreCase));

            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );
            File.WriteAllText(PATH, json);

            Log($"REMOVE - Estudiante eliminado: {registrationNumber} ({studentName})");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
        }
    }

    public static void Update(Student updatedStudent)
    {
        try
        {
            List<Student> students = Load();
            int index = students.FindIndex(s => string.Equals(s.RegistrationNumber, updatedStudent.RegistrationNumber, StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                students[index] = updatedStudent;

                string json = JsonSerializer.Serialize(students, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                );
                File.WriteAllText(PATH, json);

                Log($"UPDATE - Estudiante actualizado: {updatedStudent.RegistrationNumber} ({updatedStudent.FullName})");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error:[/] Ocurrió un error inesperado: {e.Message}");
        }
    }
}