
namespace CMP1903M_Assessment_1_Base_Code
{
    /// <summary>
    /// Handles all input from the user via the console.
    /// </summary>
    public class Input
    {
        /// <value>
        /// The <c>text</c> property represents the manual text input via the user.
        /// Other classes can access the value, but only this class can set it.
        /// Example of ENCAPSULATION.
        /// </value>
        private string Text { get; set; } = "";

        /// <summary>
        /// Prompts the user to enter text. Will accept multiple lines of text.
        /// Tells the user to enter a double semi-colons when complete.
        /// </summary>
        /// <returns>String containing the all the text the user entered.</returns>
        public string ManualTextInput()
        {
            Text = "";

            ConsoleGui.WriteLine("");
            ConsoleGui.WriteLine("Enter your text. You can use the enter key for new lines. When finished, " +
                "enter double semi-colons (;;) at the end of a line. ");

            ConsoleGui.WriteLine("Note that any double semi-colons in the middle of a sentence will be corrected " +
                "to one semi-colon for you. Double spaces will also be corrected.");
            ConsoleGui.SetBorder(2);
            ConsoleGui.WriteLine("<borderTop>");

            while (true)
            {
                string input = ConsoleGui.ReadLine("");
                input = input.Replace("  ", " ");

                // Check and replace double semi-colons in middle of input.
                if (input.Length > 2 && input.Trim().Remove(input.Length - 2, 2).Contains(";;"))
                {
                    input = input.Replace(";;", "; ");
                    ConsoleGui.ReplaceLine(input);
                }

                // Check and replace double spaces in input.
                if (input.Length > 2 && input.Contains("  "))
                {
                    input = input.Replace("  ", " ");
                    ConsoleGui.ReplaceLine(input);
                }

                if (input.Trim().EndsWith(";;"))
                {
                    input = input.Remove(input.Length - 2, 2).Trim();
                    
                    if (input.Length > 0)
                    {
                        Text += input;
                        ConsoleGui.WriteLine("<borderBottom>");
                    }
                    else
                    {
                        ConsoleGui.ReplaceLine("<borderBottom>");
                    }
                    ConsoleGui.SetBorder(1);
                    return Text;
                }
                else
                {
                    Text += $"{input}\n";
                }
            }
        }

        /// <summary>
        /// Prompts the user for a filepath and then loads the contents of te file.
        /// </summary>
        /// <returns>String representation of the file contents.</returns>
        public string FileTextInput()
        {
            ConsoleGui.WriteLine("Please enter the absolute filepath for your text file, " +
                                 "or [Q] to quit and [B] to go back.");
            ConsoleGui.SetBorder(2);
            ConsoleGui.WriteLine("<borderTop>");
            while (true)
            {
                string input = ConsoleGui.ReadLine("");

                if (input.ToLower() == "b")
                {
                    ConsoleGui.WriteLine("<borderBottom>");
                    ConsoleGui.SetBorder(1);
                    return "<mainMenu>";
                } else if (input.ToLower() == "q")
                {
                    ConsoleGui.WriteLine("<borderBottom>");
                    ConsoleGui.SetBorder(1);
                    return "<quit>";
                }

                // Check for file existence.
                if (!File.Exists(input))
                {
                    ConsoleGui.WriteLine("\u001b[31mFile doesn't seem to exist. Please try again.\u001b[0m");
                    continue;
                }
                ConsoleGui.WriteLine("<borderBottom>");
                
                Text = File.ReadAllText(input);
                ConsoleGui.SetBorder(1);
                return Text;
            }
        }

    }
}
