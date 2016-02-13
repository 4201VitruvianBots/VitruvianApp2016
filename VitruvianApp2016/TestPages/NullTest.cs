using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class NullTest:ContentPage
	{

		int[] scoreValue = new int[8];
		ParseObject data = new ParseObject ("MatchData");

		int points = 0;
		bool scaled = false;
		bool challenge = false;
		Label score;

		public NullTest ()
		{
			for (int i = 0; i < 8; i++)
				scoreValue [i] = 0;

			score = new Label () {
				Text = points.ToString ()
			};

			Button scoreBtn = new Button () {
				Text = "Score"
			};
			scoreBtn.Clicked += (object sender, EventArgs e) => {
				UpdatePoints();
			};

			Button pointBtn = new Button () {
				Text = "Add Point"
			};
			scoreBtn.Clicked += (object sender, EventArgs e) => {
				scoreValue[0]++;
				UpdatePoints();
			};

			SaveData ();

			this.Content = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					score,
					scoreBtn,
					pointBtn,
				}
			};
		}

		void UpdatePoints(){
			int A = 0;
			int B = 0;
			int C = 0;
			int D = 0;
			int E = 0;
			try{
				A += Convert.ToInt16(data["autoA1"].ToString());
			}catch{}
			try{
				A += Convert.ToInt16(data["autoA2"].ToString());
			} catch{}
			if (A + scoreValue [0] < 2)
				points += 10*A+5*scoreValue[0];
			try{
				B += Convert.ToInt16(data["autoB1"].ToString());
			}catch{}
			try{
				B += Convert.ToInt16(data["autoB2"].ToString());
			} catch{}
			if (B + scoreValue [1] < 2)
				points += 10*B+5*scoreValue[1];
			try{
				C += Convert.ToInt16(data["autoC1"].ToString());
			}catch{}
			try{
				C += Convert.ToInt16(data["autoC2"].ToString());
			} catch{}
			if (C + scoreValue [2] < 2)
				points += 10*C+5*scoreValue[2];
			try{
				D += Convert.ToInt16(data["autoD1"].ToString());
			}catch{}
			try{
				D += Convert.ToInt16(data["autoD2"].ToString());
			} catch{}
			if (D + scoreValue [3] < 2)
				points += 10*D+5*scoreValue[3];
			try{
				E += Convert.ToInt16(data["autoE"].ToString());
			}catch{}
			if (E + scoreValue [4] < 2)
				points += 10*E+5*scoreValue[4];

			//points += 5 * Convert.ToInt16(data ["teleOpShotHighSuccess"]) + 2 * scoreValue[5];
			//points += 10 * Convert.ToInt16(data ["autoShotHighSuccess"]) + 5 * scoreValue[7];
			if (scaled == true)
				points += 15;
			else if (challenge == true)
				points += 5;

			score.Text = points.ToString();
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}

