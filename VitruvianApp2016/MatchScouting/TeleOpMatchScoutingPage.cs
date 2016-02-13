using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class TeleOpMatchScoutingPage:ContentPage
	{
		ParseObject data;

		Grid layoutGrid = new Grid ();

		enum defA {Portcullis, Cheval_de_Frise};
		enum defB {Moat, Ramparts};
		enum defC {Drawbridge, Salley_Port};
		enum defD {Rock_Wall, Rough_Terrain};

		const int N = 11;
		int[] scoreValue = new int[N];
		Button[] minus = new Button[N];
		Button[] plus = new Button[N];
		Label[] displayValue = new Label[N];

		const string errorStringDefault = "The Following Data Was Unable To Be Saved: ";
		string errorString = errorStringDefault;
		bool error = false;
		bool disabled = false;
		bool challenge = false;
		bool scaled = false;
		int points = 0;

		Label scoreLabel = new Label () {
			Text = "Points: 0",
			FontSize = GlobalVariables.sizeTitle
		};

		public TeleOpMatchScoutingPage (ParseObject MatchData, int[] def)
		{
			data = MatchData;

			for(int i=0; i<N; i++){
				scoreValue [i] = 0;
				minus [i] = new Button ();
				plus [i] = new Button ();
				displayValue[i] = new Label ();
			}

			Label pageTitle = new Label () {
				Text = "TeleOp Mode",
				FontSize = GlobalVariables.sizeTitle
			};

			defense (0, 0, 1, ((defA)def[0]).ToString());
			defense (1, 3, 1, ((defB)def[1]).ToString());
			defense (2, 6, 1, ((defC)def[2]).ToString());
			defense (3, 9, 1, ((defD)def[3]).ToString());
			defense (4, 12, 1, "Low Bar");
			shoot (5, 0, 3, "Low Shot");
			shoot (7, 3, 3, "High Shot");;
			defense (10, 6, 3, "Shots Denied");

			Button challengeBtn = new Button () {
				Text = "Robot Challenge",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Gray
			};
			challengeBtn.Clicked += (object sender, EventArgs e) => {
				if(challenge==false){
					challengeBtn.BackgroundColor = Color.Red;
					challenge = true;
				} else {
					challengeBtn.BackgroundColor = Color.Gray;
					challenge = false;
				}
			};

			Button scaleBtn = new Button () {
				Text = "Robot Scaling",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Gray
			};
			scaleBtn.Clicked += (object sender, EventArgs e) => {
				if(scaled==false){
					scaleBtn.BackgroundColor = Color.Red;
					scaled = true;
				} else {
					scaleBtn.BackgroundColor = Color.Gray;
					scaled = false;
				}
			};

			// score

			Button disabledBtn = new Button () {
				Text = "Disabled",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Gray
			};
			disabledBtn.Clicked += (object sender, EventArgs e) => {
				if(disabled==false){
					disabledBtn.BackgroundColor = Color.Red;
					disabled = true;
				} else {
					disabledBtn.BackgroundColor = Color.Gray;
					disabled = false;
				}
			};

			data = MatchData;
			pointsScored ();

			Button endMatch = new Button (){
				Text = "End Match",
				BackgroundColor = Color.Red,
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeMedium
			};
			endMatch.Clicked += (object sender, EventArgs e) => {
				if(def[0] == 0)
					errorHandling("teleOpA1", Convert.ToInt32(scoreValue[0]));
				else if(def[0] == 1)
					errorHandling("teleOpA2", Convert.ToInt32(scoreValue[0]));
				if(def[1] == 0)
					errorHandling("teleOpB1", Convert.ToInt32(scoreValue[1]));
				else if(def[1] == 1)
					errorHandling("teleOpB2", Convert.ToInt32(scoreValue[1]));
				if(def[2] == 0)
					errorHandling("teleOpC1", Convert.ToInt32(scoreValue[2]));
				else if(def[2] == 1)
					errorHandling("teleOpC2", Convert.ToInt32(scoreValue[2]));
				if(def[3] == 0)
					errorHandling("teleOpD1", Convert.ToInt32(scoreValue[3]));
				else if(def[3] == 1)
					errorHandling("teleOpD2", Convert.ToInt32(scoreValue[3]));
				errorHandling("teleOpE", Convert.ToInt32(scoreValue[4]));
				errorHandling("teleOpShotLowSuccess", Convert.ToInt32(scoreValue[5]));
				errorHandling("teleOpShotLowTotal", Convert.ToInt32(scoreValue[5]+scoreValue[6]));
				errorHandling("teleOpShotHighSuccess", Convert.ToInt32(scoreValue[7]));
				errorHandling("teleOpShotHighTotal", Convert.ToInt32(scoreValue[7]+scoreValue[8]));
				if(scoreValue[7]+scoreValue[8] == 0)
					errorHandling("teleOpShotHighAccuracy", Convert.ToDouble(0));
				else
					errorHandling("teleOpShotHighAccuracy", Convert.ToDouble((double)scoreValue[7]/(double)(scoreValue[7]+scoreValue[8])));
				errorHandling("shotsDenied", scoreValue[9]);
				errorHandling("challenge", challenge);
				errorHandling("scaled", scaled);
				errorHandling("disabled", disabled);
				pointsScored();
				errorHandling("score", points);

				if(error == true){	
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
					errorString = errorStringDefault;
					error = false;
				} else {
					Navigation.PushModalAsync(new PostMatchScoutingPage(data));
				}
			};
			layoutGrid.Children.Add (pageTitle, 0, 6, 0, 1);
			layoutGrid.Children.Add (scoreLabel, 12, 15, 0, 1);
			layoutGrid.Children.Add (challengeBtn, 9, 12, 4, 5);
			layoutGrid.Children.Add (scaleBtn, 9, 12, 5, 6);
			layoutGrid.Children.Add (disabledBtn, 12, 15, 4, 5);
			layoutGrid.Children.Add (endMatch, 12, 15, 5, 6);

			this.Content = new StackLayout () {
				Children = {
					layoutGrid
				}
			};
			BackgroundColor = Color.Silver;
		}

		void defense(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			minus [arrayIndex].Text = "-";
			minus [arrayIndex].BackgroundColor = Color.Red;
			plus [arrayIndex].Text = "+";
			plus [arrayIndex].BackgroundColor = Color.Green;

			minus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if(scoreValue[arrayIndex] != 0){
					scoreValue[arrayIndex]--;
					displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
					pointsScored();
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				scoreValue[arrayIndex]++;
				displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
				pointsScored();
			};

			Label defLabel = new Label () {
				Text = title,
				FontSize = GlobalVariables.sizeTitle,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			layoutGrid.Children.Add (defLabel,x, x+3, y, y+1); // Picker 
			layoutGrid.Children.Add (minus[arrayIndex],x, y+1); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x+1, y+1); // Minus  
			layoutGrid.Children.Add (plus[arrayIndex],x+2, y+1); // Plus
		}

		void shoot(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex + 1].Text = scoreValue[arrayIndex+1].ToString();
			displayValue [arrayIndex + 1].FontSize = GlobalVariables.sizeMedium;
			minus [arrayIndex].Text = "-";
			minus [arrayIndex].BackgroundColor = Color.Red;
			minus [arrayIndex + 1].Text = "-";
			minus [arrayIndex + 1].BackgroundColor = Color.Red;
			plus [arrayIndex].Text = "+";
			plus [arrayIndex].BackgroundColor = Color.Green;
			plus [arrayIndex + 1].Text = "+";
			plus [arrayIndex + 1].BackgroundColor = Color.Green;

			minus[arrayIndex].Clicked += (object sender, EventArgs e) => {	// Hit
				if(scoreValue[arrayIndex] != 0){
					scoreValue[arrayIndex]--;							
					displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
					pointsScored();
				}
			};
			minus[arrayIndex+1].Clicked += (object sender, EventArgs e) => {	// Miss
				if(scoreValue[arrayIndex+1] != 0){
					scoreValue[arrayIndex+1]--;							
					displayValue [arrayIndex+1].Text = scoreValue[arrayIndex+1].ToString();
					pointsScored();
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {	// Hit
				scoreValue[arrayIndex]++;
				displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
				pointsScored();
			};
			plus[arrayIndex+1].Clicked += (object sender, EventArgs e) => {	// Miss
				scoreValue[arrayIndex+1]++;
				displayValue [arrayIndex+1].Text = scoreValue[arrayIndex+1].ToString();
				pointsScored();
			};

			Label titleLabel = new Label () {
				Text = title,
				FontSize = GlobalVariables.sizeTitle,
				HorizontalOptions = LayoutOptions.CenterAndExpand

			};
			layoutGrid.Children.Add (titleLabel, x, x+3, y, y+1); // title 
			layoutGrid.Children.Add (minus[arrayIndex],x, y+1); // Minus 
			layoutGrid.Children.Add (minus[arrayIndex+1],x, y+2); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x+1, y+1); // value 
			layoutGrid.Children.Add (displayValue[arrayIndex+1],x+1, y+2); // value 
			layoutGrid.Children.Add (plus[arrayIndex],x+2, y+1); // Plus
			layoutGrid.Children.Add (plus[arrayIndex+1],x+2, y+2); // Plus
		}

		void pointsScored(){
			points = 0;

			double A = 0;
			double B = 0;
			double C = 0;
			double D = 0;
			double E = 0;
			try{
				A += Convert.ToDouble(data["autoA1"].ToString());
			}catch{}
			try{
				A += Convert.ToDouble(data["autoA2"].ToString());
			} catch{}
			if (A == 0 && scoreValue [0] < 2)
				points += 5 * scoreValue [0];
			else if (A == 0 && scoreValue [0] == 2)
				points += 10;
			else if (A != 0 && scoreValue [0] == 0)
				points += (int)(10 * A);
			else if (A != 0 && scoreValue[0] == 1)
				points += (int)(10*A+5);
			
			try{
				B += Convert.ToDouble(data["autoB1"].ToString());
			}catch{}
			try{
				B += Convert.ToDouble(data["autoB2"].ToString());
			} catch{}
			if (B == 0 && scoreValue [1] < 2)
				points += 5 * scoreValue [1];
			else if (B == 0 && scoreValue [1] == 2)
				points += 10;
			else if (B != 0 && scoreValue [1] == 0)
				points += (int)(10 * B);
			else if (B != 0 && scoreValue[1] == 1)
				points += (int)(10 * B + 5);
			
			try{
				C += Convert.ToDouble(data["autoC1"].ToString());
			}catch{}
			try{
				C += Convert.ToDouble(data["autoC2"].ToString());
			} catch{}
			if (C == 0 && scoreValue [2] < 2)
				points += 5 * scoreValue [2];
			else if (C == 0 && scoreValue [2] == 2)
				points += 10;
			else if (C != 0 && scoreValue [2] == 0)
				points += (int)(10 * C);
			else if (C != 0 && scoreValue[2] == 1)
				points += (int)(10 * C+5);
			
			try{
				D += Convert.ToDouble(data["autoD1"].ToString());
			}catch{}
			try{
				D += Convert.ToDouble(data["autoD2"].ToString());
			} catch{}
			if (D == 0 && scoreValue [3] < 2)
				points += 5 * scoreValue [3];
			else if (D == 0 && scoreValue [3] == 2)
				points += 10;
			else if (D != 0 && scoreValue [3] == 0)
				points += (int)(10 * D);
			else if (D != 0 && scoreValue[3] == 1)
				points += (int)(10 * D+5);
			
			try{
				E += Convert.ToDouble(data["autoE"].ToString());
			}catch{}
			if (E == 0 && scoreValue [4] < 2)
				points += 5 * scoreValue [4];
			else if (E == 0 && scoreValue [4] == 2)
				points += 10;
			else if (E != 0 && scoreValue [4] == 0)
				points += (int)(10 * E);
			else if (E != 0 && scoreValue[4] == 1)
				points += (int)(10 * E+5);
			
			points += 5 * Convert.ToInt16(data ["autoShotLowSuccess"]) + 2 * scoreValue[5];
			points += 10 * Convert.ToInt16(data ["autoShotHighSuccess"]) + 5 * scoreValue[7];

			if (scaled == true)
				points += 15;
			else if (challenge == true)
				points += 5;

			scoreLabel.Text = "Points: " + points.ToString();
		}

		void errorHandling(string d, int i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, double i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, bool i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}

