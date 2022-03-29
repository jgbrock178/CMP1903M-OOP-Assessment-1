using System.Collections.Generic;

namespace CMP1903M_Assessment_1_Base_Code;

public class ConsoleGUI
{
    public static int ConsoleWidth = 80;
    private static int ConsoleLines = 0;
    
    /// <summary>
    /// Clears the terminal screen.
    /// </summary>
    public static void Clear()
    {
        Console.Write($"\u001b[2J\x1b[H\x1b[J");
    }

    /// <summary>
    /// Clears the current terminal line from the cursor to the end of the line.
    /// </summary>
    public static void ClearLine()
    {
        Console.Write($"\u001b[0K");
    }

    /// <summary>
    /// Wrappper the <c>console.ReadLine()</c>. Prints the prompt, moves the cursor up a line and then to 2 spaces after
    /// the end of the prompt. Passes through options to <c>WriteLine</c> method for formatting.
    /// </summary>
    /// <param name="prompt">the text to the shown to the user.</param>
    /// <param name="borderLevel">how many borders to show</param>
    /// <param name="closingBorder">whether to show a closing border.</param>
    /// <param name="borderOverride">defines the border style if needed.</param>
    /// <returns>Returns what the user has typed into the console.</returns>
    public static string ReadLine(string prompt, int borderLevel = 0, bool closingBorder = true, string borderOverride = "")
    {
        int cursorPromptPosition = prompt.Length + 1;
        if (prompt.Length == 0)
        {
            cursorPromptPosition -= 1;
        }
        
        if (borderLevel >= 1)
        {
            cursorPromptPosition += 6;
        }
        if (borderLevel >= 2)
        {
            cursorPromptPosition += 2;
        }
        
        WriteLine(prompt, borderLevel, closingBorder, borderOverride);
        MoveUp(1);
        MoveRight(cursorPromptPosition);
        string output = Console.ReadLine()!;
        return output;
    }

    /// <summary>
    /// Wraps the content with a border to look like printed paper.
    /// If <c>borderLevel == 2</c> then no closing border is output, no matter what value is passed
    /// for <c>closingBorder</c>.
    /// </summary>
    /// <param name="content">
    /// the content for the line(s). If set to any of the following, then will output special lines:
    /// "<topBorder>" - Will output a top border if the <c>borderLevel</c> is 2.
    /// "<bottomBorder>" - Will output a bottom border if the <c>borderLevel</c> is 2.
    /// "<hr>" - will output a horizontal line at any border level.
    /// </param>
    /// <param name="borderLevel">how many border to output. Can be 0, 1 or 2.</param>
    /// <param name="closingBorder">whether to close the outer border.</param>
    /// <param name="borderOverride">
    /// used to override the border style. Valid value is "punched".
    /// Any other non-empty string results in a plain border.
    /// </param>
    /// <returns>Returns the content input wrapped in a border.</returns>
    private static string FormatAsPaper(string content, int borderLevel = 0, bool closingBorder = true, string borderOverride = "")
    {
        string output = "";
        int contentWidth = ConsoleWidth;

        if (borderLevel >= 1)
        {
            if (PunchedBorder() || borderOverride == "punched")
            {
                output += "┃ O ┃";
            }
            else
            {
                output += "┃   ┃";
            }
        }

        if (borderLevel >= 2 && content.Trim() != "<borderTop>" && content.Trim() != "<borderBottom>")
        {
            output += " │";
        }
        else if (borderLevel >= 2 && content.Trim() == "<borderTop>")
        {
            output += " ╭";
        }
        else if (borderLevel >= 2 && content.Trim() == "<borderBottom>")
        {
            output += " ╰";
        }
        
        // Sets the available width - removing space for closing border if needed.
        contentWidth -= output.Length;
        if (closingBorder && borderLevel == 1)
        {
            contentWidth -= 5;
        }

        switch (content.Trim())
        {
            case "<hr>":
                output += new String('-', contentWidth);
                break;
            case "<borderTop>":
            case "<borderBottom>":
                output += new String('─', contentWidth) + "┄┄┄┄";
                break;
            default:
                output += $" {content} ".PadRight(contentWidth);
                break;
        }
        
        if (closingBorder && borderLevel == 1)
        {
            if (PunchedBorder())
            {
                output += "┃ O ┃";
            }
            else
            {
                output += "┃   ┃";
            }
        }

        return output;
    }

    /// <summary>
    /// Wrapper for <c>console.WriteLine()</c> that will ensure all lines are bordered appropriately, as well as
    /// constrained to the <c>ConsoleWidth</c> specified on the class. Also increases <c>ConsoleLines</c> to ensure the
    /// correct border style can be output. <see cref="FormatAsPaper"/>
    /// </summary>
    /// <param name="content">the content to be output.</param>
    /// <param name="borderLevel">the border level to be styled.</param>
    /// <param name="closingBorder">whether to include a closing border.</param>
    /// <param name="borderOverride">allows manual border to be set.</param>
    public static void WriteLine(string content, int borderLevel = 0, bool closingBorder = true, string borderOverride = "")
    {
        List<string> splitText = SplitTextByLength(content, ConsoleWidth - 12);

        foreach (string line in splitText)
        {
            ConsoleLines += 1;
            Console.WriteLine($"{FormatAsPaper(line, borderLevel, closingBorder, borderOverride)}\u001b[0K");   
        }
    }

    /// <summary>
    /// Moves the cursor up one line and outputs a line in the same was as <see cref="WriteLine"/>, except
    /// <c>ConsoleLines</c> is NOT incremented. This is because the line is replacing a line already output.
    /// </summary>
    /// <param name="content">the content to be printed.</param>
    /// <param name="borderLevel">the border level to be styled.</param>
    /// <param name="closingBorder">whether to include a closing border.</param>
    /// <param name="borderOverride">allows border style to be set.</param>
    public static void ReplaceLine(string content, int borderLevel = 0, bool closingBorder = true, string borderOverride = "")
    {
        MoveUp();
        Console.WriteLine($"{FormatAsPaper(content, borderLevel, closingBorder, borderOverride)}\u001b[0K");
    }
    
    /// <summary>
    /// Whether to output a printed border or not, based on the <c>ConsoleLines</c> value.
    /// </summary>
    /// <returns>Boolean value whether to output a punched style border.</returns>
    private static bool PunchedBorder()
    {
        if (ConsoleLines % 2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    /// <summary>
    /// Outputs a prompt to the user and then ensures only valid values can be returned. If the user enters an incorrect
    /// value, an error message is displayed and the cursor returns to the original prompt position.
    /// </summary>
    /// <param name="prompt">the text to show to the user.</param>
    /// <param name="validInputs">a string array of valid values.</param>
    /// <param name="caseSensitive">whether the values are case sensitive. Default is false.</param>
    /// <param name="borderLevel">the border level to be styled.</param>
    /// <param name="closingBorder">whether to include a closing border.</param>
    /// <returns>Returns a string of the valid value entered by the user.</returns>
    public static string GetValidatedUserInput(string prompt, string[] validInputs, bool caseSensitive = false, 
        int borderLevel = 0, bool closingBorder = true)
    {
        string option = "";
        string borderOverride = "";
        bool errorShown = false;
        bool borderPunched = PunchedBorder();
        
        if (!borderPunched)
        {
            borderOverride = "punched";
        }
        
        while (!validInputs.Contains(option))
        {
            option = ReadLine(prompt, borderLevel, closingBorder, borderOverride);
            
            // Ensure all valid options and option is lowercase if not case-sensitive.
            if (!caseSensitive)
            {
                for (int i = 0; i < validInputs.Length; i++)
                {
                    validInputs[i] = validInputs[i].ToLower();
                }

                option = option.ToLower();
            }
            
            if (!validInputs.Contains(option))
            {
                WriteLine("Invalid option, please try again.", borderLevel, closingBorder);
                MoveUp(2);
                errorShown = true;
            }
        }
        ClearLine();
        return option;
    }

    /// <summary>
    /// Prints the app title to the console.
    /// </summary>
    public static void PrintTitle()
    {
        WriteLine("", 1);
        WriteLine("<hr>", 1);
        WriteLine("TEXT ANALYSER", 1);
        WriteLine("Analyse your text!", 1);
        WriteLine("<hr>", 1);
    }
    
    /// <summary>
    /// Prints a fancy looking menu with options to the console.
    /// </summary>
    public static void PrintMainMenu()
    {
        WriteLine("What would you like to do?", 1);
        WriteLine("  1) Write text via console", 1);
        WriteLine("  2) Import text file", 1);
        WriteLine("  3) Perform analyse test", 1);
        WriteLine("  4) Quit", 1);
        WriteLine("<hr>", 1);
    }

    /// <summary>
    /// Moves the console cursor up.
    /// </summary>
    /// <param name="lines">the number of lines to move up by. Default is 1.</param>
    public static void MoveUp(int lines = 1)
    {
        Console.Write($"\u001b[{lines}A");
    }

    /// <summary>
    /// Moves the console cursor to the right.
    /// </summary>
    /// <param name="chars">the number of characters to move by. Default is 1.</param>
    public static void MoveRight(int chars = 1)
    {
        Console.Write($"\u001b[{chars}C");
    }

    /// <summary>
    /// Moves the console cursor to the left.
    /// </summary>
    /// <param name="chars">the number of characters to move by. Default is 1.</param>
    public static void MoveLeft(int chars = 1)
    {
        Console.Write($"\u001b[{chars}D");
    }
    
    /// <summary>
    /// Splits the text into multiple strings based on the given length.
    /// </summary>
    /// <param name="text">the input text to be analysed and split.</param>
    /// <param name="length">the maximum length for the text to be split into.</param>
    /// <returns>A list of strings representing the lines.</returns>
    private static List<string> SplitTextByLength(string text, int length)
    {
        List<string> splitText = new List<string>();
        string line = "";
        string[] words = text.Split(' ');
        
        foreach (string word in words)
        {
            if (line.Trim().Length + word.Length + 1 >= length)
            {
                splitText.Add(line.Trim());
                line = $"{word} ";
            }
            else
            {
                line += $"{word} ";
            }
        }

        if (line.Length > 0)
        {
            splitText.Add(line);
        }

        return splitText;
    }
}