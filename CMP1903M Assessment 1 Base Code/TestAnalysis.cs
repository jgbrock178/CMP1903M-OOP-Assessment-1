namespace CMP1903M_Assessment_1_Base_Code;

public class TestAnalysis
{
    /// <summary>
    /// Validates a known file analysed by <c>Analyse.AnalyseText()</c> and writes the results to the console.
    /// </summary>
    /// <returns>Returns a boolean representing whether the analysis matches the known values.</returns>
    public static bool ValidateTestFile()
    {
        string text = File.ReadAllText("./known_test_file.txt");
        Dictionary<string, int> knownValues = new Dictionary<string, int>();
        
        knownValues.Add("words", 98);
        knownValues.Add("unique words", 64);
        knownValues.Add("sentences", 6);
        knownValues.Add("characters", 615);
        knownValues.Add("vowels", 189);
        knownValues.Add("consonants", 317);
        knownValues.Add("uppercase letters", 9);
        knownValues.Add("lowercase letters", 497);
        knownValues.Add("numbers", 0);
        knownValues.Add("number characters", 0);

        Analyse analysis = new Analyse();

        Dictionary<string, int> testAnalysis = analysis.AnalyseText(text);
        bool testSuccess = true;
        
        int max_metric_length = knownValues.Max(x => x.Key.Length);
        
        // Compare the known values with the analysed text.
        foreach (var metric in knownValues)
        {
            string testResult = $"  Number of {metric.Key.PadRight(max_metric_length)} : {testAnalysis[metric.Key].ToString().PadLeft(3)} == " +
                                $"{metric.Value.ToString().PadRight(3)} : ";

            if (testAnalysis[metric.Key] == metric.Value)
            {
                testResult += "PASS";
            }
            else
            {
                testResult += "FAIL";
                testSuccess = false;
            }
            
            ConsoleGUI.WriteLine(testResult, 1);
        }
        ConsoleGUI.WriteLine("",1);

        if (testSuccess)
        {
            return true;
        }
        return false;
    }
}