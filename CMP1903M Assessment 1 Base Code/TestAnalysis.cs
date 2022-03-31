namespace CMP1903M_Assessment_1_Base_Code;

/// <summary>
/// Checks known text against <c>Analyse</c> class.
/// </summary>
public static class TestAnalysis
{
    /// <summary>
    /// Validates a known file analysed by <c>Analyse.AnalyseText()</c> and writes the results to the console.
    /// </summary>
    /// <returns>Returns a boolean representing whether the analysis matches the known values.</returns>
    public static bool ValidateTestFile(string filename)
    {
        string testText = "";
        try
        {
            testText = File.ReadAllText("known_test_file.txt");
        }
        catch (FileNotFoundException)
        {
            ConsoleGui.WriteLine("Can't find the known test file :(");
            return false;
        }
        
        Dictionary<string, int> knownValues = new Dictionary<string, int>
        {
            {"words", 98},
            {"unique words", 64},
            {"sentences", 6},
            {"characters", 615},
            {"vowels", 189},
            {"consonants", 317},
            {"uppercase letters", 9},
            {"lowercase letters", 497},
            {"numbers", 0},
            {"number characters", 0},
            {"spaces", 98}
        };

        Analyse analysis = new Analyse();

        Dictionary<string, int> testAnalysis = analysis.AnalyseText(testText);
        bool testSuccess = true;
        
        int maxMetricLength = knownValues.Max(x => x.Key.Length);
        
        // Compare the known values with the analysed text.
        foreach (var metric in knownValues)
        {
            string testResult = $"  Number of {metric.Key.PadRight(maxMetricLength)} : {testAnalysis[metric.Key].ToString().PadLeft(3)} == " +
                                $"{metric.Value.ToString(),-3} : ";

            if (testAnalysis[metric.Key] == metric.Value)
            {
                testResult += "PASS";
            }
            else
            {
                testResult += "FAIL";
                testSuccess = false;
            }
            
            ConsoleGui.WriteLine(testResult);
        }
        ConsoleGui.WriteLine("");

        if (testSuccess)
        {
            return true;
        }
        return false;
    }
}