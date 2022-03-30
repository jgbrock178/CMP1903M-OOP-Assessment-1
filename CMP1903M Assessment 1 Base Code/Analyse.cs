
using System.Text.RegularExpressions;

namespace CMP1903M_Assessment_1_Base_Code
{
    /// <summary>
    /// Methods to perform various text analysis.
    /// </summary>
    public class Analyse
    {
        /// <summary>
        /// Returns a dictionary of metrics about the input text. Includes sentences, vowels, consonants, 
        /// uppercase letters and lowercase letters.
        /// </summary>
        /// <param name="input">the text to be analysed.</param>
        /// <returns>Dictionary containing the names and values of each metric.</returns>
        public Dictionary<string, int> AnalyseText(string input)
        {
            Dictionary<string, int> values = new Dictionary<string, int>
            {
                {"words", CountWords(input)},
                {"unique words", WordList(input).Count},
                {"sentences", CountSentenceTerminators(input)},
                {"characters", CountCharacters(input)},
                {"vowels", CountVowels(input)},
                {"consonants", CountConsonants(input)},
                {"spaces", CountSpaces(input)},
                {"uppercase letters", CountUppercaseLetters(input)},
                {"lowercase letters", CountLowercaseLetters(input)},
                {"numbers", CountNumbers(input)},
                {"number characters", CountNumberCharacters(input)}
            };

            return values;
        }

        /// <summary>
        /// Returns a dictionary containing letters and their frequencies in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns></returns>
        public Dictionary<string, int> LetterFrequency(string text)
        {
            Dictionary<string, int> letterFrequency = new Dictionary<string, int>();
            
            var query = Regex.Replace(text, "[^a-zA-Z]", "")
                .ToList().GroupBy(l => l.ToString().ToLower()).OrderByDescending(l => l.Count());

            foreach (var letter in query)
            {
                letterFrequency.Add(letter.Key, letter.Count());
            }
            return letterFrequency;
        }

        /// <summary>
        /// Analyses and returns a list of all words within the input text. Matches letters and underscores. Any other
        /// characters are excluded.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Distinct list of all words.</returns>
        public Dictionary<string, int> WordList(string text)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();

            Regex rx = new Regex(@"\b[a-z'-]+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matchedPatterns = rx.Matches(text);
            var wordList = matchedPatterns
                .GroupBy(w => w.ToString().ToLower())
                .OrderByDescending(w => w.Count());

            foreach (var word in wordList)
            {
                words.Add(word.Key, word.Count());
            }

            return words;
        }

        /// <summary>
        /// Counts the number of words in the input text. Note: underscores are treated like spaces.
        /// </summary>
        /// <param name="text">the input text to be analysed.</param>
        /// <returns>An integer representing the number of words.</returns>
        int CountWords(string text)
        {
            return CountRegexMatches(text, @"\b[A-Z'-]+\b");
        }
        
        /// <summary>
        /// Counts the number of characters in the input text.
        /// </summary>
        /// <param name="text">the input text to be analysed.</param>
        /// <returns>An integer representing the number of characters.</returns>
        int CountCharacters(string text)
        {
            return CountRegexMatches(text, @".");
        }

        /// <summary>
        /// Counts the number of uppercase letters in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of uppercase letters in the input text.</returns>
        int CountUppercaseLetters(string text)
        {
            return CountRegexMatches(text, @"[A-Z]", true);
        }

        /// <summary>
        /// Counts the number of lowercase letters in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of lowercase letters in the input text.</returns>
        int CountLowercaseLetters(string text)
        {
            return CountRegexMatches(text, @"[a-z]", true);
        }
        
        /// <summary>
        /// Counts the number of consonants in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of consonants in the input text.</returns>
        int CountConsonants(string text)
        {
            return CountRegexMatches(text, @"(?![aeiou])[a-z]");
        }
        
        /// <summary>
        /// Counts the number of vowels in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of vowels in the input text.</returns>
        int CountVowels(string text)
        {
            return CountRegexMatches(text, @"[aeiou]");
        }
        
        /// <summary>
        /// Counts the number of full stops ".", exclamation marks "!", ellipses "…" and question marks "?"
        /// in the input.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the total number of terminators.</returns>
        int CountSentenceTerminators(string text)
        {
            return CountRegexMatches(text, @"([\.\?\!…] |[\.\?\!…][\r|\n]|[\.\?\!…]$)");
        }

        /// <summary>
        /// Counts the number of complete numbers in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of numbers.</returns>
        int CountNumbers(string text)
        {
            return CountRegexMatches(text, @"\b[0-9]+\b");
        }
        
        /// <summary>
        /// Counts the number of number characters in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of number characters.</returns>
        int CountNumberCharacters(string text)
        {
            return CountRegexMatches(text, @"[0-9]");
        }

        /// <summary>
        /// Counts the number of spaces in the input text.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <returns>Integer representing the number of spaces.</returns>
        int CountSpaces(string text)
        {
            return CountRegexMatches(text, @"[ ]");
        }


        /// <summary>
        /// Performs regex using the input pattern to count the number of occurrences matching the pattern.
        /// </summary>
        /// <param name="text">the text to be analysed.</param>
        /// <param name="pattern">the regex pattern to be used for the analysis.</param>
        /// <param name="caseSensitive">Whether the analysis should be case sensitive.</param>
        /// <returns></returns>
        private int CountRegexMatches(string text, string pattern, bool caseSensitive = false)
        {
            Regex rx = caseSensitive
                ? new Regex(pattern, RegexOptions.Compiled)
                : new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matchedPatterns = rx.Matches(text);
            return matchedPatterns.Count;
        }
    }
}
