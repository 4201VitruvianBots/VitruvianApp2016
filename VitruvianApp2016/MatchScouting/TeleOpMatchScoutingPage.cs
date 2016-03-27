using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class TeleOpMatchScoutingPage:ContentPage
	{
		ParseObject data;

		Grid layoutGrid = new Grid ();

		enum defenses {Portcullis, Cheval_de_Frise, Moat, Ramparts, Drawbridge, Salley_Port, Rock_Wall, Rough_Terrain};
		enum parseString {teleOpA1, teleOpA2, teleOpB1, teleOpB2, teleOpC1, teleOpC2, teleOpD1, teleOpD2};
		enum defA {Portcullis, Cheval_de_Frise};
		enum defB {Moat, Ramparts};
		enum defC {Drawbridge, Salley_Port};
		enum defD {Rock_Wall, Rough_Terrain};

		const int N = 12;
		int[] scoreValue = new int[N];
		double[] aDef = new double[N];
		Button[] minus = new Button[N];
		Button[] plus = new Button[N];
		Button[] stallBtn = new Button[5];
		bool[] stallBool = new bool[5] {false, false, false, false, false};
		Label[] displayValue = new Label[N];
		Label[] attemptsDisplay = new Label[5];
		int[] attemptsValue = new int[5] {0,0,0,0,0};
		Button[] attemptsMinus = new Button[5];
		Button[] attemptsPlus = new Button[5];
		Label[] attemptsLabel = new Label[10];
		Label[] shotDisplay = new Label[4];
		int shotDisplayInt = 0;
		int defInt = 0;

		const string errorStringDefault = "The Following Data Was Unable To Be Saved: ";
		string errorString = errorStringDefault;
		bool error = false;
		bool disabled = false;
		bool challenge = false;
		bool scaled = false;
		int points = 0;

		Label scoreLabel = new Label () {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			Text = "Points: 0",
			TextColor = Color.White,
			BackgroundColor = Color.Black,
			FontSize = GlobalVariables.sizeTitle,
			FontAttributes = FontAttributes.Bold
		};

		public TeleOpMatchScoutingPage (ParseObject MatchData, int[] def, double[] autoScoreValue, int teamNo)
		{
			data = MatchData;

			for(int i=0; i<N; i++){
				scoreValue [i] = 0;
				minus [i] = new Button ();
				plus [i] = new Button ();
				displayValue[i] = new Label ();
			}

			Label pageTitle = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "TeleOp Mode",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label teamNumber = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Team: " + teamNo.ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			defense (0, 0, 1, "Low Bar", autoScoreValue[0]);
			defense (1, 3, 1, ((defenses)def[0]).ToString(), autoScoreValue[1]);
			defense (2, 6, 1, ((defenses)def[1]).ToString(), autoScoreValue[2]);
			defense (3, 9, 1, ((defenses)def[2]).ToString(), autoScoreValue[3]);
			defense (4, 12, 1, ((defenses)def[3]).ToString(), autoScoreValue[4]);
			shoot (5, 0, 7, "Low Shot");
			shoot (7, 3, 7, "High Shot");
			counter (9, 6, 8, "Teamwork");
			counter (10, 6, 10, "Shots Altered");
			counter (11, 12, 8, "Fouls");

			Button challengeBtn = new Button () {
				Text = "Robot Challenge",
				FontSize = GlobalVariables.sizeMedium,
				BackgroundColor = Color.Gray
			};
			challengeBtn.Clicked += (object sender, EventArgs e) => {
				if(challenge==false){
					challengeBtn.BackgroundColor = Color.Green;
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
					scaleBtn.BackgroundColor = Color.Green;
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
				if (new CheckInternetConnectivity().InternetStatus()){
					errorHandling("teleOpE", Convert.ToDouble(scoreValue[0]));
					errorHandling(((parseString)def[0]).ToString(), Convert.ToDouble(scoreValue[1]));
					errorHandling(((parseString)def[1]).ToString(), Convert.ToDouble(scoreValue[2]));
					errorHandling(((parseString)def[2]).ToString(), Convert.ToDouble(scoreValue[3]));
					errorHandling(((parseString)def[3]).ToString(), Convert.ToDouble(scoreValue[4]));
					errorHandling("teleOpEAtmpts", Convert.ToInt16(attemptsValue[0]));
					errorHandling(((parseString)def[0]).ToString()+"Atmpts", Convert.ToInt16(attemptsValue[1]));
					errorHandling(((parseString)def[1]).ToString()+"Atmpts", Convert.ToInt16(attemptsValue[2]));
					errorHandling(((parseString)def[2]).ToString()+"Atmpts", Convert.ToInt16(attemptsValue[3]));
					errorHandling(((parseString)def[3]).ToString()+"Atmpts", Convert.ToInt16(attemptsValue[4]));
					if(stallBool[0] == true)
						errorHandling("teleOpEStall", 1);
					if(stallBool[1] == true)
						errorHandling(((parseString)def[0]).ToString()+"Stall", 1);
					if(stallBool[2] == true)
						errorHandling(((parseString)def[1]).ToString()+"Stall", 1);
					if(stallBool[3] == true)
						errorHandling(((parseString)def[2]).ToString()+"Stall", 1);
					if(stallBool[4] == true)
						errorHandling(((parseString)def[3]).ToString()+"Stall", 1);
					errorHandling("teleOpShotLowSuccess", Convert.ToInt32(scoreValue[5]));
					errorHandling("teleOpShotLowTotal", Convert.ToInt32(scoreValue[5]+scoreValue[6]));
					if(scoreValue[5]+scoreValue[6] == 0)
						errorHandling("teleOpShotLowAccuracy", Convert.ToDouble(0));
					else
						errorHandling("teleOpShotLowAccuracy", Convert.ToDouble((double)scoreValue[5]/(double)(scoreValue[5]+scoreValue[6])));
					errorHandling("teleOpShotHighSuccess", Convert.ToInt32(scoreValue[7]));
					errorHandling("teleOpShotHighTotal", Convert.ToInt32(scoreValue[7]+scoreValue[8]));
					if(scoreValue[7]+scoreValue[8] == 0)
						errorHandling("teleOpShotHighAccuracy", Convert.ToDouble(0));
					else
						errorHandling("teleOpShotHighAccuracy", Convert.ToDouble((double)scoreValue[7]/(double)(scoreValue[7]+scoreValue[8])));
					errorHandling("teamwork", scoreValue[9]);
					errorHandling("shotsDenied", scoreValue[10]);
					errorHandling("fouls", scoreValue[11]);
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
				}
			};
			layoutGrid.Children.Add (pageTitle, 0, 4, 0, 1);
			layoutGrid.Children.Add (teamNumber, 3, 13, 0, 1);
			layoutGrid.Children.Add (scoreLabel, 12, 15, 0, 1);
			layoutGrid.Children.Add (challengeBtn, 9, 12, 10, 11);
			layoutGrid.Children.Add (scaleBtn, 9, 12, 11, 12);
			layoutGrid.Children.Add (disabledBtn, 12, 15, 10, 11);
			layoutGrid.Children.Add (endMatch, 12, 15, 11, 12);

			this.Content = new StackLayout () {
				Children = {
					layoutGrid
				}
			};
			BackgroundColor = Color.White;
		}

		void defense(int arrayIndex, int x, int y, string title, double autoDef){
			aDef [arrayIndex] = autoDef;
			displayValue [arrayIndex].Text = (scoreValue[arrayIndex] + aDef[arrayIndex]).ToString();
			displayValue [arrayIndex].TextColor = Color.Black;
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex].HorizontalOptions = LayoutOptions.Center;
			attemptsValue [arrayIndex] = Convert.ToInt16 (aDef [arrayIndex]);
			attemptsDisplay [arrayIndex] = new Label {
				Text = Convert.ToInt16(aDef[arrayIndex]).ToString(),
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.Center
			};

			attemptsLabel [arrayIndex] = new Label {
				Text = "Successes",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			attemptsLabel [arrayIndex+5] = new Label {
				Text = "Attempts",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			stallBtn [arrayIndex] = new Button () {
				BackgroundColor = Color.Gray,
				Text = "Stalled",
			};
			stallBtn[arrayIndex].Clicked += (object sender, EventArgs e) => {
				stallBool[arrayIndex] = !stallBool[arrayIndex];
				if(stallBool[arrayIndex] == true)
					stallBtn[arrayIndex].BackgroundColor = Color.Red;
				else
					stallBtn[arrayIndex].BackgroundColor = Color.Gray;
			};

			minus [arrayIndex].Text = "-";
			minus [arrayIndex].BackgroundColor = Color.Red;
			attemptsMinus [arrayIndex] = new Button () {
				Text = "-",
				BackgroundColor = Color.Red
			};
			plus [arrayIndex].Text = "+";
			plus [arrayIndex].BackgroundColor = Color.Green;
			attemptsPlus [arrayIndex] = new Button () {
				Text = "+",
				BackgroundColor = Color.Green
			};

			minus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if(scoreValue[arrayIndex] != 0){
					scoreValue[arrayIndex]--;
					attemptsValue[arrayIndex]--;
					displayValue [arrayIndex].Text = (scoreValue[arrayIndex] + aDef[arrayIndex]).ToString();							
					attemptsDisplay [arrayIndex].Text = attemptsValue[arrayIndex].ToString();
					pointsScored();
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if((scoreValue[arrayIndex] + aDef[arrayIndex] < 2 || (scoreValue[arrayIndex] < 2 && aDef[arrayIndex] == 0.2))){
					scoreValue[arrayIndex]++;
					attemptsValue[arrayIndex]++;
					displayValue [arrayIndex].Text = (scoreValue[arrayIndex] + aDef[arrayIndex]).ToString();							
					attemptsDisplay [arrayIndex].Text = attemptsValue[arrayIndex].ToString();
					pointsScored();
				}
			};
			attemptsMinus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if(attemptsValue[arrayIndex] != 0){
					if(attemptsValue[arrayIndex] != (scoreValue[arrayIndex] + Convert.ToInt16(aDef[arrayIndex]))){
						attemptsValue[arrayIndex]--;							
						attemptsDisplay [arrayIndex].Text = attemptsValue[arrayIndex].ToString();
					}
				}
			};
			attemptsPlus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				attemptsValue[arrayIndex]++;
				attemptsDisplay [arrayIndex].Text = attemptsValue[arrayIndex].ToString();
			};

			Label defLabel = new Label () {
				Text = title,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			layoutGrid.Children.Add (defLabel,x, x+3, y, y+1); // Picker 
			layoutGrid.Children.Add (attemptsLabel[arrayIndex],x, x+3, y+1, y+2); // Picker 
			layoutGrid.Children.Add (minus[arrayIndex],x, y+2); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x+1, y+2); // Minus  
			layoutGrid.Children.Add (plus[arrayIndex],x+2, y+2); // Plus
			layoutGrid.Children.Add (attemptsLabel[arrayIndex+5],x, x+3, y+3, y+4); // Picker 
			layoutGrid.Children.Add (attemptsMinus[arrayIndex],x, y+4); // Minus 
			layoutGrid.Children.Add (attemptsDisplay[arrayIndex],x+1, y+4); // Minus  
			layoutGrid.Children.Add (attemptsPlus[arrayIndex],x+2, y+4); // Plus
			layoutGrid.Children.Add (stallBtn[arrayIndex],x, x+3, y+5, y+6); // Picker 
		}

		void shoot(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			displayValue [arrayIndex].TextColor = Color.Black;
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex].HorizontalOptions = LayoutOptions.Center;
			displayValue [arrayIndex + 1].Text = scoreValue[arrayIndex+1].ToString();
			displayValue [arrayIndex + 1].TextColor = Color.Black;
			displayValue [arrayIndex + 1].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex + 1].HorizontalOptions = LayoutOptions.Center;
			shotDisplay [shotDisplayInt] = new Label {
				Text = "Hits",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			shotDisplay [shotDisplayInt + 1] = new Label {
				Text = "Misses",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

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
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.CenterAndExpand

			};
			layoutGrid.Children.Add (titleLabel,x, x+3, y, y+1); // title 
			layoutGrid.Children.Add (shotDisplay[shotDisplayInt], x, x + 3, y + 1, y + 2); // Hits 
			layoutGrid.Children.Add (minus[arrayIndex],x, y + 2); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x + 1, y + 2); // value 
			layoutGrid.Children.Add (plus[arrayIndex],x + 2, y + 2); // Plus
			layoutGrid.Children.Add (shotDisplay[shotDisplayInt + 1], x, x + 3, y + 3, y + 4); // Misses 
			layoutGrid.Children.Add (minus[arrayIndex+1],x, y + 4); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex+1],x + 1, y+ 4); // value 
			layoutGrid.Children.Add (plus[arrayIndex+1],x + 2, y + 4); // Plus
			shotDisplayInt += 2;
		}

		void counter(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = "0";
			displayValue [arrayIndex].TextColor = Color.Black;
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex].HorizontalOptions = LayoutOptions.Center;

			minus [arrayIndex].Text = "-";
			minus [arrayIndex].BackgroundColor = Color.Red;
			plus [arrayIndex].Text = "+";
			plus [arrayIndex].BackgroundColor = Color.Green;

			minus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if(scoreValue[arrayIndex] != 0){
					scoreValue[arrayIndex]--;
					displayValue [arrayIndex].Text = (scoreValue[arrayIndex] + aDef[arrayIndex]).ToString();
					pointsScored();
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				scoreValue[arrayIndex]++;
				displayValue [arrayIndex].Text = (scoreValue[arrayIndex] + aDef[arrayIndex]).ToString();
				pointsScored();
			};

			Label defLabel = new Label () {
				Text = title,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			layoutGrid.Children.Add (defLabel,x, x+3, y, y+1); // Picker 
			layoutGrid.Children.Add (minus[arrayIndex],x, y+1); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x+1, y+1); // Minus  
			layoutGrid.Children.Add (plus[arrayIndex],x+2, y+1); // Plus
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
			else if (A == 0.2 && scoreValue[0] == 2)
				points += (int)(10*A+10);
			
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
			else if (B == 0.2 && scoreValue[1] == 2)
				points += (int)(10*B+10);
			
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
			else if (C == 0.2 && scoreValue[2] == 2)
				points += (int)(10*C+10);
			
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
			else if (D == 0.2 && scoreValue[3] == 2)
				points += (int)(10*D+10);
			
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
			else if (E == 0.2 && scoreValue[4] == 2)
				points += (int)(10*E+10);
			
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

