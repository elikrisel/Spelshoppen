namespace Spelshoppen.UX;

public class Window
{
    public string Header { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public int MaxWidth { get; set; }
    public List<string> TextRows { get; set; }
    
    public Window(string header, int left, int top, List<string> textRows)
    {
        Header = header;
        Left = left;
        Top = top;
        MaxWidth = 40;
        TextRows = PrepareRows(textRows); 
    }

    
    private List<string> PrepareRows(List<string> originalRows)
    {
        var result = new List<string>();
        foreach (var row in originalRows)
        {
            // if (string.IsNullOrEmpty(row)) 
            // {
            //     result.Add("");
            //     continue;
            // }

            // Om raden är för lång, dela upp den i flera rader
            string[] words = row.Split(' ');
            string currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + word).Length > MaxWidth)
                {
                    result.Add(currentLine.Trim());
                    currentLine = "";
                }
                currentLine += word + " ";
            }
            result.Add(currentLine.Trim());
        }
        return result;
    }

    public void Draw()
    {
        // Nu kan vi säkert räkna ut bredden eftersom PrepareRows har kört
        var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault()?.Length ?? 0;

        if (width < Header.Length + 4)
        {
            width = Header.Length + 4;
        }

        // Rita Header
        Console.SetCursorPosition(Left, Top);
        if (Header != "")
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(Header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" " + new String('─', width - Header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new String('─', width + 2) + '┐');
        }

        // Rita raderna i sträng-Listan
        for (int j = 0; j < TextRows.Count; j++)
        {
            Console.SetCursorPosition(Left, Top + j + 1);
            Console.WriteLine('│' + " " + TextRows[j] + new String(' ', width - TextRows[j].Length + 1) + '│');
        }

        // Rita undre delen av fönstret
        Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
        Console.Write('└' + new String('─', width + 2) + '┘');

        // Uppdatera LowestPosition
        if (Lowest.LowestPosition < Top + TextRows.Count + 2)
        {
            Lowest.LowestPosition = Top + TextRows.Count + 2;
        }

        Console.SetCursorPosition(0, Lowest.LowestPosition);
    }
}
    public static class Lowest
    {
        public static int LowestPosition { get; set; }
    }


