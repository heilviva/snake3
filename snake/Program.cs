namespace snake
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    namespace snake
    {
        public enum MaxBorder
        {
            MaxRight = 79,
            MaxBottom = 23
        }

        public class SnakeGame
        {
            private List<Position> snake;
            private Position direction;
            private Position food;
            private int score;
            private bool isGameOver;
            private Random random;

            public SnakeGame()
            {
                snake = new List<Position>();
                direction = new Position(1, 0);
                food = GenerateFood();
                score = 0;
                isGameOver = false;
                random = new Random();

                Console.Title = "Snake Game";
                Console.SetWindowSize((int)MaxBorder.MaxRight + 1, (int)MaxBorder.MaxBottom + 1);
                Console.CursorVisible = false;

                snake.Add(new Position(0, 0)); // Initial position of the snake
                snake.Add(new Position(1, 0));
                snake.Add(new Position(2, 0));
            }

            public void Run()
            {
                Thread snakeThread = new Thread(MoveSnake);
                snakeThread.Start();

                while (!isGameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        ChangeDirection(key.Key);
                    }
                }
            }

            private void MoveSnake()
            {
                while (!isGameOver)
                {
                    Move();
                    Draw();

                    if (IsGameOver())
                    {
                        isGameOver = true;
                        Console.Clear();
                        Console.SetCursorPosition((int)MaxBorder.MaxRight / 2 - 4, (int)MaxBorder.MaxBottom / 2);
                        Console.WriteLine("Game Over!");
                        Console.SetCursorPosition((int)MaxBorder.MaxRight / 2 - 4, (int)MaxBorder.MaxBottom / 2 + 1);
                        Console.WriteLine("Score: " + score);
                    }

                    Thread.Sleep(200);
                }
            }

            private void Move()
            {
                Position head = snake[0];
                Position newHead = new Position(head.X + direction.X, head.Y + direction.Y);
                snake.Insert(0, newHead);

                if (newHead.X == food.X && newHead.Y == food.Y)
                {
                    score++;
                    food = GenerateFood();
                }
                else
                {
                    snake.RemoveAt(snake.Count - 1);
                }
            }

            private void Draw()
            {
                Console.Clear();

                // Draw snake
                foreach (Position position in snake)
                {
                    Console.SetCursorPosition(position.X, position.Y);
                    Console.Write("*");
                }

                // Draw food
                Console.SetCursorPosition(food.X, food.Y);
                Console.Write("#");

                // Draw score
                Console.SetCursorPosition((int)MaxBorder.MaxRight - 10, 0);
                Console.Write("Score: " + score);
            }

            private void ChangeDirection(ConsoleKey key)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction.Y != 1)
                        {
                            direction = new Position(0, -1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction.Y != -1)
                        {
                            direction = new Position(0, 1);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction.X != 1)
                        {
                            direction = new Position(-1, 0);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction.X != -1)
                        {
                            direction = new Position(1, 0);
                        }
                        break;
                }
            }

            private Position GenerateFood()
            {
                int x = random.Next(0, (int)MaxBorder.MaxRight + 1);
                int y = random.Next(0, (int)MaxBorder.MaxBottom + 1);

                return new Position(x, y);
            }

            private bool IsGameOver()
            {
                Position head = snake[0];

                if (head.X < 0 || head.X >(int) MaxBorder.MaxRight || head.Y < 0 || head.Y > (int)MaxBorder.MaxBottom)
                {
                    return true;
                }

                for (int i = 1; i < snake.Count; i++)
                {
                    if (head.X == snake[i].X && head.Y == snake[i].Y)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                SnakeGame snakeGame = new SnakeGame();
                snakeGame.Run();
            }
        }
    }
}