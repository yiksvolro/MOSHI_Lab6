using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab6
{
    class Program
    {
        static char[,] board = new char[3, 3];
        static char currentPlayer = 'X';

        static void Main(string[] args)
        {
            
            InitializeBoard();

            while (!IsGameOver())
            {
                DrawBoard();
                GetPlayerInput();

                if (!IsGameOver())
                {
                    ChangePlayer();
                    MakeComputerMove();
                    ChangePlayer();
                }
            }

            DrawBoard();
            ShowResult();
        }

        static void InitializeBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        static void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine("     0   1   2");
            Console.WriteLine("   -------------");

            for (int row = 0; row < 3; row++)
            {
                Console.Write($"{row}  |");

                for (int col = 0; col < 3; col++)
                {
                    Console.Write($" {board[row, col]} |");
                }

                Console.WriteLine("\n   -------------");
            }
        }

        static void GetPlayerInput()
        {
            string input;
            int row = -1, col = -1; // Присвоюємо початкові значення для уникнення помилки

            do
            {
                Console.Write($"Player '{currentPlayer}', enter your move (row, col): ");
                input = Console.ReadLine();
                string[] inputParts = input.Split(',');

                if (inputParts.Length != 2)
                {
                    Console.WriteLine("Invalid input format! Please enter the move in the format 'row, col'.");
                    continue;
                }

                if (!int.TryParse(inputParts[0], out row) || !int.TryParse(inputParts[1], out col))
                {
                    Console.WriteLine("Invalid input format! Please enter numeric values for row and col.");
                    continue;
                }

            } while (!IsValidMove(row, col));

            board[row, col] = currentPlayer;
        }

        static bool IsValidMove(int row, int col)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                Console.WriteLine("Invalid move! Row and column values must be between 0 and 2.");
                return false;
            }

            if (board[row, col] != ' ')
            {
                Console.WriteLine("Invalid move! The selected cell is already occupied.");
                return false;
            }

            return true;
        }

        static bool IsGameOver()
        {
            if (HasPlayerWon('X'))
            {
                return true;
            }

            if (HasPlayerWon('O'))
            {
                return true;
            }

            if (IsBoardFull())
            {
                return true;
            }

            return false;
        }

        static bool HasPlayerWon(char player)
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] == player && board[row, 1] == player && board[row, 2] == player)
                {
                    return true;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] == player && board[1, col] == player && board[2, col] == player)
                {
                    return true;
                }
            }

            // Check diagonals
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            {
                return true;
            }

            if (board[2, 0] == player && board[1, 1] == player && board[0, 2] == player)
            {
                return true;
            }

            return false;
        }

        static bool IsBoardFull()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static void ShowResult()
        {
            if (HasPlayerWon('X'))
            {
                Console.WriteLine("Player 'X' wins!");
            }
            else if (HasPlayerWon('O'))
            {
                Console.WriteLine("Player 'O' wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }

        static void ChangePlayer()
        {
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }

        static void MakeComputerMove()
        {
            int bestScore = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = currentPlayer;
                        int score = MiniMax(board, 0, false);
                        board[row, col] = ' ';

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }

            board[bestRow, bestCol] = currentPlayer;
        }

        static int MiniMax(char[,] board, int depth, bool isMaximizingPlayer)
        {
            if (HasPlayerWon('X'))
            {
                return -1;
            }

            if (HasPlayerWon('O'))
            {
                return 1;
            }

            if (IsBoardFull())
            {
                return 0;
            }

            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == ' ')
                        {
                            board[row, col] = 'O';
                            int score = MiniMax(board, depth + 1, false);
                            board[row, col] = ' ';

                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (board[row, col] == ' ')
                        {
                            board[row, col] = 'X';
                            int score = MiniMax(board, depth + 1, true);
                            board[row, col] = ' ';

                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }

                return bestScore;
            }
        }
    }
}
