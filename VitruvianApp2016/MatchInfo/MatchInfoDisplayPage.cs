using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FFImageLoading.Forms;

namespace VitruvianApp2016
{
	public class MatchInfoDisplayPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		ParseObject data;
		ParseObject[] teamData = new ParseObject[6];

		ScrollView[] teamView = new ScrollView[6];
		StackLayout[] pitInfo = new StackLayout [6];
		StackLayout[] defStats = new StackLayout [6];
		CachedImage[] robotImage = new CachedImage[6];
		CachedImage[] robotImageFull = new CachedImage[6];

		int[] teamNumber = new int[6];

		Grid layoutGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			Padding = 0,
			ColumnSpacing = 0,
			RowSpacing = 0,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },							// Title
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },		// dataLayoutGrid
				new RowDefinition{ Height = GridLength.Auto },							// navigationBtns
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto },
				new ColumnDefinition{ Width = GridLength.Auto },
				new ColumnDefinition{ Width = new GridLength(150, GridUnitType.Absolute)},
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid dataLayoutGrid = new Grid(){
			Padding = 0,
			ColumnSpacing = 0,
			RowSpacing = 0,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },							// defItems
				new RowDefinition{ Height = GridLength.Auto },							// shotItems
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },		// pitInfo
				//new RowDefinition { Height = new GridLength (150, GridUnitType.Absolute)},
				//new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid statsGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
			}
		};
		Grid headerGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,

			RowDefinitions = {
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (120, GridUnitType.Absolute)},
			}
		};
		Grid rowHeaderGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 2,
			RowSpacing = 1,

			RowDefinitions = {
				new RowDefinition { Height = new GridLength (85, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
			}
		};
		Grid dataGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};
		Grid[] defGrid = new Grid[6];
		Grid[] shotGrid = new Grid[6];

		int rowHeaders = 1;
		int gridHieght = 0;
		int Z=0;
		Label[] rowHeaderLabels = new Label[20];
		Label[,] descriptionLabel = new Label[6,99];
		Label[,] dataLabel = new Label[6,99];
		ColumnHeaderCellSmall[,] defHeaderCells = new ColumnHeaderCellSmall[6,4];
		ColumnHeaderCellSmall[,] shotHeaderCells = new ColumnHeaderCellSmall[6,3];
		double redScore = 0;
		double blueScore = 0;

		public MatchInfoDisplayPage (ParseObject matchInfo)
		{
			initialization ();
			data = matchInfo;

			Label pageTitle = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Match " + matchInfo["matchNo"].ToString(),
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus())
					refreshPage();
			};

			//Back Button
			Button backBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			// Navigation Panel
			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn,
					refreshBtn
				}
			};

			if (new CheckInternetConnectivity().InternetStatus())
				PopulateData (matchInfo);

			layoutGrid.Children.Add (rowHeaderGrid, 0, 1, 0, 2);
			layoutGrid.Children.Add (dataLayoutGrid, 1, 3, 1, 2);
			layoutGrid.Children.Add (pageTitle, 0, 3, 0, 1);
			layoutGrid.Children.Add (busyIcon, 2, 0);
			layoutGrid.Children.Add (navigationBtns, 1, 3, 2, 3);

			this.Content = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					layoutGrid
				}
			};
		}

		void PopulateData(ParseObject matchData){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			teamNumber[0] = Convert.ToInt16 (matchData ["red1"].ToString ());
			teamNumber[1] = Convert.ToInt16 (matchData ["red2"].ToString ());
			teamNumber[2] = Convert.ToInt16 (matchData ["red3"].ToString ());
			teamNumber[3] = Convert.ToInt16 (matchData ["blue1"].ToString ());
			teamNumber[4] = Convert.ToInt16 (matchData ["blue2"].ToString ());
			teamNumber[5] = Convert.ToInt16 (matchData ["blue3"].ToString ());

			dataLayoutGrid.Children.Clear ();
			headerGrid.Children.Clear ();

			for (int i = 0; i < 6; i++) {
				addColumnHeaders (teamNumber [i].ToString (), i);
				addTeamData (teamNumber [i], i);
			}

			dataLayoutGrid.Children.Add (statsGrid, 0, 1);
			dataLayoutGrid.Children.Add (headerGrid, 0, 0);
			dataLayoutGrid.Children.Add (dataGrid, 0, 2);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		void addColumnHeaders(string headerName, int arrayIndexX){
			ColumnHeaderCell teamNumber = new ColumnHeaderCell ();
			teamNumber.header.Text = headerName;

			headerGrid.Children.Add (teamNumber, 2 * arrayIndexX, 0);
		}

		async void addTeamData(int getTeamNumber, int arrayIndex){

			dataGrid.Children.Clear ();
			pitInfo [arrayIndex].Children.Clear ();

			ParseQuery<ParseObject> query = ParseObject.GetQuery ("TeamData");
			ParseQuery<ParseObject> findTeam = query.WhereEqualTo ("teamNumber", getTeamNumber);	
			var teamObj = await findTeam.FindAsync();
			foreach (ParseObject team in teamObj) {
				teamData[arrayIndex] = team;
			}
			DataCell avgScoreCell = new DataCell ();
			avgScoreCell.data.TextColor = Color.White;
			if (arrayIndex < 3)
				avgScoreCell.BackgroundColor = Color.Red;
			else
				avgScoreCell.BackgroundColor = Color.Blue;
			try{
				avgScoreCell.data.Text = String.Format("{0:N2}", teamData[arrayIndex]["avgScore"]);
			}
			catch{
				avgScoreCell.data.Text = "NULL";
			}
			headerGrid.Children.Add (avgScoreCell, 2*arrayIndex + 1, 0);


			calculateAllianceScore (teamData[arrayIndex], arrayIndex);

			Z = 0;
			gridHieght = 0;

			// Add team data, should be similar to RobotInfoViewPage
			addRobotImage(teamData[arrayIndex], arrayIndex);
			defGrid [arrayIndex].Children.Add (defHeaderCells [arrayIndex,0], 0, gridHieght);
			defGrid [arrayIndex].Children.Add (defHeaderCells [arrayIndex,1], 1, gridHieght);
			defGrid [arrayIndex].Children.Add (defHeaderCells [arrayIndex,2], 2, gridHieght);
			defGrid [arrayIndex].Children.Add (defHeaderCells [arrayIndex,3], 3, gridHieght++);
			//shotGrid [arrayIndex].Children.Add (avgScoreCell, 1, gridHieght++);
			addDefItems ("E", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("A1", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("A2", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("B1", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("B2", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("C1", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("C2", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("D1", teamData [arrayIndex], arrayIndex, gridHieght++);
			addDefItems ("D2", teamData [arrayIndex], arrayIndex, gridHieght++);
			//listedItem("Low Bar Successes:", "E", teamData[arrayIndex], arrayIndex);
			//listedItem("Average Score:", "avgScore", teamData[arrayIndex], arrayIndex);
			//listedItem("Portcullis Successes:", "A1", teamData[arrayIndex], arrayIndex);
			//listedItem("Cheval de Frise Sccesses:", "A2", teamData[arrayIndex], arrayIndex);
			//listedItem("Moat Successes:", "B1", teamData[arrayIndex], arrayIndex);
			//listedItem("Rampart Successes:", "B2", teamData[arrayIndex], arrayIndex);
			//listedItem("Drawbridge Successes:", "C1", teamData[arrayIndex], arrayIndex);
			//listedItem("Sally Port Successes:", "C2", teamData[arrayIndex], arrayIndex);
			//listedItem("Rock Wall Successes:", "D1", teamData[arrayIndex], arrayIndex);
			//listedItem("Rough Terrain Successes:", "D2", teamData [arrayIndex], arrayIndex);


			gridHieght = 0;
			shotGrid [arrayIndex].Children.Add (shotHeaderCells [arrayIndex,0], 0, gridHieght);
			shotGrid [arrayIndex].Children.Add (shotHeaderCells [arrayIndex,1], 1, gridHieght);
			shotGrid [arrayIndex].Children.Add (shotHeaderCells [arrayIndex,2], 2, gridHieght++);

			addShotItems ("teleOpHigh", teamData [arrayIndex], arrayIndex, gridHieght++);
			addShotItems ("teleOpLow", teamData [arrayIndex], arrayIndex, gridHieght++);

			//listedItem("Total High Goal Acc", "totalTeleOpHighAccuracy", teamData[arrayIndex], arrayIndex);
			//listedItem("Best High Goal Acc", "bestTeleOpHighAccuracy", teamData[arrayIndex], arrayIndex);
			//listedItem("Total Low Goal Acc", "totalTeleOpLowAccuracy", teamData[arrayIndex], arrayIndex);
			//listedItem("Best Low Goal Acc", "bestTeleOpLowAccuracy", teamData[arrayIndex], arrayIndex);

			//listedItem ("Drive Train:", "driveType", teamData[arrayIndex], arrayIndex);
			listedItem ("Can LowBar: ", "lowBarAccess", teamData[arrayIndex], arrayIndex);
			//listedItem ("Intake Position:", "intakePos", teamData[arrayIndex], arrayIndex);
			listedItem ("Notes:", "notes", teamData[arrayIndex], arrayIndex);
			//listedItem ("Match Notes:", "matchNotes", teamData[arrayIndex], arrayIndex);

			teamView [arrayIndex].Content = pitInfo [arrayIndex];
			statsGrid.Children.Add (defGrid [arrayIndex], arrayIndex, 0);
			statsGrid.Children.Add (shotGrid [arrayIndex], arrayIndex, 1);
			dataGrid.Children.Add (teamView [arrayIndex], arrayIndex, 0);
		}

		void listedItem(string description, string parseString, ParseObject tData, int arrayIndex){
			if (description != "Notes:") {
				if(arrayIndex == 0)
					addRowHeaders (description);

				try {
					if (tData [parseString] != null)
						dataLabel [arrayIndex, Z].Text = tData [parseString].ToString ();
				} catch {
					dataLabel [arrayIndex, Z].Text = "<No Data Recorded>";
				}
				pitInfo[arrayIndex].Children.Add (dataLabel[arrayIndex,Z]);
			}
			else if (description == "Notes:"){
				descriptionLabel [arrayIndex, Z].Text = description;

				try {
					if (tData [parseString] != null)
						dataLabel [arrayIndex, Z].Text = tData [parseString].ToString ();
				} catch {
					dataLabel [arrayIndex, Z].Text = "<No Data Recorded>";
				}

				try{
					if (string.IsNullOrEmpty(tData ["matchNotes"].ToString()) == false) {
						descriptionLabel [arrayIndex, Z].BackgroundColor = Color.Fuchsia;

						TapGestureRecognizer tap = new TapGestureRecognizer ();
						tap.Tapped += (object sender, EventArgs e) => {
							DisplayAlert ("Match Notes", tData ["matchNotes"].ToString (), "OK");
						};
						descriptionLabel [arrayIndex, Z].GestureRecognizers.Add (tap);
					}
				}
				catch{
				}
				pitInfo[arrayIndex].Children.Add (descriptionLabel[arrayIndex,Z]);
				pitInfo[arrayIndex].Children.Add (dataLabel[arrayIndex,Z]);
			}
			Z++;
		}

		void initialization(){
			redScore = 0;
			blueScore = 0;
			for (int i = 0; i < 6; i++) {
				
				teamView [i] = new ScrollView ();

				pitInfo[i] = new StackLayout () {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};
				if (i < 3)
					pitInfo [i].BackgroundColor = Color.Maroon;
				else
					pitInfo [i].BackgroundColor = Color.Teal;
				
				for (int j = 0; j < 99; j++) {
					descriptionLabel [i, j] = new Label {
						FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
						TextColor = Color.White
					};
					dataLabel [i, j] = new Label () {
						FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
					};
				}
				defGrid [i] = new Grid () {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.Black,
					ColumnSpacing = 1,
					RowSpacing = 1,

					ColumnDefinitions = {
						new ColumnDefinition{ Width = new GridLength (1, GridUnitType.Star) },
						new ColumnDefinition{ Width = new GridLength (1, GridUnitType.Star) },
						new ColumnDefinition{ Width = new GridLength (1, GridUnitType.Star) },
						new ColumnDefinition{ Width = new GridLength (1, GridUnitType.Star) },
					},
					RowDefinitions = {
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
						new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) },
					}
				};
				shotGrid[i] = new Grid(){
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.Black,
						ColumnSpacing = 1,
						RowSpacing = 1,

						ColumnDefinitions = {
							new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
							new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
							new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
							new ColumnDefinition{ Width = GridLength.Auto },
						},
						RowDefinitions = {
							new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
							new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
							new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
						}
				};
				
				defHeaderCells [i, 0] = new ColumnHeaderCellSmall ();
				defHeaderCells [i, 0].header.Text = "Stall";
				defHeaderCells [i, 1] = new ColumnHeaderCellSmall ();
				defHeaderCells [i, 1].header.Text = "Pass";
				defHeaderCells [i, 2] = new ColumnHeaderCellSmall ();
				defHeaderCells [i, 2].header.Text = "Tries";
				defHeaderCells [i, 3] = new ColumnHeaderCellSmall ();
				defHeaderCells [i, 3].header.Text = "Faced";
				shotHeaderCells [i, 0] = new ColumnHeaderCellSmall ();
				shotHeaderCells [i, 0].header.Text = "Made";
				shotHeaderCells [i, 1] = new ColumnHeaderCellSmall ();
				shotHeaderCells [i, 1].header.Text = "Tried";
				shotHeaderCells [i, 2] = new ColumnHeaderCellSmall ();
				shotHeaderCells [i, 2].header.Text = "%";
			}
			for (int i = 0; i < 20; i++)
				rowHeaderLabels [i] = new Label () {
					FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.White
				};

			addRowHeaders ("Red Total:");
			addRowHeaders ("test");
			addRowHeaders ("Blue Total:");
			addRowHeaders ("test");
			addRowHeaders (" ");
			addRowHeaders ("Low Bar ");
			addRowHeaders ("Port");
			addRowHeaders ("Cheval");
			addRowHeaders ("Moat");
			addRowHeaders ("Ramp");
			addRowHeaders ("Draw");
			addRowHeaders ("Sally");
			addRowHeaders ("Rock");
			addRowHeaders ("Rough");
			addRowHeaders (" ");
			addRowHeaders ("High");
			addRowHeaders ("Low");
		}

		void addRowHeaders(string description){
			if(rowHeaders == 2)
				rowHeaderLabels [rowHeaders].Text = "0";
			else if(rowHeaders == 4)
				rowHeaderLabels [rowHeaders].Text = "0";
			else
				rowHeaderLabels [rowHeaders].Text = description;
			rowHeaderGrid.Children.Add (rowHeaderLabels [rowHeaders], 0, rowHeaders);
			rowHeaders++;
		}
		void calculateAllianceScore (ParseObject tData, int arrayIndex){
			if (arrayIndex < 3) {
				redScore += Convert.ToDouble (tData ["avgScore"]);
				rowHeaderLabels [2].Text = String.Format("{0:N2}", redScore);
			} else {
				blueScore += Convert.ToInt16 (tData ["avgScore"]);
				rowHeaderLabels [4].Text = String.Format("{0:N2}", blueScore);
			}
		}

		void addDefItems(string defense, ParseObject tData, int arrayIndex, int gridHieght){
			DataCell cell1 = new DataCell ();
			DataCell cell2 = new DataCell ();
			DataCell cell3 = new DataCell ();
			DataCell cell4 = new DataCell ();
			cell1.data.TextColor = Color.White;
			cell2.data.TextColor = Color.White;
			cell3.data.TextColor = Color.White;
			cell4.data.TextColor = Color.White;
			if (arrayIndex < 3) {
				cell1.BackgroundColor = Color.Red;
				cell2.BackgroundColor = Color.Red;
				cell3.BackgroundColor = Color.Red;
				cell4.BackgroundColor = Color.Red;
			} else {
				cell1.BackgroundColor = Color.Blue;
				cell2.BackgroundColor = Color.Blue;
				cell3.BackgroundColor = Color.Blue;
				cell4.BackgroundColor = Color.Blue;
			}
			try{
				cell1.data.Text = tData[defense+"sta"].ToString();
			}
			catch{
				cell1.data.Text = "NULL";
			}
			defGrid [arrayIndex].Children.Add (cell1, 0, gridHieght);
			try{
				cell2.data.Text = tData[defense].ToString();
			}
			catch{
				cell2.data.Text = "NULL";
			}
			defGrid [arrayIndex].Children.Add (cell2, 1, gridHieght);
			try{
				cell3.data.Text = tData[defense+"att"].ToString();
			}
			catch{
				cell3.data.Text = "NULL";
			}
			defGrid [arrayIndex].Children.Add (cell3, 2, gridHieght);
			try{
				cell4.data.Text = tData[defense+"count"].ToString();
			}
			catch{
				cell4.data.Text = "N";
			}
			defGrid [arrayIndex].Children.Add (cell4, 3, gridHieght);
		}

		void addShotItems(string shot, ParseObject tData, int arrayIndex, int gridHieght){
			DataCell cell1 = new DataCell ();
			DataCell cell2 = new DataCell ();
			DataCell cell3 = new DataCell ();
			cell1.data.TextColor = Color.White;
			cell2.data.TextColor = Color.White;
			cell3.data.TextColor = Color.White;
			if (arrayIndex < 3) {
				cell1.BackgroundColor = Color.Red;
				cell2.BackgroundColor = Color.Red;
				cell3.BackgroundColor = Color.Red;
			} else {
				cell1.BackgroundColor = Color.Blue;
				cell2.BackgroundColor = Color.Blue;
				cell3.BackgroundColor = Color.Blue;
			}
			try{
				cell1.data.Text = tData[shot+"Successes"].ToString();
			}
			catch{
				cell1.data.Text = "NULL";
			}
			shotGrid [arrayIndex].Children.Add (cell1, 0, gridHieght);
			try{
				cell2.data.Text = tData[shot+"Total"].ToString();
			}
			catch{
				cell2.data.Text = "NULL";
			}
			shotGrid [arrayIndex].Children.Add (cell2, 1, gridHieght);
			try{
				cell3.data.Text = string.Format("{0:P}", tData[shot+"Accuracy"]);
			}
			catch{
				cell3.data.Text = "NULL";
			}
			shotGrid [arrayIndex].Children.Add (cell3, 2, gridHieght);
		}

		void addRobotImage(ParseObject teamData, int arrayIndex){
			try {
				if (teamData ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)teamData ["robotImage"];
					//Gets the image from parse and converts it to ParseFile

					robotImage[arrayIndex] = new CachedImage(){
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						Source = new UriImageSource{
							Uri = robotImageURL.Url,
							CachingEnabled = true,
							CacheValidity = new TimeSpan(7,0,0,0) //Caches Images onto your device for a week
						},
						HeightRequest = 120,
						DownsampleToViewSize = true
					};
					robotImageFull[arrayIndex] = new CachedImage(){
						Source = new UriImageSource{
							Uri = robotImageURL.Url,
							CachingEnabled = true,
							CacheValidity = new TimeSpan(7,0,0,0) //Caches Images onto your device for a week
						}
					};

					TapGestureRecognizer tap = new TapGestureRecognizer();
					tap.Tapped += (object sender, EventArgs e) => {
						// Create a gesture recognizer to display the popup image
						popUpPage(robotImageFull[arrayIndex]);
					};
					robotImage[arrayIndex].GestureRecognizers.Add (tap);
					robotImage[arrayIndex].Aspect = Aspect.AspectFit; 
				} else {}
			}
			catch {
				robotImage[arrayIndex].Source = "Placeholder_image_placeholder.png";
			}

			headerGrid.Children.Add (robotImage[arrayIndex], 2 * arrayIndex, 2 * (arrayIndex + 1 ), 1, 2);
		}

		async void popUpPage(CachedImage rImage){
			await Task.Yield ();
			await PopupNavigation.PushAsync (new ImagePopupPage (rImage), false);
		}
			
		async void refreshPage(){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			for(int i=0; i< 6; i++){
				new CalculateAverageData (teamNumber[i]);
				ParseQuery<ParseObject> refresh = ParseObject.GetQuery ("TeamData");
				ParseQuery<ParseObject> sorted = refresh.WhereEqualTo ("teamNumber", teamNumber[i]);

				var allTeams = await sorted.FindAsync ();
				foreach (ParseObject obj in allTeams)
					teamData [i] = obj;
			}

			PopulateData(data);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

