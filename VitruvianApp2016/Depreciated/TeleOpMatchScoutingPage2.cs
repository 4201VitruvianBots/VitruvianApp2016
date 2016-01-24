using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class TeleOpMatchScoutingPage:ContentPage
	{
		ParseObject data;

		string errorString = "The following data was unable to be saved: ";
		bool errorStatus = false;

		public TeleOpMatchScoutingPage (ParseObject MatchData)
		{
			int N = 35, Z = 0; //N is the maximum number of points, Z is which cycle the user is currently on.
			int[] CyclePoints = new int[N];

			ToteCount.BackgroundColor = Color.White;
			ToteCount.TextColor = Color.Black; 
			ToteCount.FontSize = 18;
			ToteCount.Text = "0";
			ToteCount.HorizontalOptions = LayoutOptions.CenterAndExpand;
			ToteCount.VerticalOptions = LayoutOptions.CenterAndExpand;
			ToteStackPoints.Text = "0";

			Label LandfillTotesLabel = new Label {
				Text = "Landfill",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Label LandfillTotesDisplay = new Label (){
				Text = "0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Button LandfillPlus = new Button ();						//A +Tote Button for the "Stacking" cluster
			LandfillPlus.Text = "+";
			LandfillPlus.BackgroundColor = Color.Green;
			LandfillPlus.Clicked += (object sender, EventArgs e) => {
				if (landfillTotes+stationTotes < 6) { 
					landfillTotes++;
					UpdateValues();
					LandfillTotesDisplay.Text = landfillTotes.ToString();
					ToteCount.Text = Convert.ToString(landfillTotes+stationTotes);
				}
			};

			Button LandfillMinus = new Button ();						//A +Tote Button for the "Stacking" cluster
			LandfillMinus.Text = "-";
			LandfillMinus.BackgroundColor = Color.Red;
			LandfillMinus.Clicked += (object sender, EventArgs e) => {
				if (landfillTotes+stationTotes <= 6 && landfillTotes != 0) { 
					landfillTotes--;
					UpdateValues();
					LandfillTotesDisplay.Text = landfillTotes.ToString();
					ToteCount.Text = Convert.ToString(landfillTotes+stationTotes);
				}
			};

			Label stationTotesLabel = new Label {
				Text = "Station",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Label stationTotesDisplay = new Label{
				Text ="0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Button stationPlus = new Button ();						//A +Tote Button for the "Stacking" cluster
			stationPlus.Text = "+";
			stationPlus.BackgroundColor = Color.Green;
			stationPlus.Clicked += (object sender, EventArgs e) => {
				if (landfillTotes+stationTotes < 6) { 
					stationTotes++;
					UpdateValues();
					stationTotesDisplay.Text = stationTotes.ToString();
					ToteCount.Text = Convert.ToString(landfillTotes+stationTotes);
				}
			};

			Button stationMinus = new Button ();						//A +Tote Button for the "Stacking" cluster
			stationMinus.Text = "-";
			stationMinus.BackgroundColor = Color.Red;
			stationMinus.Clicked += (object sender, EventArgs e) => {
				if (landfillTotes+stationTotes <= 6 && stationTotes!=0) { 
					stationTotes--;
					UpdateValues();
					stationTotesDisplay.Text = stationTotes.ToString(); 
					ToteCount.Text = Convert.ToString(landfillTotes+stationTotes);
				}
			};

			Button ToteStack = new Button ();
			ToteStack.Text = "Stacking Totes";
			ToteStack.BackgroundColor = Color.Green;
			ToteStack.TextColor = Color.Black;
			ToteStack.Clicked += (object sender, EventArgs e) => {
				if(stacker==0){
					stacker++;
					ToteStack.BackgroundColor = Color.Gray;
					LandfillTotesLabel.Text = "Tote stack hieght";
					stationTotesLabel.Text = "Totes with can";
					UpdateValues();
				} else {
					stacker--;
					ToteStack.BackgroundColor = Color.Green;
					LandfillTotesLabel.Text = "Landfill";
					stationTotesLabel.Text = "Station";
					UpdateValues();
				}
			};


			Label canLabel = new Label {
				Text = "Cans",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};


			Label CanCount = new Label () { //Can Counter 
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Text = "0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Button canPlus = new Button ();						//A +Tote Button for the "Stacking" cluster
			canPlus.Text = "+";
			canPlus.BackgroundColor = Color.Green;
			canPlus.Clicked += (object sender, EventArgs e) => {
				CanButtonPushes++;
				CanCount.Text = Convert.ToString (CanButtonPushes.ToString ()); 
				UpdateValues();
			};

			Button canMinus = new Button ();						//A +Tote Button for the "Stacking" cluster
			canMinus.Text = "-";
			canMinus.BackgroundColor = Color.Red;
			canMinus.Clicked += (object sender, EventArgs e) => {
				if (CanButtonPushes != 0) { 
					CanButtonPushes--;
					CanCount.Text = Convert.ToString (CanButtonPushes.ToString ()); 
					UpdateValues();
				}
			};

			Label LitterCount = new Label ();		
			LitterCount.BackgroundColor = Color.White;
			LitterCount.TextColor = Color.Black;

			Button LitterButton = new Button ();
			LitterButton.Text = "Litter";
			LitterButton.BackgroundColor=Color.Gray;
			LitterButton.Clicked += (object sender, EventArgs e) => {
				if (LitterButtonPushes == 0 && CanButtonPushes == 1) {
					LitterButtonPushes++;
					LitterCount.Text = LitterButtonPushes.ToString ();
					LitterButton.BackgroundColor=Color.Green;
					UpdateValues();
				} else if(LitterButtonPushes == 1 && CanButtonPushes == 1){
					LitterButtonPushes--;
					LitterCount.Text = LitterButtonPushes.ToString ();
					LitterButton.BackgroundColor=Color.Gray;
					UpdateValues();
				}
			};

			Label CoopertitionLabel = new Label () {
				Text = "Coopertition",
				BackgroundColor = Color.Black,
				TextColor = Color.Green,
				HorizontalOptions = LayoutOptions.Center
			};

			Button CoopertitionSet = new Button();
			CoopertitionSet.Text = "Set";
			CoopertitionSet.BackgroundColor = Color.Gray;
			CoopertitionSet.Clicked +=(object sender, EventArgs e) =>{
				//CoopertitionStackButtonClicked = true; 
				if (CoopertitionSetPushes == 0) 
				{		
					//CyclePoints[Z]+=20;                                           
					CoopertitionSetPushes++;
					CoopertitionSet.BackgroundColor = Color.Yellow;                                        
					//CoopertitionSetCount.Text = CoopertitionSetPushes.ToString(); 
					UpdateValues();
				} else {
					CoopertitionSetPushes--;
					CoopertitionSet.BackgroundColor = Color.Gray;                                        
					//CoopertitionSetCount.Text = CoopertitionSetPushes.ToString(); 
					UpdateValues();
				}
			};

			Button CoopertitionStack = new Button();
			CoopertitionStack.Text = "Stack";
			CoopertitionStack.BackgroundColor = Color.Gray;

			CoopertitionStack.Clicked +=(object sender, EventArgs e) =>{
				if (CoopertitionStackPushes== 0)
				{
					//CoopertitionStackButtonClicked = true;
					//CyclePoints[Z]+=40;
					currentCyclePoints.ToString();
					CoopertitionStackPushes++;
					CoopertitionStack.BackgroundColor = Color.Yellow;
					//CoopertitionStackCount.Text = CoopertitionStackPushes.ToString();
					UpdateValues();
				} else {
					CoopertitionStackPushes--;
					CoopertitionStack.BackgroundColor = Color.Gray;
					//CoopertitionStackCount.Text = CoopertitionStackPushes.ToString();
					UpdateValues();
				}
			};

			Label scoreCount = new Label () {
				Text = totalScore.ToString ()
			};

			Label TotalScoreCount = new Label();
			Button ScoreResetToteStack = new Button();
			ScoreResetToteStack.Text = "Score";
			ScoreResetToteStack.BackgroundColor = Color.Lime;
			ScoreResetToteStack.Clicked += (object sender, EventArgs e)=>
			{
				if(landfillTotes+stationTotes>=4 && CanButtonPushes>0 && goodStacker==false){
					goodStacker = true;
				}
				/*
				scorecount was a label I created to display the 'total cumulative score' of all cycles. Technically, this is redundant, since totalScore should have the value already, but if you want to specify it as a label (to change the color/font size,etc.) then it is fine.
				If you want to just state the total score, without the need of changing how it looks, you can put TeleopLayout.Children.Add(totalScore, int, int);
				*/
				CyclePoints[Z]=currentCyclePoints;
				totalScore+=CyclePoints[Z];
				scoreCount.Text = totalScore.ToString();
				TotalScoreCount.Text = totalScore.ToString(); 
				Z++;


				//Reset Tote, Can, Litter Counters (of the individual totestack cluster)
				//ToteButtonPushes = 0;	//Max. of 6
				//ToteCount.Text=ToteButtonPushes.ToString();

				totalLandfillTotes += landfillTotes;
				landfillTotes =0;
				LandfillTotesDisplay.Text ="0";
				totalStationTotes += stationTotes;
				stationTotes = 0;
				stationTotesDisplay.Text = "0";
				CanButtonPushes = 0;  //Max of 1
				CanCount.Text=CanButtonPushes.ToString();
				LitterButtonPushes = 0;
				LitterCount.Text = LitterButtonPushes.ToString();
				LitterButton.BackgroundColor = Color.Gray;

				CoopertitionStackPushes=0;
				CoopertitionStack.BackgroundColor = Color.Gray;
				CoopertitionSetPushes=0;
				CoopertitionSet.BackgroundColor = Color.Gray;

				UpdateValues();
			};

			Button ResetStack = new Button { 
				Text = "Reset Count",
				BackgroundColor = Color.Green,
			};

			ResetStack.Clicked += (object sender, EventArgs e) => {

				ToteButtonPushes = 0;	//Max. of 6
				ToteCount.Text=ToteButtonPushes.ToString();
				CanButtonPushes = 0;  //Max of 1
				CanCount.Text=CanButtonPushes.ToString();
				LitterButtonPushes = 0;
				LitterCount.Text = LitterButtonPushes.ToString();
				LitterButton.BackgroundColor=Color.Gray;
				landfillTotes =0;
				LandfillTotesDisplay.Text = landfillTotes.ToString();
				stationTotes =0;
				stationTotesDisplay.Text = stationTotes.ToString();

				UpdateValues(); 
			};

			int stepCans = 0;

			Label stepCanPullLabel = new Label {
				Text = "Step Can Pull",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};


			Label stepCanCount = new Label () { //Can Counter 
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Text = "0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Button stepCanPlus = new Button ();						//A +Tote Button for the "Stacking" cluster
			stepCanPlus.Text = "+";
			stepCanPlus.BackgroundColor = Color.Green;
			stepCanPlus.Clicked += (object sender, EventArgs e) => {
				stepCans++;
				stepCanCount.Text = Convert.ToString (stepCans.ToString ()); 
			};

			Button stepCanMinus = new Button ();						//A +Tote Button for the "Stacking" cluster
			stepCanMinus.Text = "-";
			stepCanMinus.BackgroundColor = Color.Red;
			stepCanMinus.Clicked += (object sender, EventArgs e) => {
				if (stepCans != 0) { 
					stepCans--;
					stepCanCount.Text = Convert.ToString (stepCans.ToString ()); 
				}
			};

			int canUpright = 0;

			Label canUprightLabel = new Label {
				Text = "R.Can Upright",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};


			Label canUprightCount = new Label () { //Can Counter 
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Text = "0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			Button canUprightPlus = new Button ();						//A +Tote Button for the "Stacking" cluster
			canUprightPlus.Text = "+";
			canUprightPlus.BackgroundColor = Color.Green;
			canUprightPlus.Clicked += (object sender, EventArgs e) => {
				canUpright++;
				canUprightCount.Text = Convert.ToString (canUpright.ToString ()); 
			};

			Button canUprightMinus = new Button ();						//A +Tote Button for the "Stacking" cluster
			canUprightMinus.Text = "-";
			canUprightMinus.BackgroundColor = Color.Red;
			canUprightMinus.Clicked += (object sender, EventArgs e) => {
				if (canUpright != 0) { 
					canUpright--;
					canUprightCount.Text = Convert.ToString (canUpright.ToString ()); 
				}
			};
			Label litterThrowLabel = new Label {
				Text = "Litter Throws",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};


			int throws = 0;
			int Success = 0;

			Label litterThrowCount = new Label{
				Text = "0"
			};

			Button litterThrow = new Button ();						//A +Tote Button for the "Stacking" cluster
			litterThrow.Text = "Throw";
			litterThrow.BackgroundColor = Color.Green;
			litterThrow.Clicked += (object sender, EventArgs e) => {
				throws++;
				litterThrowCount.Text = Convert.ToString (throws.ToString ()); 
			};

			Label litterSuccessCount = new Label{
				Text = "0"
			};

			Button litterSuccess = new Button ();						
			litterSuccess.Text = "Success";
			litterSuccess.BackgroundColor = Color.Green;
			litterSuccess.Clicked += (object sender, EventArgs e) => {
				if(Success<throws){
					Success++;
					litterSuccessCount.Text = Convert.ToString (Success.ToString ()); 
				}
			};

			Button litterThrowReset = new Button ();			
			litterThrowReset.Text = "Reset";
			litterThrowReset.BackgroundColor = Color.Red;
			litterThrowReset.Clicked += (object sender, EventArgs e) => {
				Success=0;
				throws=0;
				litterThrowCount.Text = Convert.ToString (throws.ToString ()); 
				litterSuccessCount.Text = Convert.ToString (Success.ToString ()); 
			};

			bool disableToggle = false;
			Button Disable = new Button();
			Disable.Text= "Robot Disabled";
			Disable.BackgroundColor = Color.Gray;
			Disable.Clicked += (object sender, EventArgs e) =>  {
				if(disableToggle==false){
					disableToggle=true;
					Disable.BackgroundColor = Color.Red;
				} else{
					disableToggle=false;
					Disable.BackgroundColor = Color.Gray;
				}
			};

			data = MatchData;

			Button finishTeleop = new Button();
			finishTeleop.Text = "End Teleop";
			finishTeleop.BackgroundColor = Color.Red;
			finishTeleop.TextColor = Color.Black;


			finishTeleop.Clicked += (object sender, EventArgs e) => {
				
				if (goodStacker == true) {
					errorHandling("goodstack", goodStacker);
				} else {
					errorHandling("goodstack", goodStacker);
				}
				errorHandling("CycleAmount", Z);
				errorHandling("TotalScore", totalScore);
				errorHandling("CycleData", CyclePoints);
				errorHandling("disabled", disableToggle);
				errorHandling("teleopStepCanPulls", stepCans);
				errorHandling("landfillTotes", totalLandfillTotes);
				errorHandling("stationTotes", totalStationTotes);
				errorHandling("stepCanPull", stepCans);
				errorHandling("canUprightCount", canUpright);
				errorHandling("humanThrows", throws);
				errorHandling("humanThrowsSuccess", Success);

				if(errorStatus == true){
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
				} else {
					Navigation.PushModalAsync(new PostMatchScoutingPage(data));
				}
				/*
				if (goodStacker == true) {
					errorHandling("goodstack",goodStacker.ToString());
					data ["goodStack"] = goodStacker;
				} else {
					data ["goodStack"] = false;
				}
				data ["CycleAmount"] = Z;
				data ["TotalScore"] = Convert.ToInt32 (totalScore.ToString ());
				data ["CycleData"] = CyclePoints;
				data ["disabled"] = disableToggle;
				data ["teleopStepCanPulls"] = Convert.ToInt16 (stepCans.ToString ());
				if(stacker==0){
					data ["landfillTotes"] = totalLandfillTotes;
					data ["stationTotes"] = totalStationTotes;
				}
				data ["stepCanPull"] = stepCans;
				data ["canUprightCount"] = canUpright;
				data ["humanThrows"] = throws;
				data ["humanThrowsSuccess"] = Success;

				SaveData();
				Navigation.PushModalAsync(new PostMatchScoutingPage(data));
				*/
			};

			Grid TeleopLayout = new Grid () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				ColumnDefinitions = {
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto }, 
				},
				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition{ Height = GridLength.Auto}, 
				},
				//with two integers: int left, int top
				//with four integers: int left, int right, int top, int bottom
			};

			Label currentCyclePointsLabel = new Label {
				Text = "Current Cycle:"
			};
			Label totalPointsLabel = new Label{
				Text = "Total:"
			};

			TeleopLayout.Children.Add (ToteStack, 0, 3, 1, 2); //label - "Stacking Totes"

			TeleopLayout.Children.Add (LandfillMinus, 0, 2);
			TeleopLayout.Children.Add (LandfillTotesLabel, 1, 2);
			TeleopLayout.Children.Add (LandfillPlus, 2, 2);
			TeleopLayout.Children.Add (LandfillTotesDisplay,3,2);

			TeleopLayout.Children.Add (stationMinus, 0, 3);
			TeleopLayout.Children.Add (stationTotesLabel, 1, 3);
			TeleopLayout.Children.Add (stationPlus, 2, 3);
			TeleopLayout.Children.Add (stationTotesDisplay,3,3);

			TeleopLayout.Children.Add (ToteCount, 4, 5, 2, 4);

			TeleopLayout.Children.Add (canMinus, 0, 4);
			TeleopLayout.Children.Add (canLabel, 1, 4);
			TeleopLayout.Children.Add (canPlus, 2, 4);
			TeleopLayout.Children.Add (CanCount, 3, 4);

			TeleopLayout.Children.Add (LitterButton,0,3,5,6);
			TeleopLayout.Children.Add (ScoreResetToteStack, 3,6,6,7);
			TeleopLayout.Children.Add (ResetStack, 0,3,6,7);


			TeleopLayout.Children.Add (currentCyclePointsLabel, 1,0);
			TeleopLayout.Children.Add (ToteStackPoints, 2,0); //Shows points gained from the Totestack
			TeleopLayout.Children.Add (totalPointsLabel, 3,0);
			TeleopLayout.Children.Add (scoreCount, 4,0);
			//TeleopLayout.Children.Add (LitterCount, 1, 3, 3, 4);
			//TeleopLayout.Children.Add (TotalScoreCount, 1, 1, 0, 1); //needs debugging - argument has been thrown

			TeleopLayout.Children.Add (stepCanMinus, 0, 7);
			TeleopLayout.Children.Add (stepCanPullLabel, 1, 7);
			TeleopLayout.Children.Add (stepCanPlus, 2, 7);
			TeleopLayout.Children.Add (stepCanCount, 3,6, 7,8);

			TeleopLayout.Children.Add (canUprightMinus, 0, 8);
			TeleopLayout.Children.Add (canUprightLabel, 1, 8);
			TeleopLayout.Children.Add (canUprightPlus, 2, 8);
			TeleopLayout.Children.Add (canUprightCount, 3,6, 8,9);

			TeleopLayout.Children.Add (litterThrowLabel, 0,9);
			TeleopLayout.Children.Add (litterThrow, 1, 9);
			TeleopLayout.Children.Add (litterThrowCount, 2, 9);
			TeleopLayout.Children.Add (litterSuccess, 1,10);
			TeleopLayout.Children.Add (litterSuccessCount, 2,10);
			TeleopLayout.Children.Add (litterThrowReset, 0,10);

			/*
			If you use a Children.Add with 4 values, and then use ones with 2, keep in mind that this may affect how everything else is layed, out, so you should try to remain consisntant and have all of them with 4 variables.
			*/
			TeleopLayout.Children.Add (CoopertitionLabel, 0, 6, 11, 12);
			TeleopLayout.Children.Add (CoopertitionStack, 0,2,12, 13);
			TeleopLayout.Children.Add (CoopertitionSet, 2,6,12,13);
			TeleopLayout.Children.Add (Disable, 0,6,13,14);
			TeleopLayout.Children.Add (finishTeleop, 0, 6, 14, 15);

			//TeleopLayout.Children.Add (CoopertitionStackCount, 1, 3, 7, 8);
			//TeleopLayout.Children.Add (CoopertitionSetCount, 1,3, 8,9);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = TeleopLayout
			};

		}

		void errorHandling(string d, string i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, int i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, int[] i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, bool i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}

		async void UpdateValues(){

		}
	}  
}
