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
		ParseObject teamData;


		ScrollView[] teamView = new ScrollView[6];
		StackLayout[] teamStack = new StackLayout[6];
		Image[] robotImage = new Image[6];

		int[] teamNumber = new int[6];

		Grid layoutGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition{ Height = GridLength.Auto },
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

		int X1=0;
		int X2=0;
		int Z=0;
		Label[]	descriptionLabel = new Label[999];
		Label[]	dataLabel = new Label[999];

		public MatchInfoDisplayPage (ParseObject matchInfo)
		{
			initializeStacks ();
			data = matchInfo;

			Label pageTitle = new Label () {
				Text = "Match " + matchInfo["matchNo"].ToString(),
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				PopulateData(matchInfo);
			};

			//Back Button
			Button backBtn = new Button () {
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			// Navigation Panel
			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn,
					refreshBtn
				}
			};

			layoutGrid.Children.Add (dataLayoutGrid, 0, 2, 1, 2);
			layoutGrid.Children.Add (pageTitle, 0, 0);
			layoutGrid.Children.Add (busyIcon, 1, 0);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					layoutGrid
				}
			};
			PopulateData (matchInfo);
		}

		void PopulateData(ParseObject matchData){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			teamNumber[0] = Convert.ToInt16 (matchData ["red1"].ToString ());
			teamNumber[1] = Convert.ToInt16 (matchData ["red2"].ToString ());
			teamNumber[2] = Convert.ToInt16 (matchData ["red3"].ToString ());
			teamNumber[3] = Convert.ToInt16 (matchData ["blue1"].ToString ());
			teamNumber[4] = Convert.ToInt16 (matchData ["blue1"].ToString ());
			teamNumber[5] = Convert.ToInt16 (matchData ["blue2"].ToString ());

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
			ColumnHeader teamNumber = new ColumnHeader ();
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
				teamData = team;
			}

			X1 = 0;
			X2 = 0;
			Z = 0;

			// Add team data, should be similar to RobotInfoViewPage
			addRobotImage(teamData, arrayIndex);
			listedItem ("Drive Train", "driveType", teamData, arrayIndex);
			listedItem ("Low Bar Capable", "lowBarAccess", teamData, arrayIndex);

			teamView [arrayIndex].Content = teamStack [arrayIndex];
			dataGrid.Children.Add (teamView [arrayIndex], arrayIndex, 0);
		}

		void listedItem(string description, string parseString, ParseObject teamData, int arrayIndex){
			descriptionLabel[X1] = new Label {
				Text = description,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				TextColor = Color.White
			};

			dataLabel[X2] = new Label();
			try{
				if(data[parseString] != null)
					dataLabel[X2].Text = teamData [parseString].ToString();
			} catch {
				dataLabel[X2].Text = "<No Data Recorded>";
			}
			dataLabel [X2].FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));

			teamStack [arrayIndex].Children.Add (descriptionLabel [X1]);
			teamStack [arrayIndex].Children.Add (dataLabel [X2]);
			X1++;
			X2++;
			Z++;
		}

		void initializeStacks(){
			for (int i = 0; i < 6; i++) {
				teamStack [i] = new StackLayout ();
				if (i < 3)
					teamStack [i].BackgroundColor = Color.Red;
				else
					teamStack [i].BackgroundColor = Color.Blue;
				
				teamView [i] = new ScrollView ();
				robotImage [i] = new Image ();
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
	}
}

