using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] players = { 'X', 'O' };

            char playAgain = 'Y';
            while (playAgain == 'Y' || playAgain == 'y')
            {
                char[,] gameState = new char[3, 3] {
                       {' ', ' ', ' '} ,
                       {' ', ' ', ' '} ,
                       {' ', ' ', ' '} };

                string currentState = "Not Done";
                int opponentChoice;
                bool opponentValidChoiceflag;
                OpponentType Opponent;

                do
                {
                    Console.Write("\nNew Game. Select an opponent type");
                    Console.Write("\n1. Dumb AI (has no idea what he's doing)");
                    Console.Write("\n2. Smart AI (you won't stand a chance)");
                    Console.Write("\nYour Choice: ");
                    try
                    {
                        opponentChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    catch(Exception ex)
                    {
                        Console.Write(ex.Message);
                        throw ex;
                    }

                    opponentValidChoiceflag = (opponentChoice == 1 || opponentChoice == 2);

                    if (!opponentValidChoiceflag)
                    {
                        Console.Write("\nInvalid Choice! Try Again.");
                    }

                } while (!opponentValidChoiceflag);

                Opponent = opponentChoice == 1 ? OpponentType.Dumb : OpponentType.Smart;

                PrintBoard(gameState);

                char playerChoice;
                bool playerValidChoiceflag;

                do
                {
                    Console.Write("\nChoose which player goes first - X (You - the human) or O (The AI): ");
                    try
                    {
                        playerChoice = char.ToLower(Console.ReadLine()[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        throw ex;
                    }
                    playerValidChoiceflag = (playerChoice == 'x' || playerChoice == 'o');
                    if (!playerValidChoiceflag)
                    {
                        Console.Write("\nInvalid Choice! Try Again.");
                    }
                } while (!playerValidChoiceflag);

                char winner = ' ';
                int currentPlayerIndex;

                if (playerChoice == 'X' || playerChoice == 'x')
                    currentPlayerIndex = 0;
                else
                    currentPlayerIndex = 1;

                while (currentState == "Not Done")
                {
                    if (currentPlayerIndex == 0)
                    {
                        Console.Write("\nHuman's turn! Choose where to place (1 to 9): ");
                        int blockChoice = Convert.ToInt32(Console.ReadLine());
                        PlayMove(gameState, players[currentPlayerIndex], blockChoice);
                    }
                    else
                    {
                        int blockChoice;
                        if (Opponent is OpponentType.Smart)
                            blockChoice = getBestMove(gameState, players[currentPlayerIndex], -1).Item2;
                        else
                            blockChoice = getRandomMove(gameState);
                        PlayMove(gameState, players[currentPlayerIndex], blockChoice);
                        Console.WriteLine("\nAI plays move: {0}", blockChoice);
                    }

                    PrintBoard(gameState);

                    CheckCurrentState(gameState, out currentState, out winner);

                    if (winner != ' ')
                    {
                        Console.WriteLine("\nPlayer {0} won!", winner);
                    }
                    else
                    {
                        currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                    }

                    if (currentState == "Draw")
                    {
                        Console.WriteLine("\nGame was a Draw!");
                    }
                }

                Console.WriteLine("\nTry Again? (Y/N)");
                playAgain = Console.ReadLine()[0];
                Console.Clear();

            }

        }

        static void PrintBoard(char[,] board)
        {

            Console.WriteLine("\n----------------");
            Console.WriteLine("| " + board[0, 0] + " || " + board[0, 1] + " || " + board[0, 2] + " |");
            Console.WriteLine("----------------");
            Console.WriteLine("| " + board[1, 0] + " || " + board[1, 1] + " || " + board[1, 2] + " |");
            Console.WriteLine("----------------");
            Console.WriteLine("| " + board[2, 0] + " || " + board[2, 1] + " || " + board[2, 2] + " |");
        }
        static void PlayMove(char[,] gameState, char player, int blockChoice)
        {
            if (gameState[(int)(blockChoice - 1) / 3, (int)(blockChoice - 1) % 3] == ' ')
            {
                gameState[(int)(blockChoice - 1) / 3, (int)(blockChoice - 1) % 3] = player;
            }
            else
            {
                Console.Write("Block is not empty! Choose again: ");
                blockChoice = Convert.ToInt32(Console.ReadLine());
                PlayMove(gameState, player, blockChoice);
            }
        }
        static void CheckCurrentState(char[,] gameState, out string currentState, out char winner)
        {
            // Check horizontals
            for (int i = 0; i < 3; i++)
            {
                if (gameState[i, 0] == gameState[i, 1] && gameState[i, 1] == gameState[i, 2] && gameState[i, 0] != ' ')
                {
                    winner = gameState[i, 0];
                    currentState = "Done";
                    return;
                }
            }

            // Check verticals
            for (int i = 0; i < 3; i++)
            {
                if (gameState[0, i] == gameState[1, i] && gameState[1, i] == gameState[2, i] && gameState[0, i] != ' ')
                {
                    winner = gameState[0, i];
                    currentState = "Done";
                    return;
                }
            }

            // Check diagonals
            if (gameState[0, 0] == gameState[1, 1] && gameState[1, 1] == gameState[2, 2] && gameState[0, 0] != ' ')
            {
                winner = gameState[1, 1];
                currentState = "Done";
                return;
            }
            if (gameState[2, 0] == gameState[1, 1] && gameState[1, 1] == gameState[0, 2] && gameState[2, 0] != ' ')
            {
                winner = gameState[1, 1];
                currentState = "Done";
                return;
            }

            // Check for draw
            bool draw_flag = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameState[i, j] == ' ')
                        draw_flag = true;
                }
            }
            if (!draw_flag)
            {
                winner = ' ';
                currentState = "Draw";
                return;
            }

            winner = ' ';
            currentState = "Not Done";
            return;
        }

        static int getRandomMove(char[,] gameState)
        {
            int move = -1;
            Random rand = new Random();
            bool validMove = false;
            do
            {
                move = rand.Next(1, 10);

                if (gameState[(int)(move - 1) / 3, (int)(move - 1) % 3] == ' ')
                    validMove = true;

            } while (!validMove);

            return move;
        }

        static Tuple<int, int> getBestMove(char[,] gameState, char player, int prevMove)
        {
            char winnerLoser = ' ';
            string currentState = " ";
            CheckCurrentState(gameState, out currentState, out winnerLoser);

            if (currentState == "Done" && (winnerLoser == 'O' || winnerLoser == 'o'))
                return new Tuple<int, int>(1, prevMove);
            else if (currentState == "Done" && (winnerLoser == 'X' || winnerLoser == 'x'))
                return new Tuple<int, int>(-1, prevMove);
            else if (currentState == "Draw")
                return new Tuple<int, int>(0, prevMove);

            List<Dictionary<string, int>> moves = new List<Dictionary<string, int>>();
            List<int> emptyCells = new List<int>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameState[i, j] == ' ')
                    {
                        emptyCells.Add(i * 3 + (j + 1));
                    }
                }
            }

            foreach (var emptyCell in emptyCells)
            {
                Dictionary<string, int> move = new Dictionary<string, int>();
                move["index"] = emptyCell;
                char[,] newState = CopyGameState(gameState);
                PlayMove(newState, player, emptyCell);

                Tuple<int, int> result;
                if (player == 'O' || player == 'o')
                {
                    result = getBestMove(newState, 'X', emptyCell);
                }
                else
                {
                    result = getBestMove(newState, 'O', emptyCell);
                }
                move["score"] = result.Item1;
                moves.Add(move);
            }

            // Find Best Move
            int bestMove = 0;
            int bestScore = 0;
            if (player == 'O' || player == 'o')
            {
                int max = -2;
                foreach (var move in moves)
                {
                    if (move["score"] > max)
                    {
                        max = move["score"];
                        bestMove = move["index"];
                        bestScore = max;
                    }
                }
            }
            else
            {
                int max = 2;
                foreach (var move in moves)
                {
                    if (move["score"] < max)
                    {
                        max = move["score"];
                        bestMove = move["index"];
                        bestScore = max;
                    }
                }
            }
            return new Tuple<int, int>(bestScore, bestMove);
        }

        private static char[,] CopyGameState(char[,] gameState)
        {
            char[,] newState = new char[3, 3] {
                       {' ', ' ', ' '} , {' ', ' ', ' '} , {' ', ' ', ' '} };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    newState[i, j] = gameState[i, j];
                }
            }
            return newState;
        }
    }
    public enum OpponentType
    {
        Dumb,
        Smart
    }
}