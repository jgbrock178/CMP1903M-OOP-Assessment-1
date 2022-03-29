namespace CMP1903M_Assessment_1_Base_Code;

public class TestAnalysis
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
        Dictionary<string, int> knownValues = new Dictionary<string, int>();
        
        knownValues.Add("words", 98);
        knownValues.Add("unique words", 64);
        knownValues.Add("sentences", 5);
        knownValues.Add("characters", 613);
        knownValues.Add("vowels", 189);
        knownValues.Add("consonants", 317);
        knownValues.Add("uppercase letters", 9);
        knownValues.Add("lowercase letters", 497);
        knownValues.Add("numbers", 0);
        knownValues.Add("number characters", 0);
        knownValues.Add("spaces", 96);

        Analyse analysis = new Analyse();

        Dictionary<string, int> testAnalysis = analysis.AnalyseText(TestText);
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