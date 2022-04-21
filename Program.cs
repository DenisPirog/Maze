using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Program
    {
        private static void Main(string[] args)
        {          
            GameLoop();           
        }

        private static void GameLoop()
        {
            Vector2 playerPosition = new Vector2(67, 10);
            Vector2 finishPosition = new Vector2(3, 10);

            string[] maze =
            {
                "███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████",
                "██                                ██            ██          ██      ██            ██                  ██          ██        ██   ██",
                "██  ██    ██████  ████      ████  ██  ████  ██  ██  ██████  ██████  ██  ████  ██  ██  ██████  ██  ██  ██  ██████████  ██    ██   ██",
                "██  ████  ██  ██  ██████  ████        ██    ██████  ██  ██          ██  ██    ██  ██  ██      ██  ██  ██    ██    ██  ████  ███████",
                "██    ██  ██  ██      ██  ██    ████████  ████      ██  ██  ██  ██      ██  ████  ██      ██  ██  ██  ████  ████      ██         ██",
                "██  ████  ██  ████  ████  ██████████  ██  ████  ██████  ██  ██████  ██  ██    ██████  ██████  ██████          ██  ██████  ████   ██",
                "██    ██████    ██        ██                      ██    ██  ██      ██  ████              ██    ██    ██  ██████      ██    ██   ██",
                "██    ██  ████  ████████      ██████████  ██  ██  ████          ██  ██    ██████████  ██  ████      ████  ██      ██  ████  ██   ██",
                "████████    ██        ██████████      ██  ██  ██    ██  ██████████  ████      ██      ██    ██████  ████  ████  ████             ██",
                "██    ████  ████████              ██      ██    ██      ██  ████      ██████████  ██  ████  ██  ██████      ██  ████  ██    ███████",
                "██ F              ██  ████████  ████  ██  ████    ████████                        ██  ████          ██  ██            ████  ██   ██",
                "██    ████████  ████████    ██  ██  ████    ████  ██  ██    ████      ██████  ██  ██    ██  ██████      ████  ██████  ████  ██   ██",
                "████████████    ██        ████  ██      ██                ██  ████  ████  ██  ██  ████  ██      ██  ██  ████            ██  ██   ██",
                "██        ██  ██    ████  ████████  ██  ██  ████████████  ██  ██    ████      ██    ██████████  ██  ██    ██  ████████  ██████   ██",
                "██████  ████  ██  ██████      ██    ██  ██████    ██  ██  ██  ██    ██████  ██████  ██      ██  ██  ████  ██████  ██    ██       ██",
                "██            ██  ████    ████████  ██  ██            ██  ██  ████  ████    ██  ██  ██  ██  ██  ██    ██  ██      ██    ████████████",
                "████  ██████████  ██    ████        ██      ████  ██  ██  ██                ██  ██████  ██  ██  ████      ██  ██  ████  ██     ████",
                "██    ██          ██  ████    ████████  ██    ██  ██  ██████████████    ██████  ██      ██  ████████  ██  ██  ██        ██       ██",
                "██  ████  ██████████  ████  ████████    ████  ██████  ████    ████████████████  ██  ██████  ██        ██  ██████  ██  ████  ██   ██",
                "██                          ██            ██                                        ██          ████████      ██  ██        ███████",
                "███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████",
            };

            OpenStartMenu();
            if (IsGameStarted())
            {
                while (!IsEndGame(playerPosition, finishPosition))
                {
                    UpdateField(playerPosition, finishPosition, maze);
                    playerPosition = Logic(Input(), playerPosition, maze);
                    Console.Clear();
                }
                EndGame();
            }                          
        }

        private static void OpenStartMenu()
        {
            Console.SetWindowSize(140, 30);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Press ENTER to start!");
        }

        private static bool IsGameStarted()
        {
            var inputValue = Console.ReadKey();

            if (inputValue.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                return true;
            }

            return false;
        }

        private static bool IsEndGame(Vector2 playerPosition, Vector2 finishPosition)
            => playerPosition == finishPosition;

        private static string[] UpdateField(Vector2 playerPosition, Vector2 finishPosition, string[] maze)
        {           
            int fieldWidth = maze[0].Length;
            int fieldHeight = maze.Length;

            char[,] field = new char[fieldWidth, fieldHeight];

            for (int i = 0; i < fieldHeight; i++)
            {
                for (int j = 0; j < fieldWidth; j++)
                {
                    string line = maze[i];
                    field[j, i] = line[j];
                }
            }
            
            field[playerPosition.X, playerPosition.Y] = '@';
            field[finishPosition.X, finishPosition.Y] = 'F';

            DrawField(field, fieldWidth, fieldHeight);
            return maze;
        }

        private static void DrawField(char[,] field, int fieldWidth, int fieldHeight)
        {
            for (int j = 0; j < fieldHeight; j++)
            {
                for (int i = 0; i < fieldWidth; i++)
                {
                    Console.Write(field[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static Vector2 Logic(Vector2 inputResult, Vector2 playerPosition, string[] maze)
        {
            Vector2 newPlayerPosition = new Vector2(playerPosition.X + inputResult.X, playerPosition.Y + inputResult.Y);
            return TryMove(newPlayerPosition, playerPosition, maze);
        }

        private static Vector2 Input()
        {
            Vector2 inputResult = new Vector2();
            var inputValue = Console.ReadKey();

            switch (inputValue.Key)
            {
                case ConsoleKey.W:
                    inputResult.Y = -1;
                    break;
                case ConsoleKey.A:
                    inputResult.X = -1;
                    break;
                case ConsoleKey.S:
                    inputResult.Y = 1;
                    break;
                case ConsoleKey.D:
                    inputResult.X = 1;
                    break;
            }

            return inputResult;
        }  

        private static Vector2 TryMove(Vector2 newPlayerPosition, Vector2 playerPosition, string[] maze)
        {
            if (IsWalkable(newPlayerPosition, maze))
            {
                return newPlayerPosition;
            }
            return playerPosition;      
        }

        private static bool IsWalkable(Vector2 newPlayerPosition, string[] maze)
        {
            char[] line = maze[newPlayerPosition.Y].ToCharArray();
            return line[newPlayerPosition.X].ToString() != "█";
        }

        private static void EndGame()
        {
            Console.Clear();
            Console.WriteLine("You won!");
        }
    }       
}

struct Vector2
{
    public int X;
    public int Y;

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Vector2 vector1, Vector2 vector2)
    {
        return vector1.X == vector2.X && vector1.Y == vector2.Y;
    }

    public static bool operator !=(Vector2 vector1, Vector2 vector2)
    {
        return vector1.X != vector2.X && vector1.Y != vector2.Y;
    }
}