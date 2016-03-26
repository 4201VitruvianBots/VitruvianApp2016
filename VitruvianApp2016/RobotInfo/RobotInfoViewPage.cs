using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Rg.Plugins.Popup.Services;

namespace VitruvianApp2016
{
	public class RobotInfoViewPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		ParseObject data;
		ParseObject data2;
		CachedImage robotImage;
		CachedImage robotImageFull;

		int Z=0;
		Label[]	descriptionLabel = new Label[99];
		Label[]	dataLabel = new Label[99];
		int Z2=0;

		public RobotInfoViewPage (ParseObject teamData)
		{
			//refreshPage ();
			data = teamData;

			Grid topGrid = new Grid () {
				BackgroundColor = Color.Black,

				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = new GridLength(130, GridUnitType.Absolute) },
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = new GridLength(150, GridUnitType.Absolute) },
					new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
				}
			};
			Grid grid = new Grid () {
				//Padding = new Thickness(0,20,0,0), 
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Black,

				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition{ Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					//new ColumnDefinition{ Width = new GridLength(150, GridUnitType.Absolute) },
					new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			// RobotImage
			addRobotImage();

			ListView teamInfo = new ListView ();

			Label teamNumber = new Label () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				FontSize = GlobalVariables.sizeTitle,
				BackgroundColor = Color.Black,
				TextColor = Color.White,
			};
			try {
				if (teamData ["teamNumber"] != null) {
					teamNumber.Text = "Team " + teamData ["teamNumber"].ToString();
				} else {}
			}
			catch {
				teamNumber.Text = "<No Team Number>";
			}

			Label teamName = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				FontSize = GlobalVariables.sizeTitle,
				BackgroundColor = Color.Black,
			};
			try {
				if (teamData ["teamName"] != null) {
					teamName.Text = teamData ["teamName"].ToString();
				} else {} 
			} catch {
				teamName.Text = "<No Team Name>";
			}

			listedItem("Drive Type:", "driveType");
			listedItem("Can use the low bar?:", "lowBarAccess");
			listedItem("Intake Position:", "intakePos");
			listedItem("Auto Strategy:", "autoStrategy");
			listedItem("TeleOp Strategy:", "teleOpStrategy");
			listedItem("Additional Notes:", "notes");
			Z2 = Z;
			listedItem("Average Score:", "avgScore");
			listedItem("Highest Score:", "highScore1");
			listedItem("Second Highest Score:", "highScore2");
			listedItem("Third Highest Score:", "highScore3");
			listedItem("Thrid Lowest Score:", "lowScore3");
			listedItem("Second Lowest Score:", "lowScore2");
			listedItem("Lowest Score:", "lowScore1");
			listedItem("Total High Goal Acc", "totalTeleOpHighAccuracy");
			listedItem("Best High Goal Acc", "bestTeleOpHighAccuracy");
			listedItem("Total Low Goal Acc", "totalTeleOpLowAccuracy");
			listedItem("Best Low Goal Acc", "bestTeleOpLowAccuracy");
			listedItem("Portcullis Successes:", "A1");
			listedItem("Cheval de Frise Sccesses:", "A2");
			listedItem("Moat Successes:", "B1");
			listedItem("Rampart Successes:", "B2");
			listedItem("Drawbridge Successes:", "C1");
			listedItem("Sally Port Successes:", "C2");
			listedItem("Rock Wall Successes:", "D1");
			listedItem("Rough Terrain Successes:", "D2");
			listedItem("Low Bar Successes:", "E");

			//Refresh Button
			Button refreshBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus())
					Navigation.PushModalAsync(new RobotInfoViewPage(data));
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

			StackLayout pitInfo = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			for(int i=0; i<Z2; i++){
				pitInfo.Children.Add(descriptionLabel[i]);
				pitInfo.Children.Add (dataLabel [i]);
			}

			StackLayout stats = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Aqua
			};
			for(int i=Z2; i<Z; i++){
				stats.Children.Add(descriptionLabel[i]);
				stats.Children.Add (dataLabel [i]);
			}

			StackLayout allInfo = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.White,

				Children = {
					pitInfo,
					stats
				}
			};

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

			ScrollView infoScroll = new ScrollView () {
				Content = allInfo
			};
					
			topGrid.Children.Add (robotImage, 0, 1, 0, 2);
			topGrid.Children.Add (teamNumber, 1, 2, 0, 1);
			topGrid.Children.Add (teamName, 1, 2, 1, 2);
			topGrid.Children.Add (busyIcon, 2, 3, 0, 1);
			grid.Children.Add (infoScroll, 0, 1);
			grid.Children.Add (topGrid, 0, 0);
			grid.Children.Add (navigationBtns, 0, 2);

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					grid
				}
			};
		}

		void listedItem(string description, string parseString){
			descriptionLabel[Z] = new Label {
				Text = description,
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.Black
			};

			dataLabel [Z] = new Label (){ 
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.Gray
			};
			try{
				if(data[parseString] != null)
					dataLabel[Z].Text = data [parseString].ToString();
			} catch {
				dataLabel[Z].Text = "<No Data Recorded>";
			}
			dataLabel [Z].FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));

			Z++;
		}

		void listedItem2(string description, string parseString){
			descriptionLabel[Z] = new Label {
				Text = description,
				FontSize = GlobalVariables.sizeMedium,
				TextColor = Color.Black
			};

			dataLabel [Z] = new Label (){ 
				FontSize = GlobalVariables.sizeSmall,
				TextColor = Color.Gray
			};
			try{
				Console.WriteLine(data2.ClassName.ToString());
				if(data2[parseString] != null)
					dataLabel[Z].Text = data2 [parseString].ToString();
			} catch {
				dataLabel[Z].Text = "<No Data Recorded>";
			}
			dataLabel [Z].FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));

			Z++;
		}

		async void refreshPage(){
			data = await data.FetchAsync ();
			var test = new CalculateAverageData (Convert.ToInt16 (data ["teamNumber"].ToString ()));
			data = await data.FetchAsync ();
			/*
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.WhereEqualTo ("teamNumber", data ["teamNumber"]);

			var allTeams = await sorted.FindAsync();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchAsync ();
				data = obj;
			}
			*/
		}

		/*
		async void getTeamStats(){
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamStats");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");
			ParseQuery<ParseObject> filter = sorted.WhereEqualTo("teamNumber", Convert.ToInt16 (data ["teamNumber"].ToString ()));
			IEnumerable<ParseObject> teamSelect = await filter.FindAsync();

			foreach (ParseObject obj in teamSelect)
				data2 = obj;

			new CalculateAverageData (Convert.ToInt16 (data ["teamNumber"].ToString ()));
			Console.WriteLine(data2["highScore1"].ToString());
			SaveData2 ();
		}

		async void SaveData2(){
			await data2.SaveAsync ();
		}
		*/

		void addRobotImage(){
			try {
				if (data ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)data ["robotImage"];
					//Gets the image from parse and converts it to ParseFile
					robotImage = new CachedImage(){
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
					robotImageFull = new CachedImage(){
						Source = new UriImageSource{
							Uri = robotImageURL.Url,
							CachingEnabled = true,
							CacheValidity = new TimeSpan(7,0,0,0) //Caches Images onto your device for a week
						}
					};

					TapGestureRecognizer tap = new TapGestureRecognizer();
					tap.Tapped += (object sender, EventArgs e) => {
						// Create a gesture recognizer to display the popup image
						popUpPage(robotImageFull);
					};
					robotImage.GestureRecognizers.Add (tap);
					robotImage.Aspect = Aspect.AspectFit; 
				} else {}
			}
			catch {
				robotImage.Source = "Placeholder_image_placeholder.png";
			}
			robotImage.Aspect = Aspect.AspectFit; //Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
			//robotImage.GestureRecognizers.Add (imageTap);
		}

		async void popUpPage(CachedImage rImage){
			await Task.Yield ();
			await PopupNavigation.PushAsync (new ImagePopupPage (rImage), false);
		}
	}
}

