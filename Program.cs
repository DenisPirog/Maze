using System;

namespace Maze
{
    struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        private static string[] maze = 
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
                "██                ██  ████████  ████  ██  ████    ████████                        ██  ████          ██  ██            ████  ██   ██",
                "██    ████████  ████████    ██  ██  ████    ████  ██  ██    ████      ██████  ██  ██    ██  ██████      ████  ██████  ████  ██   ██",
                "████████████    ██        ████  ██      ██                ██  ████  ████  ██  ██  ████  ██      ██  ██  ████            ██  ██   ██",
                "██        ██  ██    ████  ████████  ██  ██  ████████████  ██  ██    ████      ██    ██████████  ██  ██    ██  ████████  ██████   ██",
                "██████  ████  ██  ██████      ██    ██  ██████    ██  ██  ██  ██    ██████  ██████  ██      ██  ██  ████  ██████  ██    ██       ██",
                "██            ██  ████    ████████  ██  ██            ██  ██  ████  ████    ██  ██  ██  ██  ██  ██    ██  ██      ██    ███████████",
                "████  ██████████  ██    ████        ██      ████  ██  ██  ██                ██  ██████  ██  ██  ████      ██  ██  ████  ██     ████",
                "██    ██          ██  ████    ████████  ██    ██  ██  ██████████████    ██████  ██      ██  ████████  ██  ██  ██        ██       ██",
                "██  ████  ██████████  ████  ████████    ████  ██████  ████    ████████████████  ██  ██████  ██        ██  ██████  ██  ████  ██   ██",
                "██                          ██            ██                                        ██          ████████      ██  ██        ███████",
                "███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████",
            };

        private static void Main(string[] args)
        {          
            GameLoop();           
        }

        private static void GameLoop()
        {
            Vector2 playerPosition = new Vector2(67, 10);
            Vector2 finishPosition = new Vector2(3, 10);

            OpenStartMenu();
            if (IsGameStarted())
            {
                while (!IsEndGame(playerPosition, finishPosition))
                {
                    UpdateField(playerPosition, finishPosition);
                    playerPosition = Logic(Input(), playerPosition);
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
            => playerPosition.X == finishPosition.X && playerPosition.Y == finishPosition.Y;

        private static void UpdateField(Vector2 playerPosition, Vector2 finishPosition)
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

        private static Vector2 Logic(Vector2 inputResult, Vector2 playerPosition)
        {
            Vector2 newPlayerPosition = new Vector2(playerPosition.X + inputResult.X, playerPosition.Y + inputResult.Y);
            return TryMove(newPlayerPosition, playerPosition);
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

        private static Vector2 TryMove(Vector2 newPlayerPosition, Vector2 playerPosition)
        {
            if (IsWalkable(newPlayerPosition))
            {
                return newPlayerPosition;
            }
            return playerPosition;      
        }

        private static bool IsWalkable(Vector2 newPlayerPosition)
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