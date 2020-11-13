using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4 {
	public partial class Form1 : Form {

		private GameState state = new GameState();
		private Button[,] buttons = new Button[7,6];
		private Dictionary<Button, Point> points = new Dictionary<Button, Point>();

		public Form1() {
			InitializeComponent();
			GenerateButtons();
		}

		private void GenerateButtons() {
			for(int y = 0; y < 6; y++) {
				for(int x = 0; x < 7; x++) {
					Button btn = new Button();
					btn.Text = "";
					btn.Size = new Size(40, 40);
					btn.Location = new Point(x * 46, y * 46);
					btn.MouseClick += ButtonClicked;
					btn.Parent = ButtonHolder;
					buttons[x, y] = btn;
					points[btn] = new Point(x, y);
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
			
		}

		private void ColorAiButton(int x, int y) {
			buttons[x, y].BackColor = Color.Yellow;
		}

		private void ButtonClicked(object sender, MouseEventArgs args) {
			Point p = points[(Button)sender];
			if (state.TryHumanMove(p.X, p.Y)) {
				((Button)sender).BackColor = Color.Red;
				state.AiTurn(ColorAiButton);
			}
		}
	}
}
