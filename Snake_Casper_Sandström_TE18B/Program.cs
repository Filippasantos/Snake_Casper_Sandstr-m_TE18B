using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake_Casper_Sandström_TE18B
{
    class Program
    {
           //Hejsan git
        //Spelet är ganska laggigt men jag vet inte hur jag ska fixa det då jag inte vet nått sätt att ändra på text i konsollen utan att skriva ut all text igen fast ändrad
        //Detta gör att spelet inte är så optimerat men funkar hyfsat på små boards, däremot så att det skapat så att man kan göra ett board hur stort som helst

        static ConsoleKeyInfo result;
        static int xPositionChanger = 1;
        static int yPositionChanger = 0;
        static bool gameOver = false;

        public static void KeyMapping()// Den thread som hela tiden körs och tar upp vad man trycker på och ändrar riktningskoeficienten
        {
            while (!(gameOver))
            {

                result = Console.ReadKey();

                if ((result.KeyChar == 'W') || (result.KeyChar == 'w'))
                {
                    yPositionChanger = -1;
                    xPositionChanger = 0;

                }
                else if ((result.KeyChar == 'S') || (result.KeyChar == 's'))
                {
                    yPositionChanger = 1;
                    xPositionChanger = 0;

                }
                else if ((result.KeyChar == 'A') || (result.KeyChar == 'a'))
                {
                    xPositionChanger = -1;
                    yPositionChanger = 0;
                }
                else if ((result.KeyChar == 'D') || (result.KeyChar == 'd'))
                {
                    xPositionChanger = 1;
                    yPositionChanger = 0;
                }
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                xPositionChanger = 1;
                yPositionChanger = 0;
                gameOver = false;
                ThreadStart childref = new ThreadStart(KeyMapping);// säger att threaden ska starta metoden KeyMapping
                Thread childThread = new Thread(childref);// Skapar en thread med en thread start
                Console.ReadKey();
                childThread.Start();// Startar Threaden

                int points = 0;

                int round = 0;

                Random generator = new Random();

                List<int> xPosLog = new List<int>();
                List<int> yPosLog = new List<int>();

                List<int> xPos = new List<int> { 5, 4, 3 }; //Skapar x och y pos för ormen på en orm med 3 "bitar"
                List<int> yPos = new List<int> { 5, 5, 5 };
                int applePosY = 7; //sätter ut det första äpplet man ska hämta
                int applePosX = 5;
                string[,] board = new string[13, 13];//Skapar en array för boardet
                for (int y = 0; y < board.GetLength(1); y++)//Ger boardet det som krävs för att det ska bli ett visuellt board genom att sätta ut en massa Mellanrum
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        board[x, y] = "  ";
                    }
                }

                while (true)//Game state loopen
                {
                    Thread.Sleep(200);//Gör så att spelet går i en viss hastighet
                    Console.Clear();

                    for (int posLog = 0; posLog < xPos.ToArray().Length; posLog++)//Loggar alla nuvarande positioner för ormen 
                    {
                        xPosLog.Add(xPos[posLog]);
                        yPosLog.Add(yPos[posLog]);
                    }

                    for (int pos = 1; pos < xPos.ToArray().Length; pos++) //sätter de nya positionerna för allt utom huvudet på ormen genom att ta den loggade positionen för kropsdelen precis innan
                    {
                        xPos[pos] = xPosLog[pos - 1];
                        yPos[pos] = yPosLog[pos - 1];
                    }
                    xPosLog.Clear();//Rensar logs så att de går att använda igen
                    yPosLog.Clear();
                    yPos[0] += yPositionChanger;//Ser till att huvudet går i den riktiningen man har anget med knapparna
                    xPos[0] += xPositionChanger;

                    for (int y = 0; y < board.GetLength(1); y++)//Den loop som ritar ut ett board
                    {
                        Console.WriteLine();
                        for (int x = 0; x < board.GetLength(1); x++)
                        {

                            if (y % 2 == 0)//Skapar rutnätsmönstret som finns i spelet
                            {
                                if (x % 2 == 0)
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                }
                            }
                            else
                            {
                                if (x % 2 == 0)
                                {
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                }
                            }//Slut på rutmönstret
                            if (y == applePosY && x == applePosX)//Ritar ut vart äpplet är på boardet
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                            }
                            for (int behindParts = 0; behindParts < xPos.ToArray().Length; behindParts++)//Ritar ut hela ormen
                            {
                                if (y == yPos[behindParts] && x == xPos[behindParts])
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                }
                            }
                            Console.Write(board[x, y]);//Det som faktiskt ritar ut allt (Det andra ändrar bara färgerna)
                            Console.ResetColor();

                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Points: " + points);//Skriver ut hur mycket points man har i spelet

                    if (xPos[0] == applePosX && yPos[0] == applePosY)//Kollar om ormens huvud är på samma ställe som äpplet och ger en poäng samt gör ormen ett "steg" längre och genererar ett nytt äpple
                    {
                        applePosY = generator.Next(0, board.GetLength(1));
                        applePosX = generator.Next(0, board.GetLength(1));
                        xPos.Add(100);
                        yPos.Add(100);
                        points++;
                    }
                    if (round > 2)//Lite onödig men gör så att man inte dör inom de två första Game state rundorna
                    {
                        for (int Hit = 1; Hit < xPos.ToArray().Length; Hit++)//Kollar om ormens huvud slår i nån annan del av ormen och sätter gameover till true då
                        {
                            if (xPos[0] == xPos[Hit] && yPos[0] == yPos[Hit])
                            {
                                gameOver = true;
                                break;
                            }
                        }
                    }
                    

                    if (xPos[0] > board.GetLength(1) - 1 || yPos[0] > board.GetLength(1) - 1 || xPos[0] < 0 || yPos[0] < 0)//Gör så att man förlorar om man åker in i kanten av brädet
                    {
                        gameOver = true;
                    }

                    if (gameOver)//Gör så att Gamestate loopen slutar
                    {
                        break;
                    }
                    round++;

                }
                Thread.Sleep(600);
                Console.Clear();
                Console.WriteLine("Game over!");
                Console.WriteLine("You got " + points + " points! Good job!");
                Console.ReadLine();//Skriver ut hur många poäng man fick
            }
        }
    }
}
