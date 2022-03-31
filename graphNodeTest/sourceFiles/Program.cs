using System;
using System.Collections.Generic;

namespace graphNodeTest
{
    class Program
    {
        string[] nodes = new string[5];
        string[,] nodeConnections = new string[6, 2];

        int lastNodeIndex = 0;
        int lastEmptyConnection = 0;

        static void Main(string[] args)
        {
            new Program().Execute();
        }

        /// <summary>
        /// Execute the program
        /// </summary>
        public void Execute()
        {
            string answer = "";
            int result;

            while (!answer.Equals("0"))
            {
                DisplayMenu();
                PrintSeparatorLines();

                Console.Write("Choice: ");
                answer = Console.ReadLine();

                //Parse the user choice into an int
                if (Int32.TryParse(answer, out result))
                {
                    //...and check if it is valid
                    SwitchOnAnswer(result);
                }
            }
        }

        /// <summary>
        /// Display selection menu
        /// </summary>
        void DisplayMenu()
        {
            Console.WriteLine("Please select an option" +
                "\n 1) Create station" +
                "\n 2) Create station connection" +
                "\n 3) Print all station connections" +
                "\n 5) Delete a station" +
                "\n 6) Delete a station connection" +
                "\n 0) Terminate program" +
                "\n" +
                "\nDEBUG 4) Print Nodes array");
        }

        /// <summary>
        /// Call to switch on the user menu choice
        /// </summary>
        /// <param name="selection">The user-selected number</param>
        void SwitchOnAnswer(int selection)
        {
            switch (selection)
            {
                case 1: //Create a new node
                    CreateNodeSequence();
                    PrintSeparatorLines();
                    break;
                case 2: //Create a new connection between nodes
                    CreateConnectionSequence();
                    PrintSeparatorLines();
                    break;
                case 3: //Display the connections between nodes
                    PrintSeparatorLines();
                    DisplayNodeConnections();
                    PrintSeparatorLines();
                    break;
                case 4: //DEBUG: Display the nodes array
                    DisplayNodesArray();
                    PrintSeparatorLines();
                    break;
                case 5:
                    DeleteNode();
                    PrintSeparatorLines();
                    break;
                case 6:
                    //Delete a node connection
                    break;
                case 0: //Terminate program
                    Environment.Exit(1);
                    break;
                default: //When none of the above is selected
                    Console.WriteLine("Please select a valid option.");
                    break;
            }
        }

        #region NODE_CREATION
        /// <summary>
        /// Call to ask the user to provide a new node name and check if it exists inside the nodes array.
        /// If it doesn't exist then add it to the nodes array.
        /// </summary>
        void CreateNodeSequence()
        {
            //Prompt for node name
            string givenName;
            do
            {
                Console.Write("Station name: ");
                givenName = Console.ReadLine();

            } while (SearchNodeName(givenName));

            //Create and add node to array
            AddNodeToArray(givenName);
        }

        /// <summary>
        /// Call to create a new node with the given name to the next empty array position
        /// <para>Displays a warning if the array is full</para>
        /// </summary>
        /// <param name="nodeName">The new node name</param>
        void AddNodeToArray(string nodeName)
        {
            string simplifiedName = nodeName.Replace(" ", "").ToLower();
            bool spaceFound = false;

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] == null)
                {
                    nodes[i] = simplifiedName;
                    spaceFound = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Station {nodeName} created");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
            }

            if (!spaceFound)
            {
                //Colour the text RED
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Station list is full");
                Console.ForegroundColor = ConsoleColor.White;
            }

            //Check if the nodes array is full
            /*if (lastNodeIndex >= nodes.Length)
            {
                //Colour the text RED
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Station list is full");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                //Add the new nodeName to the nodes array
                nodes[lastNodeIndex] = simplifiedName;
                lastNodeIndex++;

                //Colour the text GREEN
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Station {nodeName} created");
                Console.ForegroundColor = ConsoleColor.White;
            }*/
        }
        #endregion

        #region NODE_CONNECTING
        /// <summary>
        /// Call to prompt the user to input a source node and a toBeConnected node then validate if they exist.
        /// <para>If yes, then add them to the nodeConnections array.</para>
        /// </summary>
        void CreateConnectionSequence()
        {
            string nodeName, connectionName;

            Console.WriteLine("Connect which station ?");
            nodeName = Console.ReadLine();

            Console.WriteLine($"Connect {nodeName} to ...?");
            connectionName = Console.ReadLine();

            if (SearchNodeName(nodeName, false) && SearchNodeName(connectionName, false))
            {
                if (lastEmptyConnection >= nodeConnections.GetLength(0))
                {
                    Console.WriteLine("Station connection list is full");
                    return;
                }

                nodeConnections[lastEmptyConnection, 0] = nodeName;
                nodeConnections[lastEmptyConnection, 1] = connectionName;

                lastEmptyConnection++;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Connected {nodeName} with {connectionName}");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"Current index {lastEmptyConnection}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid station selected.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        #endregion

        #region NODE_PRINTING
        /// <summary>
        /// Call to display all node connections from the nodeConnections array.
        /// </summary>
        void DisplayNodeConnections()
        {
            Console.WriteLine("|Nodes|\t|Connections|");

            for (int i = 0; i < nodeConnections.GetLength(0); i++)
            {
                for (int j = 0; j < nodeConnections.GetLength(1); j++)
                {
                    Console.Write($"{nodeConnections[i, j]}\t");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region NODE_DELETION
        /// <summary>
        /// Call to display the nodes array and prompt the user which one to delete.
        /// <para>Marks the given array index as null</para>
        /// </summary>
        void DeleteNode()
        {
            string userChoice;
            int parsedAnswer;

            do
            {
                PrintSeparatorLines();

                Console.WriteLine("Which station to delete?");
                DisplayNodesArray();

                userChoice = Console.ReadLine();
            } while (!Int32.TryParse(userChoice, out parsedAnswer));

            if (parsedAnswer >= 0 && parsedAnswer <= nodes.Length)
            {
                nodes[parsedAnswer] = null;

                lastNodeIndex = parsedAnswer;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Given number is out of bounds.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        #endregion

        #region UTILITIES
        void DisplayNodesArray()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                Console.WriteLine($"Station {i}: {nodes[i]}");
            }
        }

        /// <summary>
        /// Call to search the nodes array for the given name
        /// </summary>
        /// <param name="name">Name to search for.</param>
        /// <param name="printMsg">Change to false to not print the "Name already exists" message.</param>
        /// <returns>True if name is found, false otherwise</returns>
        bool SearchNodeName(string name, bool printMsg = true)
        {
            string simplifiedName = name.Replace(" ", "").ToLower();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (simplifiedName == nodes[i])
                {
                    if (printMsg)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Name already exists");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Call to write 50 '-' to the console for STYLE
        /// </summary>
        void PrintSeparatorLines()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
        }
        #endregion
    }
}
