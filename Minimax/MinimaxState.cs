using System;
using System.Collections.Generic;
using System.Text;

namespace Minimax {
	public interface MinimaxState<T> where T : class {

		bool GameFinished { get; }

		/// <summary>
		/// Scores >0 favor the player, scores <0 favor the opponent
		/// </summary>
		/// <returns></returns>
		int GetScore();

		List<T> GetPossiblePlayerMoves();
		List<T> GetPossibleOpponentMoves();

		void ApplyMove(T move);

		void UndoMove(T move);

	}
}
