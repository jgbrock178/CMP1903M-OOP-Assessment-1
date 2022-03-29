//Base code project for CMP1903M Assessment 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMP1903M_Assessment_1_Base_Code
{
    class Program
    {
        static void Main()
        {
            // Force UTF-8 output
            Console.OutputEncoding = Encoding.UTF8;

            string text = "";
            Input input = new Input();
                
            ConsoleGUI.Clear();
            ConsoleGUI.PrintTitle();
            while (true)
            {
                ConsoleGUI.PrintMainMenu();
                string option = ConsoleGUI.GetValidatedUserInput("Choose option:",
                    new string[] { "1", "2", "3", "4" },
                    borderLevel: 1, 
                    closingBorder: true);

                if (option == "1")
                {
                    text = input.ManualTextInput();
                }
                else if (option == "2")
                {
                    text = input.fileTextInput();
                }
                else if (option == "3")
                {
                    ConsoleGUI.WriteLine("<hr>", 1);
                    ConsoleGUI.WriteLine("Analysis Validation", 1);
                    ConsoleGUI.WriteLine("<hr>", 1);
                    if (TestAnalysis.ValidateTestFile())
                    {
                        ConsoleGUI.WriteLine("Test file validated - Analysis working correctly", 1);
                    }
                    else
                    {
                        ConsoleGUI.WriteLine("Oh no! Test file NOT validated. Something's wrong.", 1);
                    }
                    ConsoleGUI.WriteLine("<hr>", 1);
                    continue;
                }

                if (text == "<mainmenu>")
                {
                    continue;
                } 
                else if (text == "<quit>" || option == "4")
                {
                    option = ConsoleGUI.GetValidatedUserInput("Are you sure you want to quit? [Y]es or [N]o:",
                        new string[] {"y", "n"}, borderLevel: 1, closingBorder: true);
                    if (option == "y")
                    {
                        ConsoleGUI.WriteLine("<hr>", 1);
                        ConsoleGUI.WriteLine("Program terminating. Goodbye!", 1);
                        ConsoleGUI.WriteLine("<hr>");
                        Environment.Exit(0);
                    }
                    else
                    {
                        ConsoleGUI.WriteLine("<hr>", 1);
                        continue;
                    }
                }

                Analyse analysis = new Analyse();

                Dictionary<string, int> wordAnalysis = analysis.WordList(text);

                Report.outputToConsole(analysis.AnalyseText(text), analysis.LetterFrequency(text),
                    wordAnalysis);
                
                ConsoleGUI.WriteLine("<hr>", 1);

                if (wordAnalysis.Count > 0)
                {
                    option = ConsoleGUI.GetValidatedUserInput("Write long words to file? [Y]es or [N]o:",
                        new string[] {"y", "n"}, borderLevel: 1, closingBorder: true);

                    if (option == "y")
                    {
                        string filename = ConsoleGUI.ReadLine("Enter filename:", 1, true);
                        filename = Report.longWordsToFile(analysis.WordList(text), filename);
                        ConsoleGUI.WriteLine($"File saved as {filename}", 1);
                    
                    }
                    ConsoleGUI.WriteLine("<hr>", 1);
                }
                ConsoleGUI.WriteLine("Analysis Complete!", 1);
                ConsoleGUI.WriteLine("<hr>");
            }
        }
    }
}
