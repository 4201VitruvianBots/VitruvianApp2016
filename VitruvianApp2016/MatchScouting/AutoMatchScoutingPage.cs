using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class AutoMatchScoutingPage:ContentPage
	{
		ParseObject data;

		Grid layoutGrid = new Grid ();

		enum defA {Portcullis, Cheval_de_Frise};
		enum defB {Moat, Ramparts};
		enum defC {Drawbridge, Salley_Port};
		enum defD {Rock_Wall, Rough_Terrain};
		bool[,] autoDef = new bool[2, 5];
		bool temp;

		const int N = 10;
		double[] scoreValue = new double[N];
		Button[] minus = new Button[N];
		Button[] plus = new Button[N];
		Label[] displayValue = new Label[N];
		int[] def = new int[4]{-1, -1, -1, -1};
		int points = 0;

		Label scoreLabel = new Label () {
			HorizontalOptions = LayoutOptions.FillAndExpand,
			Text = "Points: 0",
			TextColor = Color.White,
			BackgroundColor = Color.Black,
			FontSize = GlobalVariables.sizeTitle,
			FontAttributes = FontAttributes.Bold
		};

		const string errorStringDefault = "The Following Data Was Unable To Be Saved: ";
		string errorString = errorStringDefault;
		bool error = false;

		public AutoMatchScoutingPage (ParseObject MatchData, int[] def)
		{
			for(int i=0; i<N; i++){
				scoreValue [i] = 0;
				minus [i] = new Button ();
				plus [i] = new Button ();
				displayValue[i] = new Label ();
			}
			for (int i = 0; i < 2; i++)
				for (int j = 0; j < 5; j++)
					autoDef [i, j] = false;

			Label pageTitle = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Autonomous Mode",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			defense (0, 0, 1, ((defA)def[0]).ToString());
			defense (1, 3, 1, ((defB)def[1]).ToString());
			defense (2, 6, 1, ((defC)def[2]).ToString());
			defense (3, 9, 1, ((defD)def[3]).ToString());
			defense (4, 12, 1, "Low Bar");
			shoot (5, 0, 4, "Low Shot");
			shoot (7, 3, 4, "High Shot");

			data = MatchData;

			Button TeleopPage = new Button (){
				Text = "TeleOp",
				BackgroundColor = Color.Yellow,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			TeleopPage.Clicked += (object sender, EventArgs e) => {
				if(def[0] == 0)
					errorHandling("autoA1", Convert.ToDouble(scoreValue[0]));
				else if(def[0] == 1)
					errorHandling("autoA2", Convert.ToDouble(scoreValue[0]));
				if(def[1] == 0)
					errorHandling("autoB1", Convert.ToDouble(scoreValue[1]));
				else if(def[1] == 1)
					errorHandling("autoB2", Convert.ToDouble(scoreValue[1]));
				if(def[2] == 0)
					errorHandling("autoC1", Convert.ToDouble(scoreValue[2]));
				else if(def[2] == 1)
					errorHandling("autoC2", Convert.ToDouble(scoreValue[2]));
				if(def[3] == 0)
					errorHandling("autoD1", Convert.ToDouble(scoreValue[3]));
				else if(def[3] == 1)
					errorHandling("autoD2", Convert.ToDouble(scoreValue[3]));
				errorHandling("autoE", Convert.ToDouble(scoreValue[4]));
				for(int i=0; i<5; i++){
					if(scoreValue[i]==0.2)
						errorHandling("autoReach", 1);
				}
				errorHandling("autoShotLowSuccess", Convert.ToInt32(scoreValue[5]));
				errorHandling("autoShotLowTotal", Convert.ToInt32(scoreValue[5]+scoreValue[6]));
				errorHandling("autoShotHighSuccess", Convert.ToInt32(scoreValue[7]));
				errorHandling("autoShotHighTotal", Convert.ToInt32(scoreValue[7]+scoreValue[8]));
				if(scoreValue[7]+scoreValue[8] == 0)
					errorHandling("autoShotHighAccuracy", Convert.ToDouble(0));
				else
					errorHandling("autoShotHighAccuracy", Convert.ToDouble((double)scoreValue[7]/(double)(scoreValue[7]+scoreValue[8])));

				if(error == true){
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
					errorString = errorStringDefault;
					error = false;
				} else {
					Navigation.PushModalAsync(new TeleOpMatchScoutingPage(data, def));
				}
			};
			layoutGrid.Children.Add (pageTitle, 0, 13, 0, 1);
			layoutGrid.Children.Add (scoreLabel, 12, 15, 0, 1);
			layoutGrid.Children.Add (TeleopPage, 12, 15, 4, 5);

			this.Content = new StackLayout () {
				Children = {
					layoutGrid
				}
			};
			BackgroundColor = Color.Teal;
		}

		void defense(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			displayValue [arrayIndex].TextColor = Color.Black;
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			minus [arrayIndex].Text = "Reach";
			minus [arrayIndex].BackgroundColor = Color.Red;
			plus [arrayIndex].Text = "Crossed";
			plus [arrayIndex].BackgroundColor = Color.Red;

			// Due to forgetting reaches/having code be uniform, this part of code will look wierd
			minus[arrayIndex].Clicked += (object sender, EventArgs e) => {	
				temp = autoDef[0, arrayIndex];
				for(int i=0; i<5; i++){
					minus[i].BackgroundColor = Color.Red;
					plus[i].BackgroundColor = Color.Red;
					autoDef[0, i] = false;
					autoDef[1,i]=false;
					scoreValue[i]=0;
				}
				autoDef[0, arrayIndex] = !temp;
				if(autoDef[0, arrayIndex] ==false){
					minus[arrayIndex].BackgroundColor = Color.Red;
					scoreValue[arrayIndex] = 0;
				} else if(autoDef[0, arrayIndex] ==true){
					minus[arrayIndex].BackgroundColor = Color.Green;
					scoreValue[arrayIndex] = 0.2;
				}
				pointsScored();
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				temp = autoDef[1, arrayIndex];
				for(int i=0; i<5; i++){
					minus[i].BackgroundColor = Color.Red;
					plus[i].BackgroundColor = Color.Red;
					autoDef[0, i] = false;
					autoDef[1,i]=false;
					scoreValue[i]=0;
				}
				autoDef[1, arrayIndex] = !temp;
				if(autoDef[1, arrayIndex] == false){
					plus[arrayIndex].BackgroundColor = Color.Red;
					scoreValue[arrayIndex] = 0;
				} else if(autoDef[1, arrayIndex] ==true){
					plus[arrayIndex].BackgroundColor = Color.Green;
					minus[arrayIndex].BackgroundColor = Color.Red;
					scoreValue[arrayIndex] = 1;
				}
				pointsScored();
			};

			Label defLabel = new Label () {
				Text = title,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			layoutGrid.Children.Add (defLabel,x, x+3, y, y+1); // Picker 
			layoutGrid.Children.Add (minus[arrayIndex],x, x+3, y+1, y+2); // Reach 
			layoutGrid.Children.Add (plus[arrayIndex],x,x+3,y+2, y+3); // Challenge
		}

		void shoot(int arrayIndex, int x, int y, string title){
			displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			displayValue [arrayIndex].TextColor = Color.Black;
			displayValue [arrayIndex].FontSize = GlobalVariables.sizeMedium;
			displayValue [arrayIndex + 1].Text = scoreValue[arrayIndex+1].ToString();
			displayValue [arrayIndex + 1].TextColor = Color.Black;
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
				}
				pointsScored();
			};
			minus[arrayIndex+1].Clicked += (object sender, EventArgs e) => {	// Miss
				if(scoreValue[arrayIndex+1] != 0){
					scoreValue[arrayIndex+1]--;							
					displayValue [arrayIndex+1].Text = scoreValue[arrayIndex+1].ToString();
				}
				pointsScored();
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
				HorizontalOptions = LayoutOptions.CenterAndExpand

			};
			layoutGrid.Children.Add (titleLabel,x, x+3, y, y+1); // title 
			layoutGrid.Children.Add (minus[arrayIndex],x, y+1); // Minus 
			layoutGrid.Children.Add (minus[arrayIndex+1],x, y+2); // Minus 
			layoutGrid.Children.Add (displayValue[arrayIndex],x+1, y+1); // value 
			layoutGrid.Children.Add (displayValue[arrayIndex+1],x+1, y+2); // value 
			layoutGrid.Children.Add (plus[arrayIndex],x+2, y+1); // Plus
			layoutGrid.Children.Add (plus[arrayIndex+1],x+2, y+2); // Plus
		}

		void pointsScored(){
			points = 0;

			for (int i = 0; i < 5; i++) {
				if (autoDef [0, i] == true)
					points += 2;
				if (autoDef [1, i] == true)
					points += 10;
			}

			points += 5 * (int)scoreValue[5];
			points += 10 * (int)scoreValue[7];

			scoreLabel.Text = "Points: " + points.ToString();
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

		void errorHandling(string d, int i){
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

