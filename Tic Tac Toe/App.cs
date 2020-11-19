using System;
using System.Collections.Generic;
using System.Linq;

namespace Tic_Tac_Toe
{
    class App
    {
		static void Main(string[] args)
		{
			var stillPlaying = true;


			while (stillPlaying)
			{

				PlayGame();

			}

		}



		private static void PlayGame()
		{


			var boardSize = 9;
			var board = new string[boardSize];

			var turn = "X";
			while (true)
			{
				Console.Clear();

				var winner = WhoWins(board);
				if (winner != null)
				{
					AnnounceResult(winner[0] + " WINS!!!");
					break;
				}
				if (IsBoardFull(board))
				{
					AnnounceResult("It's a tie!!!");
					break;
				}



				DrawBoard(board);

				Console.WriteLine("\n Write location of your " + turn + " element 1-9:");

				string xoLocStr = Console.ReadLine();
				int xoLoc = Int32.Parse(xoLocStr) - 1;
				board[xoLoc] = turn;

				turn = turn == "X" ? "O" : "X";
			}
		}

		private static void AnnounceResult(string message)
		{
			Console.WriteLine();



			Console.WriteLine(message);
			Console.ReadKey();


		}


		private static void DrawBoard(string[] board)
		{
			for (int i = 0; i < 9; i++)
			{
				if (i % 3 == 0) Console.WriteLine();
				if (board[i] == null) { Console.Write(i + 1); }
				else
				{ Console.Write(board[i]); }


			}


		}

		private static bool IsBoardFull(IEnumerable<string> board)
		{
			return board.All(space => space != null);
		}

		private static string WhoWins(string[] board)
		{
			var numRows = 3;

			// sprawdzanie wierszy
			for (int row = 0; row < numRows; row++)
			{
				if (board[row * numRows] != null)
				{
					bool hasTicTacToe = true;
					for (int col = 1; col < numRows && hasTicTacToe; col++)
					{
						if (board[row * numRows + col] != board[row * numRows])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{

						for (int col = 0; col < numRows; col++)
							board[row * numRows + col] += "!";
						return board[row * numRows];
					}
				}
			}

			// sprawdzanie kolumn
			for (int col = 0; col < numRows; col++)
			{
				if (board[col] != null)
				{
					bool hasTicTacToe = true;
					for (int row = 1; row < numRows && hasTicTacToe; row++)
					{
						if (board[row * numRows + col] != board[col])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{

						for (int row = 0; row < numRows; row++)
							board[row * numRows + col] += "!";
						return board[col];
					}
				}
			}

			// sprawdzanie od na skos zaczynajac od lewego gornego
			if (board[0] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + row] != board[0])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{

					for (int row = 0; row < numRows; row++)
						board[row * numRows + row] += "!";
					return board[0];
				}
			}

			// sprawdzanie skosu zaczynajc z prawego gornego
			if (board[numRows - 1] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + (numRows - 1 - row)] != board[numRows - 1])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{

					for (int row = 0; row < numRows; row++)
						board[row * numRows + (numRows - 1 - row)] += "!";
					return board[numRows - 1];
				}
			}

			return null;
		}
	}
}
