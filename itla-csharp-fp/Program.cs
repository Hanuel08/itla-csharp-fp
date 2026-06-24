using Spectre.Console;

using itla_csharp_fp.Service;

bool running = true;

while (running)
{
    Console.Clear();
    
    string option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[yellow]Seleccione una opción[/]")
            .PageSize(10)
            .AddChoices(
                "Registrar estudiante",
                "Mostrar estudiantes",
                "Buscar estudiante",
                "Actualizar estudiante",
                "Eliminar estudiante",
                "Calcular promedio",
                "Mayor nota",
                "Estudiantes aprobados",
                "Estudiantes reprobados",
                "Salir"
            ));


    switch (option)
    {
        case "Registrar estudiante":
            Console.Clear();
            StudentService.Register();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;
        
        case "Mostrar estudiantes":
            Console.Clear();
            StudentService.Show();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;
        
        case "Buscar estudiante":
            Console.Clear();
            StudentService.Find();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Actualizar estudiante":
            Console.Clear();
            StudentService.Update();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Eliminar estudiante":
            Console.Clear();
            StudentService.Remove();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Calcular promedio":
            Console.Clear();
            StudentService.GetAverage();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Mayor nota":
            Console.Clear();
            StudentService.ShowHighestGrade();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Estudiantes aprobados":
            Console.Clear();
            StudentService.GetApproved();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Estudiantes reprobados":
            Console.Clear();
            StudentService.GetRejected();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            break;

        case "Salir":
            running = false;
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Gracias por su visita!")
                    .Color(Color.Red));
            break;
    }
}












