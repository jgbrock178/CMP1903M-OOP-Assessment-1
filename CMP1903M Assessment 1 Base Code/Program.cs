//Base code project for CMP1903M Assessment 1
using System.Text;

namespace CMP1903M_Assessment_1_Base_Code;

/// <summary>
/// Main program.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method for program execution.
    /// </summary>
    public static void Main()
    {
        // Force UTF-8 output
        Console.OutputEncoding = Encoding.UTF8;

        string text = "";
        Input input = new Input();
            
        ConsoleGui.Clear();
        ConsoleGui.SetBorder(1);
        ConsoleGui.ConsoleLeftPadding = 4;
        ConsoleGui.PrintTitle();
        while (true)
        {
            ConsoleGui.SetBorder(1);
            ConsoleGui.PrintMainMenu();
            string option = ConsoleGui.GetValidatedUserInput("Choose option:", 
                new[] { "1", "2", "3", "4" });

            switch (option)
            {
                case "1":
                    text = input.ManualTextInput();
                    break;
                case "2":
                    text = input.FileTextInput();
                    break;
                case "3":
                {
                    ConsoleGui.WriteLine("<hr>");
                    ConsoleGui.WriteLine("Analysis Validation");
                    ConsoleGui.WriteLine("<hr>");
                
                    // Validate the Analyse.AnalyseText method to know text.
                    ConsoleGui.WriteLine(TestAnalysis.ValidateTestFile("known_test_file.txt")
                        ? "Test file validated - Analysis working correctly."
                        : "Oh no! Test file NOT validated. Something's wrong.");
                    ConsoleGui.WriteLine("<hr>");
                    ConsoleGui.WriteLine("Test Validation Complete!");
                    ConsoleGui.SetBorder(0);
                    ConsoleGui.WriteLine("<hr>");
                    ConsoleGui.SetBorder(1);
                
                    // Pause for effect.
                    Thread.Sleep(1000);
                    continue;
                }
            }

            if (text == "<mainMenu>")
            {
                ConsoleGui.WriteLine("<hr>");
                continue;
            } 
            
            if (text == "<quit>" || option == "4")
            {
                option = ConsoleGui.GetValidatedUserInput("Are you sure you want to quit? [Y]es or [N]o:",
                    new[] {"y", "n"});
                if (option == "y")
                {
                    ConsoleGui.WriteLine("<hr>");
                    ConsoleGui.WriteLine("Program terminating. Goodbye!");
                    ConsoleGui.SetBorder(0);
                    ConsoleGui.WriteLine("<hr>");
                    Environment.Exit(0);
                }
                else
                {
                    ConsoleGui.WriteLine("<hr>");
                    continue;
                }
            }

            Analyse analysis = new Analyse();

            Dictionary<string, int> wordAnalysis = analysis.WordList(text);
            int maxWordLength = 0;
            if (wordAnalysis.Count > 0)
            {
                // This gets the length of the maximum word length. This is so the program knows whether to prompt
                // the user to save a long words file. No point if no long words.
                maxWordLength = wordAnalysis.Max(x => x.Key.Length);
            }

            Report.OutputToConsole(analysis.AnalyseText(text), analysis.LetterFrequency(text),
                wordAnalysis);
            
            ConsoleGui.WriteLine("<hr>");

            if (wordAnalysis.Count > 0 && maxWordLength >= 7)
            {
                option = ConsoleGui.GetValidatedUserInput("Write long words to file? [Y]es or [N]o:",
                    new[] {"y", "n"});

                if (option == "y")
                {
                    string filename = ConsoleGui.ReadLine("Enter filename:");
                    filename = Report.LongWordsToFile(analysis.WordList(text), filename);
                    ConsoleGui.WriteLine($"File saved as {filename}");
                
                }
            }
            // No words entered - only numbers and special characters.
            else if (wordAnalysis.Count == 0)
            {
                ConsoleGui.WriteLine("There are no words.");
            }
            // No words wit a length >= 7 characters.
            else
            {
                ConsoleGui.WriteLine("There are no long words (with 7 or more characters).");
            }
            ConsoleGui.WriteLine("<hr>");
            ConsoleGui.WriteLine("Analysis Complete!");
            ConsoleGui.SetBorder(0);
            ConsoleGui.WriteLine("<hr>");
            ConsoleGui.SetBorder(1);
            // Pause for effect.
            Thread.Sleep(1000);
        }
    }
}
