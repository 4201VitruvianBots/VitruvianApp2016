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

		const int N = 10;
		int[] scoreValue = new int[N];
		Button[] minus = new Button[N];
		Button[] plus = new Button[N];
		Label[] displayValue = new Label[N];
		int[] def = new int[4]{-1, -1, -1, -1};

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

			Label pageTitle = new Label () {
				Text = "Autonomous Mode",
				FontSize = GlobalVariables.sizeTitle
			};

			defense (0, 0, 1, ((defA)def[0]).ToString());
			defense (1, 3, 1, ((defB)def[1]).ToString());
			defense (2, 6, 1, ((defC)def[2]).ToString());
			defense (3, 9, 1, ((defD)def[3]).ToString());
			defense (4, 12, 1, "Low Bar");
			shoot (5, 0, 3, "Low Shot");
			shoot (7, 3, 3, "High Shot");

			data = MatchData;

			Button TeleopPage = new Button (){
				Text = "TeleOp",
				BackgroundColor = Color.Yellow,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			TeleopPage.Clicked += (object sender, EventArgs e) => {
				if(def[0] == 0)
					errorHandling("autoA1", Convert.ToInt32(scoreValue[0]));
				else if(def[0] == 1)
					errorHandling("autoA2", Convert.ToInt32(scoreValue[0]));
				if(def[1] == 0)
					errorHandling("autoB1", Convert.ToInt32(scoreValue[1]));
				else if(def[1] == 1)
					errorHandling("autoB2", Convert.ToInt32(scoreValue[1]));
				if(def[2] == 0)
					errorHandling("autoC1", Convert.ToInt32(scoreValue[2]));
				else if(def[2] == 1)
					errorHandling("autoC2", Convert.ToInt32(scoreValue[2]));
				if(def[3] == 0)
					errorHandling("autoD1", Convert.ToInt32(scoreValue[3]));
				else if(def[3] == 1)
					errorHandling("autoD2", Convert.ToInt32(scoreValue[3]));
				errorHandling("autoE", Convert.ToInt32(scoreValue[4]));
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
			layoutGrid.Children.Add (pageTitle, 0, 6, 0, 1);
			layoutGrid.Children.Add (TeleopPage, 12, 15, 4, 5);

			this.Content = new StackLayout () {
				Children = {
					layoutGrid
				}
			};
			BackgroundColor = Color.Gray;
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
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {
				if(scoreValue[arrayIndex] != 1){
					scoreValue[arrayIndex]++;
					displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
				}
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
				}
			};
			minus[arrayIndex+1].Clicked += (object sender, EventArgs e) => {	// Miss
				if(scoreValue[arrayIndex+1] != 0){
					scoreValue[arrayIndex+1]--;							
					displayValue [arrayIndex+1].Text = scoreValue[arrayIndex+1].ToString();
				}
			};
			plus[arrayIndex].Clicked += (object sender, EventArgs e) => {	// Hit
				scoreValue[arrayIndex]++;
				displayValue [arrayIndex].Text = scoreValue[arrayIndex].ToString();
			};
			plus[arrayIndex+1].Clicked += (object sender, EventArgs e) => {	// Miss
				scoreValue[arrayIndex+1]++;
				displayValue [arrayIndex+1].Text = scoreValue[arrayIndex+1].ToString();
			};

			Label titleLabel = new Label () {
				Text = title,
				FontSize = GlobalVariables.sizeTitle,
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

