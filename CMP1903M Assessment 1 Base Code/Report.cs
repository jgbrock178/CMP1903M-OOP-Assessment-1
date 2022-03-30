
namespace CMP1903M_Assessment_1_Base_Code
{
    /// <summary>
    /// Formats text analysis and reports it in various formats.
    /// </summary>
    public static class Report
    {
        /// <summary>
        /// Outputs a report of the analysis to the console.
        /// </summary>
        /// <param name="metrics">dictionary containing each metric and the count.</param>
        /// <param name="letterAnalysis">dictionary containing analysis of letters and their frequencies.</param>
        /// <param name="words">A dictionary of words and their frequencies.</param>
        public static void OutputToConsole(Dictionary<string, int> metrics, Dictionary<string, int> letterAnalysis, 
            Dictionary<string, int> words)
        {
            int maxMetricLength = metrics.Max(x => x.Key.Length);
            
            ConsoleGui.WriteLine("<hr>");
            ConsoleGui.WriteLine("Text Analysis Report");
            ConsoleGui.WriteLine("<hr>");

            foreach (var metric in metrics)
            {
                ConsoleGui.WriteLine($"  Number of {metric.Key.PadRight(maxMetricLength)} : {metric.Value}");
            }
            
            ConsoleGui.WriteLine("<hr>");
            ConsoleGui.WriteLine("Letter Analysis");
            ConsoleGui.WriteLine("<hr>");

            if (words.Count == 0)
            {
                ConsoleGui.WriteLine("No letters or words in input.");
            }
            else
            {
                ConsoleGui.WriteLine("Top 5 Letters");

                int lastLetterCount = 0;
                int i = 1;
                List<string> output = new List<string>();

                foreach (var letter in letterAnalysis.Take(5))
                {
                    output.Add($"  {i}. {letter.Key.ToUpper()} x {letter.Value}");
                    lastLetterCount = letter.Value;
                    i++;
                }

                string groupedLetters = " ";
                int maxLetterCount = 0;
                if (letterAnalysis.Count > 0)
                {
                    maxLetterCount = letterAnalysis.Max(x => x.Value.ToString().Length);
                }

                bool titleAdded = false;

                foreach (var letter in letterAnalysis.Reverse().Take(letterAnalysis.Count - 5).Reverse())
                {
                    string newLetter;
                    if (lastLetterCount == letter.Value)
                    {
                        newLetter = $", {letter.Key.ToUpper()} x {letter.Value}";
                        if ((output[^1] + newLetter).Length > ConsoleGui.ConsoleWidth - 12)
                        {
                            output.Add($"     {letter.Key.ToUpper()} x {letter.Value}");
                        }
                        else
                        {
                            output[^1] = $"{output[^1]}{newLetter}";
                        }

                        continue;
                    }
                    else if (!titleAdded)
                    {
                        output.Add("");
                        output.Add("Other Letters...");
                        titleAdded = true;
                    }

                    newLetter = $" {letter.Key.ToUpper()} x {letter.Value.ToString().PadRight(maxLetterCount)} ";
                    if ((groupedLetters + newLetter).Length > ConsoleGui.ConsoleWidth - 12)
                    {
                        output.Add(groupedLetters);
                        groupedLetters = " ";
                    }

                    groupedLetters += newLetter;
                }

                if (groupedLetters.Length > 1)
                {
                    output.Add(groupedLetters);
                }

                foreach (string line in output)
                {
                    ConsoleGui.WriteLine(line);
                }

                ConsoleGui.WriteLine("<hr>");
                ConsoleGui.WriteLine("Word Analysis");
                ConsoleGui.WriteLine("<hr>");
                ConsoleGui.WriteLine("Top 10 Frequent Words");

                int wordCountWidth = 0;
                if (words.Count > 0)
                {
                    wordCountWidth = words.Take(10).Max(w => w.Value.ToString().Length);
                }

                foreach (var word in words.Take(10))
                {
                    ConsoleGui.WriteLine($"  {word.Value.ToString().PadRight(wordCountWidth)} x {word.Key}");
                }

                ConsoleGui.WriteLine("");
                ConsoleGui.WriteLine("Top 5 Longest Words");
                foreach (var word in words.OrderByDescending(s => s.Key.Length).Take(5))
                {
                    ConsoleGui.WriteLine($"  {word.Key}");
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
        public static string LongWordsToFile(Dictionary<string, int> words, string filename)
        {
            string fullFilename = $"./{filename}.txt";
            
            // Check file doesn't already exist.
            if (File.Exists(fullFilename))
            {
                string option = ConsoleGui.GetValidatedUserInput("File exists - overwrite? [Y]es or [N]o:",
                    new [] {"y", "n"});
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
