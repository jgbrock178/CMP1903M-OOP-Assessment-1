namespace CMP1903M_Assessment_1_Base_Code;

/// <summary>
/// Checks known text against <c>Analyse</c> class.
/// </summary>
public static class TestAnalysis
{
    static string TestText = "Object Oriented programming is a programming paradigm that relies on the concept of classes and objects." +
        "A class is an abstract blueprint used to create more specific, concrete objects. Classes often represent broad categories, " +
        "like Car or Dog that share attributes. These classes define what attributes an instance of this type will have, like colour," +
        " but not the value of those attributes for a specific object. Classes can also contain functions, called methods available only" +
        " to objects of that type. These functions are defined within the class and perform some action helpful to that specific type of object.";

    /// <summary>
    /// Validates a known file analysed by <c>Analyse.AnalyseText()</c> and writes the results to the console.
    /// </summary>
    /// <returns>Returns a boolean representing whether the analysis matches the known values.</returns>
    public static bool ValidateTestFile()
    {
        Dictionary<string, int> knownValues = new Dictionary<string, int>
        {
            {"words", 98},
            {"unique words", 64},
            {"sentences", 5},
            {"characters", 613},
            {"vowels", 189},
            {"consonants", 317},
            {"uppercase letters", 9},
            {"lowercase letters", 497},
            {"numbers", 0},
            {"number characters", 0},
            {"spaces", 96}
        };

        Analyse analysis = new Analyse();

        Dictionary<string, int> testAnalysis = analysis.AnalyseText(TestText);
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