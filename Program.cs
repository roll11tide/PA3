using System;
using JUtils;
using System.Collections.Generic;
using System.Linq;

namespace PA_3v3
{
    class Program
    {
        static void Main(string[] args)
        {
            // The boys
            // A custom I/O controller and a data handler
            JCIOC ioController = new JCIOC();
            JDH dataHandler = new JDH();

            InitAndRun(ref ioController, ref dataHandler);
        }

        static public void InitAndRun(ref JCIOC ioController, ref JDH dataHandler)
        {
            // Add global gil variable
            int gil = 50;

            // IO Setup
            // Define menu options
            Dictionary<string, string> passMOTD = new Dictionary<string, string>();
            passMOTD.Add("1)", "Slots");
            passMOTD.Add("2)", "Dice");
            passMOTD.Add("3)", "Roulette");
            passMOTD.Add("4)", "Exit");
            passMOTD.Add("Gil:", gil.ToString());

            // Assign properties
            ioController.menuOptions = passMOTD;
            ioController.menuMessage = "--- Main Menu ---";

            // Let's get this bread
            bool running = true;

            while (running)
            {
                if (gil >= 300) {
                    Console.WriteLine("You won!");
                    ioController.AwaitKeyPress();
                    running = false;
                } else if (gil <= 0) {
                    Console.WriteLine("You lost.");
                    running = false;
                } else {
                    // Updating this key reads absolutely horribly but it works
                    ioController.menuOptions["Gil:"] = gil.ToString();
                    ioController.PrintMenu();
                    string rawInput = ioController.ReceiveRawInput(KeyOrLine: 1);
                    int? parsedInput = dataHandler.IntParse(rawInput);

                    if (parsedInput != null)
                    {
                        // DeNull. Yea.
                        int selection = dataHandler.DeNull(parsedInput);

                        switch (selection)
                        {
                            case 1:
                                // Slots
                                SlotMachine slotMachine = new SlotMachine();
                                slotMachine.Play(ref gil, ref ioController, ref dataHandler);
                                break;
                            case 2:
                                // Dice
                                Dice dice = new Dice();
                                dice.Play(ref gil, ref ioController, ref dataHandler);
                                break;
                            case 3:
                                // Roulette
                                Roulette roulette = new Roulette();
                                roulette.Play(ref gil, ref ioController, ref dataHandler);
                                break;
                            case 4:
                                // Leave the casino because the house always wins anyway
                                running = false;
                                break;
                            default:
                                // Hmmmmm
                                Console.WriteLine("Unexpected input.");
                                break;
                        }
                    }
                }
                
            }
        }
    }

    class Roulette
    {
        public Roulette()
        {

        }

        public void Play(ref int gil, ref JCIOC ioController, ref JDH dataHandler)
        {
            Console.Clear();

            // Initialize and configure menu
            JCIOC rouletteMenu = new JCIOC();
            Dictionary<string, string> passMOTD = new Dictionary<string, string>() { };
            passMOTD.Add("1)", "Red (2x)");
            passMOTD.Add("2)", "Black (2x)");
            passMOTD.Add("3)", "Green (8x)");
            passMOTD.Add("4)", "Quit");
            rouletteMenu.menuOptions = passMOTD;
            rouletteMenu.menuMessage = "Which color would you like to play?";
            rouletteMenu.consoleCharacter = ">";
            bool playing = true;

            // 1: Red, 2: Black, 3: Green, 0: Error
            int spinNumber = Spin();
            int spinColor = DetermineColor(spinNumber);

            while (playing)
            {
                Console.Clear();
                Console.WriteLine("This edition of roulette is a game where a ball is spun on a wheel, landing in one of 38 pockets. For 1-10 or 19-28, odd numbered pockets are red and the even numbered pockets are black. The opposite is true for the remaining numbers, with the exception of the two green slots which are separate from this pool of numbers. Hitting red or black will yield double the bet, while hitting green will yield eight times the bet.");
                string rawInput = ioController.ReceiveRawInput("How much gil would you like to bet? You currently have " + gil + " gil. (0 to quit)");
                int? parsedInput = dataHandler.IntParse(rawInput);

                if (parsedInput == null)
                {
                    Console.WriteLine("Unexpected input.");
                }
                else if (parsedInput == 0)
                {
                    // Quit option
                    playing = false;
                } else if (parsedInput <= 0)
                {
                    Console.WriteLine("You must enter a valid bet to play.");
                } else if (gil <= parsedInput)
                {
                    // You don't have the facilities for this big man
                    Console.WriteLine("You don't have the funds for this.");
                }
                else
                {
                    int gilBet = dataHandler.DeNull(parsedInput);

                    // TODO Write in menu option checker in JUtils
                    if (gilBet > gil || gilBet < 1)
                    {
                        Console.WriteLine("Invalid bet.");
                    }
                    else
                    {
                        rouletteMenu.PrintMenu();
                        string rawColorInput = rouletteMenu.ReceiveRawInput(KeyOrLine: 1);
                        int? parsedColorInput = dataHandler.IntParse(rawColorInput);
                        int gilWon = 0;
                        string outputColorString = "";
                        // TODO Add if/else if/else to change outputColorString here
                        switch (spinColor)
                        {
                            case 1:
                                outputColorString = "red";
                                break;
                            case 2:
                                outputColorString = "black";
                                break;
                            case 3:
                                outputColorString = "green";
                                break;
                            default:
                                outputColorString = "error";
                                break;
                        }

                        bool error = false;
                        switch (parsedColorInput)
                        {
                            case 1:
                                // Red
                                if (spinColor == 1)
                                {
                                    // x2
                                    gilWon = gilBet * 2;
                                }
                                break;
                            case 2:
                                // Black
                                if (spinColor == 2)
                                {
                                    // x2
                                    gilWon = gilBet * 2;
                                }
                                break;
                            case 3:
                                // GREEN
                                if (spinColor == 3)
                                {
                                    // x8
                                    gilWon = gilBet * 8;
                                }
                                break;
                            case 4:
                                // Quit
                                playing = false;
                                gilWon = -1;
                                break;
                            default:
                                // Wait, that's illegal
                                Console.WriteLine("Unexpected input.");
                                error = true;
                                break;
                        }

                        if (!error) {
                            Console.Clear();
                            Console.WriteLine("The wheel spins... the ball pockets " + spinNumber + ": " + outputColorString + ".");
                            if (gilWon == 0)
                            {
                                Console.WriteLine("No cigar! You have lost " + gilBet + " gil.");
                                gil -= gilBet;
                            }
                            else if (gilWon > 0)
                            {
                                Console.WriteLine("You won " + gilWon + "!");
                                gil += gilWon;
                            }
                        }
                        
                    }
                }
            }
        }

        public int Spin()
        {
            Random random = new Random();
            return random.Next(1, 39);
        }

        public int DetermineColor(int spin)
        {
            // 1: Red, 2: Black, 3: Green, 0: Error
            if ((spin >= 1 && spin <= 10) || (spin >= 19 && spin <= 28))
            {
                // Odd numbered pockets are red and the even numbered pockets are black
                // 1-10 or 19-28
                if (spin % 2 == 0)
                {
                    // Even
                    return 2; // Black
                }
                else
                {
                    // Odd
                    return 1; // Red
                }
            }
            else if ((spin >= 11 && spin <= 18) || (spin >= 29 && spin <= 36))
            {
                // Odd numbered pockets are black and the even numbered pockets are red
                // 11-18 or 29-36
                if (spin % 2 == 0)
                {
                    // Even
                    return 1; // Red
                }
                else
                {
                    // Odd
                    return 2; // Black
                }
            }
            else if (spin == 37 || spin == 38)
            {
                // GREEN!
                // 37 or 38
                return 3; // What a beautiful sight
            }
            else
            {
                Console.WriteLine("Error in determining color.");
                return 0;
            }
        }
    }

    class Dice
    {
        public Dice()
        {

        }

        public void Play(ref int gil, ref JCIOC ioController, ref JDH dataHandler)
        {
            bool playing = true;
            int totalBet = 0;
            int roll = Roll();

            while (playing)
            {
                Console.Clear();
                // Console.WriteLine("DEBUG DEBUG DEBUG The number is " + roll);
                Console.WriteLine("Guess a number between 5 and 30. Each guess costs 3 gil, and the total gil spent will be lost if not guessed correctly in 4 turns. The number does not change, and guessing correctly within four turns yields 50 gil.");
                string rawInput = ioController.ReceiveRawInput("Enter guess. A value of \"n\" forfeits the game and gil you've bet so far. \r\nYou have guessed " + totalBet / 3 + " times.");

                if (rawInput.ToLower() == "n")
                {
                    // Quit
                    gil -= totalBet;
                    playing = false;
                } else
                {
                    // Continue playing
                    Console.Clear();
                    int? parsedInput = dataHandler.IntParse(rawInput);

                    if (parsedInput == null || parsedInput > 35 || parsedInput < 5)
                    {
                        // Either not an integer or out of bounds
                        Console.WriteLine("Unexpected input.");
                    } else
                    {
                        int guess = dataHandler.DeNull(parsedInput);
                        if (guess == roll)
                        {
                            // Success
                            gil += 50;
                            playing = false;
                            Console.WriteLine("Success! You have won 50 gil.");
                        } else
                        {
                            // F A I L U R E
                            totalBet += 3;
                            if ((totalBet / 3) == 1)
                            {
                                Console.WriteLine("Try again! You have made 1 guess for a total of " + totalBet + " gil.");
                            } else if ((totalBet / 3) >= 4) // Don't know how it could be larger than four but hope for the best and plan for the worst
                            {
                                // Ran out of guesses
                                Console.WriteLine("Out of guesses! You have lost " + totalBet + " gil. The number was: " + roll + ".");
                                gil -= totalBet;
                                playing = false;
                            } else
                            {
                                Console.WriteLine("Try again! You have made " + (totalBet / 3) + " guesses for a total of " + totalBet + " gil.");
                            }
                        }
                    }
                }

                ioController.AwaitKeyPress();
            }
        }
        
        public int Roll() // Tide
        {
            int total = 0;
            Random random = new Random();

            for (int i = 0; i <= 6; i++)
            {
                total += random.Next(1, 7);
            }

            return total;
        }
    }

    class SlotMachine
    {
        string[] rollerSides = new string[] { "ELEPHANT", "COMPUTER", "FOOTBALL", "RESUME", "CAPSTONE", "CRIMSON" };

        public SlotMachine()
        {

        }

        public void Play(ref int gil, ref JCIOC ioController, ref JDH dataHandler)
        {
            bool playing = true;
            
            while (playing)
            {
                Console.Clear();
                Console.WriteLine("Six words are randomly matched up in a set of three. A pair will double and a three-of-a-kind will triple the bet awarded.");
                string rawInput = ioController.ReceiveRawInput("How much would you like to bet?\r\nYou have: " + gil + " gil.");
                int? parsedInput = dataHandler.IntParse(rawInput);
                Console.Clear();

                if (!(parsedInput == null))
                {
                    int bet = dataHandler.DeNull(parsedInput);

                    if (bet <= gil && bet > 0)
                    {
                        // Has the cash and bet is above zero
                        List<string> spins = Spin();
                        //spins = new List<string> { "FOOTBALL", "RESUME", "CAPSTONE" };
                        int multiplier = GetMultiplier(spins);
                        PrintSpins(spins);

                        if (multiplier == 1)
                        {
                            // No soup for you!
                            gil -= bet;
                            Console.WriteLine("You have lost " + bet + " gil.");
                        }
                        else
                        {
                            // Give the man a cigar!
                            gil += bet * multiplier;
                            Console.WriteLine("You have been awarded " + (bet * multiplier) + " gil.");
                        }

                        Console.WriteLine("Total gil: " + gil);
                    } else if (bet <= 0)
                    {
                        // Invalid amount
                        Console.WriteLine("You must bet a valid amount to play.");
                    }
                    else
                    {
                        // More was bet than possessed
                        Console.WriteLine("You don't have the funds for this.");
                    }
                }
                else
                {
                    Console.WriteLine("Unexpected input.");
                }
                
                rawInput = ioController.ReceiveRawInput("Play again? n/Y", 1);
                if (rawInput.ToLower() == "n")
                {
                    playing = false;
                }
            }
        }

        // Returns cash multiplier (0, 2, 3)
        public int GetMultiplier(List<string> spins)
        {
            int multiplier = 1;
            string match = "";

            if (spins[0] == spins[1])
            {
                // 0 + 1 match
                multiplier++;
                if (spins[0] == spins[2])
                {
                    // 0 + 1 + 2 match
                    multiplier++;
                }
            } else if (spins[0] == spins[2])
            {
                // 0 + 2 match, but not 1
                multiplier++;
            } else if (spins[1] == spins[2])
            {
                // 1 + 2 match, but not 0
                multiplier++;
            }

            return multiplier;
        }

        public void PrintSpins(List<string> spins)
        {
            Console.Write("(((  ");

            foreach (string x in spins)
            {
                Console.Write(x + "  ");
            }

            Console.Write(")))\r\n\r\n");
        }

        public List<string> Spin()
        {
            Random random = new Random();
            List<string> returnArray = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                returnArray.Add(rollerSides[random.Next(0, rollerSides.Count())]);
            }

            return returnArray;
        }
    }
}
