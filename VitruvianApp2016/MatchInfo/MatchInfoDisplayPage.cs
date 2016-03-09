using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class MatchInfoDisplayPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		ParseObject data;
		ParseObject[] teamData = new ParseObject[6];

		ScrollView[] teamView = new ScrollView[6];
		StackLayout[] teamStack = new StackLayout[6];
		StackLayout[] pitInfo = new StackLayout [6];
		StackLayout[] stats = new StackLayout [6];
		Image[] robotImage = new Image[6];

		int[] teamNumber = new int[6];

		Grid layoutGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition{ Height = GridLength.Auto },
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = new GridLength(150, GridUnitType.Absolute)},
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid dataLayoutGrid = new Grid(){
			RowDefinitions = {
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				//new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid headerGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};
		Grid dataGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};

		int Z=0;
		int Z2=0;
		Label[,] descriptionLabel = new Label[6,99];
		Label[,] dataLabel = new Label[6,99];

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

			layoutGrid.Children.Add (dataLayoutGrid, 0, 2, 1, 2);
			layoutGrid.Children.Add (pageTitle, 0, 0);
			layoutGrid.Children.Add (busyIcon, 1, 0);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 2, 3);

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
			dataLayoutGrid.Children.Add (headerGrid, 0, 0);
			dataLayoutGrid.Children.Add (dataGrid, 0, 1);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		void addColumnHeaders(string headerName, int arrayIndexX){
			ColumnHeaderCell teamNumber = new ColumnHeaderCell ();
			teamNumber.header.Text = headerName;

			headerGrid.Children.Add (teamNumber, arrayIndexX, 0);
		}

		async void addTeamData(int getTeamNumber, int arrayIndex){

			dataGrid.Children.Clear ();
			teamStack [arrayIndex].Children.Clear ();

			ParseQuery<ParseObject> query = ParseObject.GetQuery ("TeamData");
			ParseQuery<ParseObject> findTeam = query.WhereEqualTo ("teamNumber", getTeamNumber);	
			var teamObj = await findTeam.FindAsync();
			foreach (ParseObject team in teamObj) {
				teamData[arrayIndex] = team;
			}

			Z = 0;

			// Add team data, should be similar to RobotInfoViewPage
			addRobotImage(teamData[arrayIndex], arrayIndex);
			listedItem ("Drive Train:", "driveType", teamData[arrayIndex], arrayIndex);
			listedItem ("Low Bar Capable:", "lowBarAccess", teamData[arrayIndex], arrayIndex);
			listedItem ("Intake Position:", "intakePos", teamData[arrayIndex], arrayIndex);
			listedItem ("Notes:", "notes", teamData[arrayIndex], arrayIndex);
			Z2 = Z;
			listedItem("Highest Score:", "highScore1", teamData[arrayIndex], arrayIndex);
			listedItem("Second Highest Score:", "highScore2", teamData[arrayIndex], arrayIndex);
			listedItem("Third Highest Score:", "highScore3", teamData[arrayIndex], arrayIndex);
			listedItem("Thrid Lowest Score:", "lowScore3", teamData[arrayIndex], arrayIndex);
			listedItem("Second Lowest Score:", "lowScore2", teamData[arrayIndex], arrayIndex);
			listedItem("Lowest Score:", "lowScore1", teamData[arrayIndex], arrayIndex);
			listedItem("Total High Goal Acc", "totalTeleOpHighAccuracy", teamData[arrayIndex], arrayIndex);
			listedItem("Best High Goal Acc", "bestTeleOpHighAccuracy", teamData[arrayIndex], arrayIndex);
			listedItem("Total Low Goal Acc", "totalTeleOpLowAccuracy", teamData[arrayIndex], arrayIndex);
			listedItem("Best Low Goal Acc", "bestTeleOpLowAccuracy", teamData[arrayIndex], arrayIndex);
			listedItem("Portcullis Successes:", "A1", teamData[arrayIndex], arrayIndex);
			listedItem("Cheval de Frise Sccesses:", "A2", teamData[arrayIndex], arrayIndex);
			listedItem("Moat Successes:", "B1", teamData[arrayIndex], arrayIndex);
			listedItem("Rampart Successes:", "B2", teamData[arrayIndex], arrayIndex);
			listedItem("Drawbridge Successes:", "C1", teamData[arrayIndex], arrayIndex);
			listedItem("Sally Port Successes:", "C2", teamData[arrayIndex], arrayIndex);
			listedItem("Rock Wall Successes:", "D1", teamData[arrayIndex], arrayIndex);
			listedItem("Rough Terrain Successes:", "D2", teamData [arrayIndex], arrayIndex);
			listedItem("Low Bar Successes:", "E", teamData[arrayIndex], arrayIndex);

			for(int i=0; i<Z2; i++){
				pitInfo[arrayIndex].Children.Add (descriptionLabel[arrayIndex,i]);
				pitInfo[arrayIndex].Children.Add (dataLabel[arrayIndex,i]);
			}

			for(int i=Z2; i<Z; i++){
				stats[arrayIndex].Children.Add(descriptionLabel[arrayIndex,i]);
				stats[arrayIndex].Children.Add (dataLabel [arrayIndex,i]);
			}

			teamStack [arrayIndex].Children.Add (pitInfo [arrayIndex]);
			teamStack [arrayIndex].Children.Add (stats [arrayIndex]);

			teamView [arrayIndex].Content = teamStack [arrayIndex];
			dataGrid.Children.Add (teamView [arrayIndex], arrayIndex, 0);
		}

		void listedItem(string description, string parseString, ParseObject tData, int arrayIndex){
			descriptionLabel [arrayIndex, Z].Text = description;

			try{
				if(tData[parseString] != null)
					dataLabel[arrayIndex,Z].Text = tData [parseString].ToString();
			} catch {
				dataLabel[arrayIndex,Z].Text = "<No Data Recorded>";
			}

			Z++;
		}

		void initialization(){
			for (int i = 0; i < 6; i++) {
				teamStack [i] = new StackLayout ();
				if (i < 3)
					teamStack [i].BackgroundColor = Color.Red;
				else
					teamStack [i].BackgroundColor = Color.Blue;
				
				teamView [i] = new ScrollView ();
				robotImage [i] = new Image ();

				pitInfo[i] = new StackLayout () {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};
				for (int j = 0; j < 99; j++) {
					descriptionLabel [i, j] = new Label {
						FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
						TextColor = Color.White
					};
					dataLabel [i, j] = new Label () {
						FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
					};
				}
				stats[i] = new StackLayout () {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};
				if (i < 3)
					stats [i].BackgroundColor = Color.Maroon;
				else
					stats [i].BackgroundColor = Color.Teal;
			}
		}

		void addRobotImage(ParseObject teamData, int arrayIndex){
			try {
				if (teamData ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)teamData ["robotImage"];
					//Gets the image from parse and converts it to ParseFile
					//robotImage.Source = "I"+teamData["teamNumber"]+".jpg"; //Must scale down images manually before upload, & all images must be .jpg
					//How to write this so caching actually works?

					robotImage[arrayIndex].Source = new UriImageSource{
						Uri = robotImageURL.Url,
						CachingEnabled = true,
						CacheValidity = new TimeSpan(7,0,0,0) //Caches Images onto your device for a week
					};
				} else {}
			}
			catch {
				robotImage[arrayIndex].Source = "Placeholder_image_placeholder.png";
			}
			robotImage[arrayIndex].Aspect = Aspect.AspectFit; //Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
			//robotImage.GestureRecognizers.Add (imageTap);

			teamStack [arrayIndex].Children.Add (robotImage[arrayIndex]);
			Z++;
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

