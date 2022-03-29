using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903M_Assessment_1_Base_Code
{
    class Report
    {
        /// <summary>
        /// Outputs a report of the analysis to the console.
        /// </summary>
        /// <param name="metrics">dictionary containing each metric and the count.</param>
        /// <param name="letter_analysis">dictionary containing analysis of letters and their frequencies.</param>
        /// <param name="words">A dictionary of words and their frequencies.</param>
        public static void outputToConsole(Dictionary<string, int> metrics, Dictionary<string, int> letter_analysis, 
            Dictionary<string, int> words)
        {
            int max_metric_length = metrics.Max(x => x.Key.Length);
            
            ConsoleGUI.WriteLine("<hr>", 1);
            ConsoleGUI.WriteLine("Text Analysis Report", 1);
            ConsoleGUI.WriteLine("<hr>", 1);

            foreach (var metric in metrics)
            {
                ConsoleGUI.WriteLine($"  Number of {metric.Key.PadRight(max_metric_length)} : {metric.Value}", 1);
            }
            
            ConsoleGUI.WriteLine("<hr>", 1);
            ConsoleGUI.WriteLine("Letter Analysis", 1);
            ConsoleGUI.WriteLine("<hr>", 1);

            if (words.Count == 0)
            {
                ConsoleGUI.WriteLine("No letters or words in input.", 1);
            }
            else
            {
                ConsoleGUI.WriteLine("Top 5 Letters", 1);

                int last_letter_count = 0;
                int i = 1;
                List<string> output = new List<string>();

                foreach (var letter in letter_analysis.Take(5))
                {
                    output.Add($"  {i}. {letter.Key.ToUpper()} x {letter.Value}");
                    last_letter_count = letter.Value;
                    i++;
                }

                string grouped_letters = " ";
                int max_letter_count = 0;
                if (letter_analysis.Count > 0)
                {
                    max_letter_count = letter_analysis.Max(x => x.Value.ToString().Length);
                }

                bool title_added = false;

                foreach (var letter in letter_analysis.Reverse().Take(letter_analysis.Count - 5).Reverse())
                {
                    string new_letter = "";
                    if (last_letter_count == letter.Value)
                    {
                        new_letter = $", {letter.Key.ToUpper()} x {letter.Value}";
                        if ((output[^1] + new_letter).Length > ConsoleGUI.ConsoleWidth - 12)
                        {
                            output.Add($"     {letter.Key.ToUpper()} x {letter.Value}");
                        }
                        else
                        {
                            output[^1] = $"{output[^1]}{new_letter}";
                        }

                        continue;
                    }
                    else if (!title_added)
                    {
                        output.Add("");
                        output.Add("Other Letters...");
                        title_added = true;
                    }

                    new_letter = $" {letter.Key.ToUpper()} x {letter.Value.ToString().PadRight(max_letter_count)} ";
                    if ((grouped_letters + new_letter).Length > ConsoleGUI.ConsoleWidth - 12)
                    {
                        output.Add(grouped_letters);
                        grouped_letters = " ";
                    }

                    grouped_letters += new_letter;
                }

                if (grouped_letters.Length > 1)
                {
                    output.Add(grouped_letters);
                }

                foreach (string line in output)
                {
                    ConsoleGUI.WriteLine(line, 1);
                }

                ConsoleGUI.WriteLine("<hr>", 1);
                ConsoleGUI.WriteLine("Word Analysis", 1);
                ConsoleGUI.WriteLine("<hr>", 1);
                ConsoleGUI.WriteLine("Top 10 Frequent Words", 1);

                int wordCountWidth = 0;
                if (words.Count > 0)
                {
                    wordCountWidth = words.Take(10).Max(w => w.Value.ToString().Length);
                }

                foreach (var word in words.Take(10))
                {
                    ConsoleGUI.WriteLine($"  {word.Value.ToString().PadRight(wordCountWidth)} x {word.Key}", 1);
                }

                ConsoleGUI.WriteLine("", 1);
                ConsoleGUI.WriteLine("Top 5 Longest Words", 1);
                foreach (var word in words.OrderByDescending(s => s.Key.Length).Take(5))
                {
                    ConsoleGUI.WriteLine($"  {word.Key}", 1);
                }
            }
        }

        /// <summary>
        /// Writes a list of longs words to a text file. Will check for file existence. If the file exists it gives the
        /// user the option to overwrite the file or not. If not, saves the file will a number appended to the filename.
        /// </summary>
        /// <param name="words">Dictionary of the words in the analysed text.</param>
        /// <param name="filename">the filename of the file to be written.</param>
        /// <returns>Returns a string representing the final filename saved.</returns>
        public static string longWordsToFile(Dictionary<string, int> words, string filename)
        {
            string fullFilename = $"./{filename}.txt";
            
            // Check file doesn't already exist.
            if (File.Exists(fullFilename))
            {
                string option = ConsoleGUI.GetValidatedUserInput("File exists - overwrite? [Y]es or [N]o:",
                    new string[] {"y", "n"}, borderLevel: 1, closingBorder: true);
                int i = 1;
                if (option == "n")
                {
                    // append numbers and check for existence until a new file can be written.
                    while (true)
                    {
                        fullFilename = $"./{filename}-{i}.txt";
                        if (!File.Exists(fullFilename))
                        {
                            break;
                        }
                        i++;
                    }
                }
            }
            List<String> lines = words.Select(s => s.Key).Where(w => w.Length >= 7).ToList();
            
            File.WriteAllLinesAsync(fullFilename, lines);
            return fullFilename;
        }
    }
}
