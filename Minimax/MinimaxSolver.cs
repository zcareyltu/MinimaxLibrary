using System;
using System.Collections.Generic;
using System.Text;

namespace Minimax {
	public class MinimaxSolver {

		public int LookAheadLimit { get; set; }

		public MinimaxSolver(int LookAheadLimit = 3) {
			this.LookAheadLimit = LookAheadLimit;
		}

		public T Solve<T>(MinimaxState<T> initialState) where T : class {
			if (initialState.GameFinished) throw new ArgumentException("The game has already finished!", "initialState");
			List<T> moves = initialState.GetPossiblePlayerMoves();

			int score = int.MinValue;
			T nextMove = null;
			foreach(T move in moves) {
				initialState.ApplyMove(move);
				int result = solve(initialState, 1, false);
				initialState.UndoMove(move);
				if((result > score) || (nextMove == null)) {
					score = result;
					nextMove = move;
				}
			}

			return nextMove;
		}

		private int solve<T>(MinimaxState<T> state, int currentDepth, bool findMax) where T : class {
			if((currentDepth >= LookAheadLimit) || state.GameFinished) {
				return state.GetScore();
			} else {
				List<T> moves = findMax ? state.GetPossiblePlayerMoves() : state.GetPossibleOpponentMoves();

				int score;

				if (findMax) {
					score = int.MinValue;
					foreach(T move in moves) {
						state.ApplyMove(move);
						int result = solve(state, currentDepth + 1, !findMax);
						state.UndoMove(move);
						score = Math.Max(score, result);
					}
				} else {
					score = int.MaxValue;
					foreach(T move in moves) {
						state.ApplyMove(move);
						int result = solve(state, currentDepth + 1, !findMax);
						state.UndoMove(move);
						score = Math.Min(score, result);
					}
				}

				return score;
			}
		}

	}
}
