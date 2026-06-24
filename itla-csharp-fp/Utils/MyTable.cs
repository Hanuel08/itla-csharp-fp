using Spectre.Console;

namespace itla_csharp_fp.Utils;

public class MyTable
{
    private List<string> Head = new List<string>();
    private List<int> rowLength = new List<int>();
    private List<List<string>> matrix = new List<List<string>>();
    
    public MyTable(params List<string> head)
    {
        Head = head;
        for (int i = 0; i < Head.Count; i++) rowLength.Add(Head[i].Length);
    }

    private string DrawRowLine(int num)
    {
        string line = "";
        for (int i = 0; i < num; i++) line += "-";
        return line;
    }

    private string FillRowLineSpace(int num)
    {
        string space = "";
        for (int i = 0; i < num; i++) space += " ";
        return space;
    }

    public void AddRow(params List<string> row)
    {
        for (int i = 0; i < row.Count; i++) if (row[i].Length > rowLength[i]) rowLength[i] = row[i].Length;
        matrix.Add(row);
    }

    public void Write()
    {
        string rowLine = "";

        for (int i = 0; i < rowLength.Count; i++)
        {
            if (i == 0) rowLine += "+";
            rowLine += $"{DrawRowLine(rowLength[i] + 2)}+";
        }


        for (int i = 0; i < Head.Count; i++)
        {
            if (i == 0)
            {
                Console.WriteLine(rowLine);
                Console.Write($"| ");
            }
            //Console.Write(Head[i].ToUpper());
            AnsiConsole.Markup($"[bold white]{Head[i].ToUpper()}[/]");
            Console.Write($"{FillRowLineSpace(rowLength[i] - Head[i].Length)} | ");
        }
        
        Console.WriteLine();

        if (matrix.Count == 0)
        {
            Console.WriteLine(rowLine);
            return;
        }

        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                string cell =  matrix[i][j];
                if (j == 0)
                {
                    Console.WriteLine(rowLine);
                    Console.Write($"| ");
                }
                Console.Write(cell);
                Console.Write($"{FillRowLineSpace(rowLength[j] - cell.Length)} | ");
            }
            Console.WriteLine();
            if (i == matrix.Count - 1) Console.WriteLine(rowLine);
        }
    }
}