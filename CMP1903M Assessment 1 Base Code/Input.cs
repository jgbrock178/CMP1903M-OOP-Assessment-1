using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903M_Assessment_1_Base_Code
{
    /// <summary>
    /// Handles all input from the user via the console.
    /// </summary>
    public class Input
    {
        /// <value>
        /// The <c>text</c> property represents the manual text input via the user.
        /// </value>
        public string Text { get; private set; } = "";

        /// <summary>
        /// Prompts the user to enter text. Will accept multiple lines of text.
        /// Tells the user to enter a double semi-colons when complete.
        /// </summary>
        /// <returns>String containing the all the text the user entered.</returns>
        public string ManualTextInput()
        {
            Text = "";
            bool terminateInput = false;
            bool correctedText = false;
            
            ConsoleGUI.WriteLine("",1);
            ConsoleGUI.WriteLine("Enter your text. You can use the enter key for new lines. When finished, " +
                "enter double semi-colons (;;) at the end of a line. Note: Any double semi-colons in " +
                "the middle of a sentence will be corrected to one semi-colon for you.",
                1);
            ConsoleGUI.WriteLine("<borderTop>", 2, false);

            while (true)
            {
                string input = ConsoleGUI.ReadLine("", 2);

                if (input.Length > 2 && input.Trim().Remove(input.Length - 2, 2).Contains(";;"))
                {
                    // Replaces double semi-colons in the middle of input.
                    input = input.Replace(";;", "; ");
                    ConsoleGUI.ReplaceLine(input, 2);
                }

                if (input.Trim().EndsWith(";;"))
                {
                    input = input.Remove(input.Length - 2, 2).Trim();
                    
                    if (input.Length > 0)
                    {
                        Text += input;
                        ConsoleGUI.WriteLine("<borderBottom>", 2);
                    }
                    else
                    {
                        ConsoleGUI.ReplaceLine("<borderBottom>", 2);
                    }
                    return Text;
                }
                else
                {
                    Text += $"{input}\n";
                }
            }
        }

        //Method: fileTextInput
        //Arguments: string (the file path)
        //Returns: string
        //Gets text input from a .txt file
        public string fileTextInput()
        {
            ConsoleGUI.WriteLine("Please enter the absolute filepath for your text file, or [Q] to quit and [B] to go back.", 1);
            ConsoleGUI.WriteLine("<borderTop>", 2);
            while (true)
            {
                string input = ConsoleGUI.ReadLine("", 2);

                if (input.ToLower() == "b")
                {
                    ConsoleGUI.WriteLine("<borderBottom>", 2);
                    return "<mainmenu>";
                } else if (input.ToLower() == "q")
                {
                    ConsoleGUI.WriteLine("<borderBottom>", 2);
                    return "<quit>";
                }

                // Check for file existence.
                if (!File.Exists(input))
                {
                    ConsoleGUI.WriteLine("\u001b[31mFile doesn't seem to exist. Please try again.\u001b[0m", 2);
                    continue;
                }
                ConsoleGUI.WriteLine("<borderBottom>", 2);
                
                Text = File.ReadAllText(input);
                return Text;
            }
        }

    }
}
