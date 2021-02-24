using System;
using System.Collections.Generic;
using System.Text;

namespace JUtils
{
    // Entire script written by Jesse Wells
    
    /* JCIOC (J's Console IO Controller) Documentation
         *
         *   How to implement my devlishly fun IO controller:
         *   -Menu takes Dictionary<string, string> - key is number, letter, whatever + dividing character ), :, |, whatever
         *       Value is the text for the menu option. See below for helpful examples. 
         *       Exhibit A. <"1)", "first_option">
         *       Exhibit B. <"2)", "second_option">
         *   -Menu does nothing but print in dapper fashion.
         *   -Use ReceiveRawInput to have a string returned, of course with a handomse console character to boot.
         *      Can take prompt, as well as an integer than will read key if anything except 0 (default) is passed.
         *   -AwaitKeyPress... hmm... I wonder what that does. It can take a string to change default prompt.
        */

    class JCIOC
    {
        // Menu stuff
        public Dictionary<string, string> menuOptions = new Dictionary<string, string>();
        //public string[] validInputs = { };
        public string menuMessage = "--- J's Semi-Modular Menu ---";

        // Other stuff
        public string consoleCharacter = ">";
        public bool credit = true;

        // Yes, I'd rather define the properties and assign objects line by line when implementing. I'm sorry.
        public JCIOC()
        {

        }

        // Print the entire menu, sans input line
        public void PrintMenu()
        {
            Console.Clear();

            // Had to do it to 'em
            if (credit)
            {
                Console.WriteLine("Using J's Semi-Modular Menu\r\n");
            }

            // Print menu message and all options
            Console.WriteLine(menuMessage);
            foreach (var pair in menuOptions)
            {
                Console.WriteLine(pair.Key + " " + pair.Value);
            }
        }

        // Print input line, return raw input (as string) from line or key press
        public string ReceiveRawInput(string prompt = "", int KeyOrLine = 0)
        {
            // Optional prompt
            if (!(prompt == ""))
            {
                Console.WriteLine(prompt);
            }

            Console.Write(consoleCharacter + " ");

            if (KeyOrLine == 0)
            {
                // 0 returns a line
                return Console.ReadLine();
            }
            else
            {
                // anything else returns a key press
                return Console.ReadKey().KeyChar.ToString();
            }
        }

        // Can be used to "pause" a synchronous task
        public void AwaitKeyPress(string prompt = "Press any key to conintue...")
        {
            // Optional prompt change
            Console.Write(prompt);
            Console.ReadKey();
        }
    }

    // JDH (J's Data Handler) Documentation
    /* IntParse returns a nullable int, which is null if not parsable
     *  Part of the reason I did this was so that implementations could check if the input actually parsed or not
     * DeNull can be used to make int? an int - I'm not proud of the name I'm not very creative
     * DoubleParse returns a double, or NaN if not parsable
     */
    class JDH
    {
        public JDH()
        {

        }

        // Return null if non-parsable
        public int? IntParse(string input)
        {
            int result;

            if (Int32.TryParse(input, out result))
            {
                int? nullableInt = result;
                return nullableInt;
            }
            else
            {
                return null;
            }
        }

        // Return NaN if non-parsable
        public double DoubleParse(string input)
        {
            double result;

            if (Double.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                return Double.NaN;
            }
        }

        public int DeNull(int? input)
        {
            if (input == null)
            {
                return new int();
            }
            else
            {
                return (int)input;
            }
        }
    }
}
