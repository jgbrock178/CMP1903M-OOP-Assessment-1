
namespace CMP1903M_Assessment_1_Base_Code;

/// <summary>
/// Methods for interacting with the console. Also wraps the <c>Console.WriteLine</c> and <c>Console.Readline</c> for a
/// better user experience. As such, no code should reference the <c>Console</c> class except this one.
/// </summary>
public static class ConsoleGui
{
    /// <summary>
    /// Controls the width of the output to the console.
    /// </summary>
    public static int ConsoleWidth { get; set; } = 80;

    /// <summary>
    /// Controls the amount of padding in the terminal.
    /// </summary>
    public static int ConsoleLeftPadding { get; set; }

    /// <summary>
    /// Tracks how many lines has output to the console. This is to ensure the border outputs a "punched hole" on every
    /// other line.
    /// Use of ENCAPSULATION and ABSTRACTION here as only this class needs to know this.
    /// </summary>
    private static int _consoleLines;
    
    /// <summary>
    /// Whether the border should be closed on the right side. Generally true, but if an unknown user input length is
    /// possible then better to not close the border.
    /// Use of ENCAPSULATION and ABSTRACTION here as only this class needs to know this.
    /// </summary>
    private static bool _closeBorder = true;
    
    /// <summary>
    /// A method to force the border type/style to be a certain way. Should be reset to an empty string once used.
    /// Use of ENCAPSULATION and ABSTRACTION here as only this class needs to know this.
    /// </summary>
    private static string _borderOverride = "";

    /// <summary>
    /// The border level to be output. Valid valued are 0, 1 and 2.
    /// Use of ENCAPSULATION and ABSTRACTION here as only this class needs to know this.
    /// </summary>
    private static int _borderLevel;

    /// <summary>
    /// A way to specify how the console should look. Console will continue to look this way until called again.
    /// Example of EXCEPTION HANDLING.
    /// </summary>
    /// <param name="borderLevel">The border level to use. Valid values are 0, 1 or 2.</param>
    /// <param name="closeBorder">whether to close the border. Default value is true.</param>
    /// <param name="borderOverride">if set, forces the next line to be a certain border style.</param>
    /// <exception cref="InvalidBorderLevelException"></exception>
    /// <exception cref="InvalidBorderTypeException"></exception>
    public static void SetBorder(int borderLevel, bool closeBorder = true, string borderOverride = "")
    {
        string[] validBorderTypes = new string[] { "punched", "plain" };
        
        if (borderLevel > 2)
        {
            throw new InvalidBorderLevelException(borderLevel);
        }

        if (borderOverride != "" && !validBorderTypes.Contains(borderOverride))
        {
            throw new InvalidBorderTypeException(borderOverride);
        }
        
        _borderLevel = borderLevel;
        _borderOverride = borderOverride;
        _closeBorder = closeBorder;
    }

    /// <summary>
    /// Clears the terminal screen and moves the cursor to the top-left.
    /// Included so all calls in other classes can ignore the inbuilt <c>Console</c> class. Also means this can be
    /// tested for.
    /// </summary>
    public static void Clear()
    {
        Console.Clear();
    }

    /// <summary>
    /// Clears the current terminal line from the cursor to the end of the line.
    /// Use of ABSTRACTION and ENCAPSULATION. When used, user doesn't need to know implementation. Plus set to private as
    /// only used by this class.
    /// </summary>
    private static void ClearLine()
    {
        Console.Write($"\u001b[0K");
    }

    /// <summary>
    /// Wrapper for the <c>console.ReadLine()</c>. Prints the prompt, moves the cursor up a line and then to 2 spaces after
    /// the end of the prompt. Passes through options to <c>WriteLine</c> method for formatting.
    /// Example of ABSTRACTION.
    /// </summary>
    /// <param name="prompt">the text to the shown to the user.</param>
    /// <returns>Returns what the user has typed into the console.</returns>
    public static string ReadLine(string prompt)
    {
        int cursorPromptPosition = ConsoleLeftPadding + prompt.Length + 1;
        if (prompt.Length == 0)
        {
            cursorPromptPosition -= 1;
        }
        
        if (_borderLevel >= 1)
        {
            cursorPromptPosition += 6;
        }
        if (_borderLevel >= 2)
        {
            cursorPromptPosition += 2;
        }
        
        WriteLine(prompt);
        MoveUp();
        MoveRight(cursorPromptPosition);
        return Console.ReadLine()!;
    }

    /// <summary>
    /// Wraps the content with a border to look like printed paper.
    /// If <c>borderLevel == 2</c> then no closing border is output, no matter what value is passed
    /// for <c>closingBorder</c>.
    /// </summary>
    /// <param name="content">
    /// the content for the line(s). If set to any of the following, then will output special lines:
    /// "&lt;topBorder&gt;" - Will output a top border if the <c>borderLevel</c> is 2.
    /// "&lt;bottomBorder&gt;" - Will output a bottom border if the <c>borderLevel</c> is 2.
    /// "&lt;hr&gt;" - will output a horizontal line at any border level.
    /// </param>
    /// <returns>Returns the content input wrapped in a border.</returns>
    private static string FormatAsPaper(string content)
    {
        string output = new String(' ', ConsoleLeftPadding) + "";
        int contentWidth = ConsoleLeftPadding + ConsoleWidth;

        if (_borderLevel >= 1)
        {
            if (PunchedBorder() || _borderOverride == "punched")
            {
                output += "┃ O ┃";
            }
            else
            {
                output += "┃   ┃";
            }
        }

        if (_borderLevel >= 2 && content.Trim() != "<borderTop>" && content.Trim() != "<borderBottom>")
        {
            output += " │";
        }
        else if (_borderLevel >= 2 && content.Trim() == "<borderTop>")
        {
            output += " ╭";
        }
        else if (_borderLevel >= 2 && content.Trim() == "<borderBottom>")
        {
            output += " ╰";
        }
        
        // Sets the available width - removing space for closing border if needed.
        contentWidth -= output.Length;
        if (_closeBorder && _borderLevel == 1)
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
        
        if (_closeBorder && _borderLevel == 1)
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
    /// constrained to the <c>ConsoleWidth</c> specified on the class. Also increases <c>_consoleLines</c> to ensure the
    /// correct border style can be output. <see cref="FormatAsPaper"/>
    /// </summary>
    /// <param name="content">the content to be output.</param>
    public static void WriteLine(string content)
    {
        List<string> splitText = SplitTextByLength(content, ConsoleWidth - 12);

        foreach (string line in splitText)
        {
            _consoleLines += 1;
            Console.WriteLine($"{FormatAsPaper(line)}\u001b[0K");   
        }

        // Reset _borderOverride as line has been output.
        _borderOverride = "";
    }

    /// <summary>
    /// Moves the cursor up one line and outputs a line in the same was as <see cref="WriteLine"/>, except
    /// <c>_consoleLines</c> is NOT incremented. This is because the line is replacing a line already output.
    /// </summary>
    /// <param name="content">the content to be printed.</param>
    public static void ReplaceLine(string content)
    {
        MoveUp();
        Console.WriteLine($"{FormatAsPaper(content)}\u001b[0K");
        
        // Reset _borderOverride as line has been output.
        _borderOverride = "";
    }
    
    /// <summary>
    /// Whether to output a printed border or not, based on the <c>_consoleLines</c> value.
    /// </summary>
    /// <returns>Boolean value whether to output a punched style border.</returns>
    private static bool PunchedBorder()
    {
        if (_consoleLines % 2 == 0)
        {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Outputs a prompt to the user and then ensures only valid values can be returned. If the user enters an incorrect
    /// value, an error message is displayed and the cursor returns to the original prompt position.
    /// </summary>
    /// <param name="prompt">the text to show to the user.</param>
    /// <param name="validInputs">a string array of valid values.</param>
    /// <param name="caseSensitive">whether the values are case sensitive. Default is false.</param>
    /// <returns>Returns a string of the valid value entered by the user.</returns>
    public static string GetValidatedUserInput(string prompt, string[] validInputs, bool caseSensitive = false)
    {
        string option = "";
        bool borderPunched = PunchedBorder();
        
        if (!borderPunched)
        {
            _borderOverride = "punched";
        }
        
        while (!validInputs.Contains(option))
        {
            option = ReadLine(prompt);
            
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
                WriteLine("Invalid option, please try again.");
                MoveUp(2);
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
        WriteLine("<hr>");
        WriteLine(@"  _______        _                          _                     ");
        WriteLine(@" |__   __|      | |       /\               | |                    ");
        WriteLine(@"    | | _____  _| |_     /  \   _ __   __ _| |_   _ ___  ___ _ __ ");
        WriteLine(@"    | |/ _ \ \/ / __|   / /\ \ | '_ \ / _` | | | | / __|/ _ \ '__|");
        WriteLine(@"    | |  __/>  <| |_   / ____ \| | | | (_| | | |_| \__ \  __/ |   ");
        WriteLine(@"    |_|\___/_/\_\\__| /_/    \_\_| |_|\__,_|_|\__, |___/\___|_|   ");
        WriteLine(@"                                               __/ |              ");
        WriteLine(@"                                              |___/               ");
        WriteLine("");
        WriteLine("<hr>");
    }
    
    /// <summary>
    /// Prints a fancy looking menu with options to the console.
    /// </summary>
    public static void PrintMainMenu()
    {
        WriteLine("What would you like to do?");
        WriteLine("");
        WriteLine("  1) Manually write text");
        WriteLine("  2) Import text file");
        WriteLine("  3) Perform analyse test");
        WriteLine("  4) Quit");
        WriteLine("<hr>");
    }

    /// <summary>
    /// Moves the console cursor up.
    /// Example of ENCAPSULATION.
    /// </summary>
    /// <param name="lines">the number of lines to move up by. Default is 1.</param>
    private static void MoveUp(int lines = 1)
    {
        Console.Write($"\u001b[{lines}A");
    }

    /// <summary>
    /// Moves the console cursor to the right.
    /// Example of ENCAPSULATION.
    /// </summary>
    /// <param name="chars">the number of characters to move by. Default is 1.</param>
    private static void MoveRight(int chars = 1)
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