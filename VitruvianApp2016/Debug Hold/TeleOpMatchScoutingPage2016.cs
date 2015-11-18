using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class TeleOpMatchScoutingPage:ContentPage
	{
		ParseObject data;

		static int N = 35, Z = 99;
		int cycleCount = 0, labelCount = 0;
		int[] counter = new int[N];
		int[] cyclePoints = new int[N];
		Label[] counterLabel = new Label[Z];
		Label[] counterDisplay = new Label[Z];
		Button[] plus = new Button[Z];
		Button[] minus = new Button[Z];
		Label toteCount = new Label ();
		Label stackScoreLabel = new Label();

		int landfillTotes = 0, landfillTotesTotal, stationTotes = 0, stationTotesTotal = 0, totalTotes = 0, canCount = 0, totalCanCount = 0, stepCanPulls = 0, totalStepCanPulls = 0, canUpright = 0, litterStack = 0, litterCount = 0, litterSuccesses = 0, coOpSet = 0, coOpStack = 0, stackScore = 0, totalScore = 0;
		bool stacker = true, goodStacker = false, disabled = false;

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
				new RowDefinition{ Height = GridLength.Auto}, 
				new RowDefinition{ Height = GridLength.Auto}, 
				new RowDefinition{ Height = GridLength.Auto}, 
			},
		};

		public TeleOpMatchScoutingPage (ParseObject obj)
		{
			Label toteCount = new Label () {
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = totalTotes.ToString()
			};

			dataCounter ("Landfill", landfillTotes);
			dataCounter ("Station", stationTotes);
			dataCounter ("Cans", canCount);
			dataCounter ("Step Can Pulls", stepCanPulls);
			dataCounter ("Uprighting Cans", canUpright);

			Label currentCycleScoreLabel = new Label () {
				Text = "Current Cycle Score:",
				FontSize = GlobalVariables.sizeMedium
			};

			stackScoreLabel.Text = "0";

			Label totalScoreLabel = new Label () {
				Text = "Total Score:",
				FontSize = GlobalVariables.sizeMedium
			};

			Label totalScoreCountLabel = new Label () {
				Text = totalScore.ToString(),
				FontSize = GlobalVariables.sizeMedium
			};

			Button stackingButton = new Button () {
				Text = "Main Stacker",
				TextColor = Color.Black,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};
			stackingButton.Clicked += (object sender, EventArgs e) => {
				if (stacker == true)
					stackingButton.BackgroundColor = Color.Gray;
				else
					stackingButton.BackgroundColor = Color.Green;
				stacker = !stacker;
 			};

			Button litterStackBtn = new Button () {
				Text = "Stack w/ Litter",
				TextColor = Color.Black,
				BackgroundColor = Color.Gray,
				FontSize = GlobalVariables.sizeMedium
			};
			litterStackBtn.Clicked += (object sender, EventArgs e) => {
				if (litterStack == 0){
					litterStackBtn.BackgroundColor = Color.Green;
					litterStack++;
				} else {
					litterStackBtn.BackgroundColor = Color.Gray;
					litterStack--;
				}
			};

			Button stackResetBtn = new Button () {
				Text = "Reset Stack",
				TextColor = Color.Black,
				BackgroundColor = Color.Red,
				FontSize = GlobalVariables.sizeMedium
			};
			stackingButton.Clicked += (object sender, EventArgs e) => {
				landfillTotes = stationTotes = totalTotes = canCount = litterStack = stackScore = 0;
				litterStackBtn.BackgroundColor = Color.Gray;
			};

			Button scoreBtn = new Button () {
				Text = "Score",
				TextColor = Color.Black,
				BackgroundColor = Color.Lime,
				FontSize = GlobalVariables.sizeMedium
			};
			scoreBtn.Clicked += (object sender, EventArgs e) => {
				if(landfillTotes+stationTotes>=4 && canCount > 0 && goodStacker==false)
					goodStacker = true;
				if(stacker == true){
					landfillTotesTotal += landfillTotes;
					stationTotesTotal += stationTotes;
					totalTotes += landfillTotes + stationTotes;
				}
				totalCanCount += canCount;
				totalStepCanPulls += stepCanPulls;
				cyclePoints[cycleCount] = stackScore;
				totalScore += cyclePoints[cycleCount];
				cycleCount++;
				stackScoreLabel.Text = "0";
				for(int i=0; i<3; i++){
					counterDisplay[i].Text = "0";
				}
				landfillTotes = stationTotes = totalTotes = canCount = litterStack = stackScore = stepCanPulls = 0;
				litterStackBtn.BackgroundColor = Color.Gray;
			};

			Label litterThowsLabel = new Label () {
				Text = "Litter Throws",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = GlobalVariables.sizeMedium
			};

			Button litterThrowBtn = new Button () {
				Text = "Thows",
				TextColor = Color.Black,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};
			stackingButton.Clicked += (object sender, EventArgs e) => {
				litterCount++;
			};

			Button litterSuccessesBtn = new Button () {
				Text = "Successes",
				TextColor = Color.Black,
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};
			stackingButton.Clicked += (object sender, EventArgs e) => {
				if(litterSuccesses < litterCount)
					litterSuccesses++;
			};

			Button litterResetBtn = new Button () {
				Text = "Reset",
				TextColor = Color.Black,
				BackgroundColor = Color.Red,
				FontSize = GlobalVariables.sizeMedium
			};
			stackingButton.Clicked += (object sender, EventArgs e) => {
				litterCount = litterSuccesses = 0;
			};

			Label coOpLabel = new Label () {
				Text = "Coopertition",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			Button coOpSetBtn = new Button () {
				Text = "Set",
				TextColor = Color.Black,
				BackgroundColor = Color.Gray,
				FontSize = GlobalVariables.sizeMedium
			};
			coOpSetBtn.Clicked += (object sender, EventArgs e) => {
				if(coOpSet == 0){
					coOpSetBtn.BackgroundColor = Color.Green;
					coOpSet++;
				} else {
					coOpSetBtn.BackgroundColor = Color.Gray;
					coOpSet--;
				}
			};

			Button coOpStackBtn = new Button () {
				Text = "Stack",
				TextColor = Color.Black,
				BackgroundColor = Color.Gray,
				FontSize = GlobalVariables.sizeMedium
			};
			coOpStackBtn.Clicked += (object sender, EventArgs e) => {
				if(coOpStack == 0){
					coOpStackBtn.BackgroundColor = Color.Green;
					coOpStack++;
				} else {
					coOpStackBtn.BackgroundColor = Color.Gray;
					coOpStack--;
				}
			};

			Button disabledBtn = new Button () {
				Text = "Robot Disabled",
				TextColor = Color.Black,
				BackgroundColor = Color.Red,
				FontSize = GlobalVariables.sizeMedium
			};
			disabledBtn.Clicked += (object sender, EventArgs e) => {
				if(disabled == false)
					disabledBtn.BackgroundColor = Color.Red;
				else
					disabledBtn.BackgroundColor = Color.Gray;
				disabled = !disabled;
			};

<<<<<<< HEAD
			data = obj;

=======
>>>>>>> 2cbb42c73b2a977ea0d0e3196afdd90918c0acf5
			Button endTeleOpBtn = new Button () {
				Text = "End TeleOp",
				TextColor = Color.Black,
				BackgroundColor = Color.Red,
				FontSize = GlobalVariables.sizeMedium
			};
			endTeleOpBtn.Clicked += (object sender, EventArgs e) => {
				//TryCatch everything, display alert & record

				if (goodStacker == true)
					data ["goodStack"] = true;
				else
					data ["goodStack"] = false;
	
				data ["CycleAmount"] = cycleCount;
				data ["TotalScore"] = Convert.ToInt32 (totalScore.ToString ());
				data ["CycleData"] = cyclePoints;
				data ["disabled"] = disabled;
				data ["teleopStepCanPulls"] = Convert.ToInt16 (stepCanPulls);
				data ["landfillTotes"] = landfillTotesTotal;
				data ["stationTotes"] = stationTotesTotal;
				data ["stepCanPull"] = totalStepCanPulls;
				data ["canUprightCount"] = canUpright;
				data ["humanThrows"] = litterCount;
				data ["humanThrowsSuccess"] = litterSuccesses;

				SaveData();
				Navigation.PushModalAsync(new PostMatchScoutingPage(data));

			};

			// Row 1
			TeleopLayout.Children.Add (currentCycleScoreLabel, 0, 2, 0, 1);
			TeleopLayout.Children.Add (stackScoreLabel, 2, 3, 0, 1);
			TeleopLayout.Children.Add (totalScoreLabel, 3, 4, 0, 1);
			TeleopLayout.Children.Add (totalScoreLabel, 5, 6, 0, 1);

			// Row 2
			TeleopLayout.Children.Add (stackingButton, 0, 3, 1, 2);

			// Row 3
			addcounter(0, 2);

			// Row 4
			addcounter(1, 3);

			// Row 5
			addcounter(2, 4);

			// Row 6
			TeleopLayout.Children.Add (litterStackBtn, 0, 3, 5, 6);

			// Row 7
			TeleopLayout.Children.Add (stackResetBtn, 0, 3, 6, 7);
			TeleopLayout.Children.Add (scoreBtn, 3, 6, 6, 7);

			// Row 8
			addcounter(3, 7);

			// Row 9
			addcounter(4, 8);

			// Row 10
			TeleopLayout.Children.Add (litterThowsLabel, 0, 1, 9, 10);
			TeleopLayout.Children.Add (litterThrowBtn, 1, 2, 9, 10);

			// Row 11
			TeleopLayout.Children.Add (litterResetBtn, 0, 1, 10, 11);
			TeleopLayout.Children.Add (litterSuccessesBtn, 1, 2, 10, 11);

			// Row 12
			TeleopLayout.Children.Add (coOpSetBtn, 0, 3, 11, 12);
			TeleopLayout.Children.Add (coOpStackBtn, 3, 6, 9, 12);

			// Row 13
			TeleopLayout.Children.Add (disabledBtn, 0, 6, 12, 13);

			// Row 14
			TeleopLayout.Children.Add (endTeleOpBtn, 0, 6, 13, 14);

			// Total Tote Count
			TeleopLayout.Children.Add (toteCount, 4, 5, 2, 4);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = TeleopLayout
			};
		}

		void dataCounter(string title, int count){
			counterLabel[labelCount] = new Label {
				Text = title,
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = GlobalVariables.sizeMedium
			};

			counterDisplay[labelCount] = new Label (){
				Text = "0",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = GlobalVariables.sizeMedium
			};

			plus [labelCount] = new Button () {		
				Text = "+",
				BackgroundColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};
			plus[labelCount].Clicked += (object sender, EventArgs e) => {
				count++;
				counterDisplay[labelCount].Text=count.ToString();
				//UpdateValues();
				/*
				if (landfillTotes+stationTotes < 6) { 
					landfillTotes++;
					UpdateValues();
					counterDisplay[labelCount].Text = landfillTotes.ToString();
					toteCount.Text = Convert.ToString(landfillTotes+stationTotes);
				}
				*/
			};

			minus[labelCount] = new Button () {	
				Text = "-",
				BackgroundColor = Color.Red,
				FontSize = GlobalVariables.sizeMedium
			};
			minus[labelCount].Clicked += (object sender, EventArgs e) => {
				if(count > 0 )
					count--;
				counterDisplay[labelCount].Text=count.ToString();
			};
			labelCount++;
		}

		void addcounter(int a, int yi){
			TeleopLayout.Children.Add (minus[a], 0, 1, yi, yi+1);
			TeleopLayout.Children.Add (counterLabel[a], 1, 2, yi, yi+1);
			TeleopLayout.Children.Add (plus[a], 2, 3, yi, yi+1);
			TeleopLayout.Children.Add (counterDisplay[a], 3, 4, yi, yi+1);
		}

		async void UpdateValues(){
			if (stacker == true)
				stackScore = ((landfillTotes + stationTotes) * 2) + (canCount * (((landfillTotes + stationTotes) * 4) + (litterStack * 6))) + (coOpSet * 20) + (coOpStack * 40);
			else
				stackScore = (canCount * (((landfillTotes + stationTotes) * 4) + (litterStack * 6))) + stationTotes*2 + (coOpSet * 20) + (coOpStack * 40);
			
			toteCount.Text = Convert.ToString (landfillTotes +stationTotes);  
			stackScoreLabel.Text = stackScore.ToString();
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}

