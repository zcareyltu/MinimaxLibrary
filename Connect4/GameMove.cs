using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4 {
	public class GameMove {

		public readonly int Color;
		public readonly int X;
		public readonly int Y;

		public GameMove(int color, int x, int y) {
			this.Color = color;
			this.X = x;
			this.Y = y;
		}

	}
}
