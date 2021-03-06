using System;

namespace graphNodeTest
{
    class Program
    {
        string[] vertexes = new string[5];
        string[,] vertexLinks = new string[6, 2];

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

                //Try parsing the user choice into an int
                if (Int32.TryParse(answer, out result))
                {
                    //...and check if it is a valid choice
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
                "\n 4) Delete a station" +
                "\n 5) Delete a station connection" +
                "\n 0) Terminate program");
        }

        /// <summary>
        /// Call to switch on the user menu choice
        /// </summary>
        /// <param name="selection">The user-selected number</param>
        void SwitchOnAnswer(int selection)
        {
            switch (selection)
            {
                case 1: //Create a new vertex
                    CreateVertexSequence();

                    PrintSeparatorLines();
                    break;
                case 2: //Create a new edge between vertexes
                    CreateVertexEdgeSequence();

                    PrintSeparatorLines();
                    break;
                case 3: //Display the edges between vertexes
                    PrintSeparatorLines();

                    DisplayVertexLinking();

                    PrintSeparatorLines();
                    break;
                case 4: //Prompt the user to delete a vertex
                    DeleteVertex();

                    PrintSeparatorLines();
                    break;
                case 5: //Prompt the user to delete a vertex edge
                    DeleteVertexEdge();

                    PrintSeparatorLines();
                    break;
                case 0: //Terminate program
                    Environment.Exit(1);
                    break;
                default: //When none of the above is selected
                    PrintColoredMessage(ConsoleColor.Red, "Please select a valid option.");
                    break;
            }
        }

        #region NODE_CREATION
        /// <summary>
        /// Call to ask the user to provide a new node name and check if it exists inside the nodes array.
        /// If it doesn't exist then add it to the nodes array.
        /// </summary>
        void CreateVertexSequence()
        {
            //Prompt for node name
            string givenName;
            do
            {
                Console.Write("Station name: ");
                givenName = Console.ReadLine();

            } while (SearchVertexName(givenName));

            //Create and add node to array
            AddVertexToArray(givenName);
        }

        /// <summary>
        /// Call to create a new node with the given name to the next empty array position
        /// <para>Displays a warning if the array is full</para>
        /// </summary>
        /// <param name="vertexName">The new node name</param>
        void AddVertexToArray(string vertexName)
        {
            string simplifiedName = vertexName.Replace(" ", "").ToLower();
            bool spaceFound = false;

            for (int i = 0; i < vertexes.Length; i++)
            {
                if (vertexes[i] == null)
                {
                    vertexes[i] = simplifiedName;

                    spaceFound = true;

                    PrintColoredMessage(ConsoleColor.Green, $"Station {vertexName} created");
                    break;
                }
            }

            if (!spaceFound)
            {
                //Colour the text RED
                PrintColoredMessage(ConsoleColor.Red, "Station list is full");
            }
        }
        #endregion

        #region NODE_CONNECTING
        /// <summary>
        /// Call to prompt the user to input a source node and a toBeConnected node then validate if they exist.
        /// <para>If yes, then add them to the nodeConnections array.</para>
        /// </summary>
        void CreateVertexEdgeSequence()
        {
            string vertexName, edgeName;
            bool connectionFound = false;
            bool spaceFound = false;

            Console.WriteLine("Connect which station ?");
            vertexName = Console.ReadLine();

            Console.WriteLine($"Connect {vertexName} to ...?");
            edgeName = Console.ReadLine();

            if (SearchVertexName(vertexName, false) && SearchVertexName(edgeName, false))
            {
                for (int i = 0; i < vertexLinks.GetLength(0); i++)
                {
                    if ((vertexLinks[i, 0] == vertexName && vertexLinks[i, 1] == edgeName)
                        || (vertexLinks[i, 0] == edgeName && vertexLinks[i, 1] == vertexName))
                    {
                        PrintColoredMessage(ConsoleColor.Red, "Connection already exists!");

                        connectionFound = true;
                        break;
                    }
                }

                //Early exit in case a similar connection already exists
                if (connectionFound) return;

                //Search for an empty space in the vertexes array
                for (int i = 0; i < vertexLinks.GetLength(0); i++)
                {
                    //If the i position is empty - then both positions are empty...
                    if (vertexLinks[i, 0] == null)
                    {
                        //...set the new connection entry
                        vertexLinks[i, 0] = vertexName;
                        vertexLinks[i, 1] = edgeName;

                        spaceFound = true;
                        connectionFound = false;

                        PrintColoredMessage(ConsoleColor.Green, $"Connected {vertexName} with {edgeName}");

                        break;
                    }
                }

                //Display a message if a space was not found and a similar connection does not exist
                if (!spaceFound && !connectionFound)
                {
                    //Colour the text RED
                    PrintColoredMessage(ConsoleColor.Red, "Station connection list is full");
                }
            }
            else
            {
                PrintColoredMessage(ConsoleColor.Red, "A station was not found.");
            }
        }
        #endregion

        #region NODE_PRINTING
        /// <summary>
        /// Call to display all node connections from the nodeConnections array.
        /// </summary>
        void DisplayVertexLinking()
        {
            Console.WriteLine("|Nodes|\t|Connections|");

            for (int i = 0; i < vertexLinks.GetLength(0); i++)
            {
                for (int j = 0; j < vertexLinks.GetLength(1); j++)
                {
                    Console.Write($"{i} {vertexLinks[i, j]}\t");
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
        void DeleteVertex()
        {
            //Helper variables
            string userChoice;
            int parsedAnswer;

            //Ask the user to delete a node at least once
            do
            {
                PrintSeparatorLines();

                Console.WriteLine("Which station to delete?");
                DisplayVertexesArray(); //Display the vertex array

                //Cache and parse the user input
                userChoice = Console.ReadLine();
            } while (!Int32.TryParse(userChoice, out parsedAnswer));

            //Check if the given answer is inside the bounds of the array
            if (parsedAnswer >= 0 && parsedAnswer <= vertexes.Length)
            {
                //Cache the vertex name before it gets deleted so we can use it 
                //to search the edges array for the connections
                string cachedVertexName = vertexes[parsedAnswer];

                //Nullify the given index position inside the vertexes array
                DeleteVertexFromArray(vertexes, parsedAnswer);

                SearchEdgeNameAndDelete(cachedVertexName, vertexLinks);

                //Color text yellow
                PrintColoredMessage(ConsoleColor.Yellow, $"Deleted station from entry position number : {userChoice}");
            }
            else
            {
                //Color text red
                PrintColoredMessage(ConsoleColor.Red, "Given number is out of bounds.");
            }
        }

        /// <summary>
        /// Call to prompt the user to delete a node connection from inside the nodeConnections array.
        /// <para>If it exists both array indexes get nullified.</para>
        /// </summary>
        void DeleteVertexEdge()
        {
            //Helper variables
            string userChoice;
            int parsedAnswer;

            //Ask the user to delete a connection at least once
            do
            {
                PrintSeparatorLines();

                Console.WriteLine("Which connection to delete?");
                DisplayVertexLinking(); //Display the vertex edges array

                //Cache and parse the user input
                userChoice = Console.ReadLine();
            } while (!Int32.TryParse(userChoice, out parsedAnswer));

            //Check if the given answer is inside the bounds of the array
            if (parsedAnswer >= 0 && parsedAnswer <= vertexLinks.GetLength(0))
            {
                //Clear both array entries
                DeleteVertexFromArray(vertexLinks, parsedAnswer);

                //Color text yellow
                PrintColoredMessage(ConsoleColor.Yellow, $"Deleted connection from entry position number : {userChoice}");
            }
            else
            {
                //Color text red
                PrintColoredMessage(ConsoleColor.Red, "Given number is out of bounds.");
            }
        }

        /// <summary>
        /// Call to set the given index inside the given array to NULL
        /// </summary>
        /// <param name="vertArray">The array to modify.</param>
        /// <param name="index">The array index to set to null.</param>
        void DeleteVertexFromArray(string[] vertArray, int index)
        {
            vertArray[index] = null;
        }

        /// <summary>
        /// Call to set the [index,0] and [index,1] array positions to NULL.
        /// </summary>
        /// <param name="vertArray">The 2D array to modify</param>
        /// <param name="index">The array index to set to null.</param>
        void DeleteVertexFromArray(string[,] vertArray, int index)
        {
            for (int i = 0; i < vertArray.GetLength(1); i++)
            {
                vertArray[index, i] = null;
            }
        }
        #endregion

        #region UTILITIES
        /// <summary>
        /// Call to write all the contents of the vertexes array into the console
        /// </summary>
        void DisplayVertexesArray()
        {
            for (int i = 0; i < vertexes.Length; i++)
            {
                Console.WriteLine($"Station {i}: {vertexes[i]}");
            }
        }

        /// <summary>
        /// Call to search the vertexes array for the given name
        /// </summary>
        /// <param name="name">Name to search for.</param>
        /// <param name="printMsg">Change to false to not print the "Name already exists" message.</param>
        /// <returns>True if name is found, false otherwise</returns>
        bool SearchVertexName(string name, bool printMsg = true)
        {
            string simplifiedName = name.Replace(" ", "").ToLower();

            for (int i = 0; i < vertexes.Length; i++)
            {
                if (simplifiedName == vertexes[i])
                {
                    if (printMsg)
                    {
                        PrintColoredMessage(ConsoleColor.Cyan, "Name already exists");
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Call to cycle through the given array and set BOTH entries of the given name to null
        /// </summary>
        /// <param name="name">The name to delete.</param>
        /// <param name="edgesArray">The array to search for the name in.</param>
        void SearchEdgeNameAndDelete(string name, string[,] edgesArray)
        {
            for (int i = 0; i < edgesArray.GetLength(0); i++)
            {
                //If the current index is null then continue to the next one
                if (edgesArray[i, 0] == null) continue;

                //If the i index matches the given name then delete both array entries
                if (edgesArray[i, 0] == name || edgesArray[i, 1] == name)
                {
                    DeleteVertexFromArray(edgesArray, i);

                    PrintColoredMessage(ConsoleColor.Red, $"Deleted connection {edgesArray[i, 0]}-{edgesArray[i, 1]} " +
                        $"from the connection list.");
                }
            }
        }

        /// <summary>
        /// Call to WriteLine the message in the console with the given color,
        /// <para>color is set back to white after the message gets printed.</para>
        /// </summary>
        /// <param name="cColor">Color of the given sentence</param>
        /// <param name="message">The message to display</param>
        void PrintColoredMessage(ConsoleColor cColor, string message)
        {
            Console.ForegroundColor = cColor;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
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
