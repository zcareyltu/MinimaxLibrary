using Minimax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Connect4 {
	//Red, player one, human player, positive
	//Yellow, player two, ai, negative
	public class GameState : MinimaxState<GameMove> {

		private int[,] board = new int[7, 6];

		public bool GameFinished { get; private set; } = false;
		public bool HumanWon = false;

		private MinimaxSolver minimax = new MinimaxSolver(3);

		private void DeclareWinner() {
			int win = checkForWin();
			if (win == 1) {
				MessageBox.Show("You won!");
				Application.Exit();
			} else if(win == -1) {
				MessageBox.Show("You lost!");
				Application.Exit();
			}
		}

		public bool TryHumanMove(int x, int y) {
			if (board[x, y] == 0) {
				if((y == 5) || (board[x, y + 1] != 0)) {
					board[x, y] = 1;
					DeclareWinner();
					return true;
				}
			}

			return false;
		}

		public void AiTurn(Action<int, int> ui) {
			GameMove bestMove = minimax.Solve(this);
			ApplyMove(bestMove);
			ui(bestMove.X, bestMove.Y);
			DeclareWinner();
		}

		private List<GameMove> GetPossibleMoves(int color) {
			List<GameMove> moves = new List<GameMove>();
			for (int x = 0; x < 7; x++) {
				for (int y = 5; y >= 0; y--) {
					if (board[x, y] == 0) {
						moves.Add(new GameMove(color, x, y));
						break;
					}
				}
			}
			return moves;
		}

		public List<GameMove> GetPossiblePlayerMoves() {
			return GetPossibleMoves(-1);
		}

		public List<GameMove> GetPossibleOpponentMoves() {
			return GetPossibleMoves(1);
		}

		public int GetScore() {
			if (GameFinished) {
				if (HumanWon) return int.MinValue; //We dont want those hairless apes to win!
				else return int.MaxValue; //Best possible scenario!
			} else {
				return calculateScore();
			}
		}

		public void ApplyMove(GameMove move) {
			board[move.X, move.Y] = move.Color;
			int win = checkForWin();
			if(win != 0) {
				GameFinished = true;
				HumanWon = (win > 0);
			}
		}

		public void UndoMove(GameMove move) {
			board[move.X, move.Y] = 0;
			GameFinished = false;
		}

		private int calculateScore() {
			int score = 0;
			for(int y = 0; y < 6; y++) {
				for(int x = 0; x < 7; x++) {
					score += horzScore(x, y) + vertScore(x, y) + diagScore(x, y);
				}
			}
			return -score; //AI is color=-1, but should be scored in a positive manner
		}

		private int horzScore(int x, int y) {
			int color = board[x, y];
			if (color == 0) return 0;
			if(x < 4) {
				x++;
				int score = 1;
				for(int i = 1; i < 4; i++, x++) {
					if(board[x, y] == color) {
						score++;
					} else {
						return score;
					}
				}
			}

			return 0;
		}

		private int vertScore(int x, int y) {
			int color = board[x, y];
			if (color == 0) return 0;
			if(y < 3) {
				y++;
				int score = 1;
				for(int i = 1; i < 4; i++, y++) {
					if(board[x, y] == color) {
						score++;
					} else {
						return score;
					}
				}
			}

			return 0;
		}

		private int diagScore(int x, int y) {
			int color = board[x, y];
			if (color == 0) return 0;
			if (y < 3) {
				//y++;
				int score = 1;
				if(x < 4) {
					int x2 = x + 1;
					int y2 = y + 1;
					for (int i = 1; i < 4; i++, x2++, y2++) {
						if (board[x2, y2] == color) {
							score++;
						} else {
							break;
						}
					}
				}

				if(x > 2) {
					int x2 = x - 1;
					int y2 = y + 1;
					for (int i = 1; i < 4; i++, x2--, y2++) {
						if (board[x2, y2] == color) {
							score++;
						} else {
							break;
						}
					}
				}

				return score;
			}

			return 0;
		}

		//positive human won, 0 no winner, negative ai won
		private int checkForWin() {
			int win = 0;
			for(int y = 0; y < 6; y++) {
				for(int x = 0; x < 7; x++) {
					win = HorizontalWin(x, y);
					if (win != 0) return win;

					win = VerticalWin(x, y);
					if (win != 0) return win;

					win = DiagonalWin(x, y);
					if (win != 0) return win;
				}
			}

			return 0;
		}

		private int HorizontalWin(int x, int y) {
			if(x < 4) {
				int color = board[x, y];
				if (color == 0) return 0;
				x++;
				for(int i = 1; i < 4; i++, x++) {
					if(board[x, y] != color) {
						return 0;
					}
				}
				return color;
			}

			return 0;
		}

		private int VerticalWin(int x, int y) {
			if (y < 3) {
				int color = board[x, y];
				if (color == 0) return 0;
				y++;
				for (int i = 1; i < 4; i++, y++) {
					if (board[x, y] != color) {
						return 0;
					}
				}
				return color;
			}

			return 0;
		}

		private int DiagonalWin(int x, int y) {
			if(y < 3) {
				if(x < 4) {
					int color = board[x, y];
					if (color == 0) return 0;
					int x2 = x + 1;
					int y2 = y + 1;
					bool won = true;
					for(int i = 1; i < 4; i++, x2++, y2++) {
						if(board[x2, y2] != color) {
							won = false;
							break;
						}
					}
					if (won) return color;
				}

				if(x > 2) {
					int color = board[x, y];
					if (color == 0) return 0;
					int x2 = x - 1;
					int y2 = y + 1;
					bool won = true;
					for (int i = 1; i < 4; i++, x2--, y2++) {
						if (board[x2, y2] != color) {
							won = false;
							break;
						}
					}
					if (won) return color;
				}
			}

			return 0;
		}
	}
}
