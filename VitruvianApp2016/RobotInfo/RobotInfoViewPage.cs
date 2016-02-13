using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class RobotInfoViewPage:ContentPage
	{
		ParseObject data;
		Image robotImage = new Image();

		int Z=0;
		Label[]	descriptionLabel = new Label[99];
		Label[]	dataLabel = new Label[99];
		int Z2=0;

		public RobotInfoViewPage (ParseObject teamData)
		{
			data = teamData;
			string teamNo;
			try{
				teamNo = teamData ["teamNo"].ToString ();
			}
			catch{
				teamNo = "<No Data Recorded>";
			}
			Label title = new Label {
				Text = teamNo + "'s Stats"
			};

			Title = title.Text;

			Grid grid = new Grid () {
				//Padding = new Thickness(0,20,0,0), 
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = new GridLength(160, GridUnitType.Absolute) },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = new GridLength(160, GridUnitType.Absolute) },
					new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			// RobotImage
			addRobotImage();

			ListView teamInfo = new ListView ();

			Label teamNumber = new Label ();
			try {
				if (teamData ["teamNumber"] != null) {
					teamNumber.Text = teamData ["teamNumber"].ToString();
				} else {}
			}
			catch {
				teamNumber.Text = "<No Team Number>";
			}
			teamNumber.FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label));

			Label teamName = new Label ();
			try {
				if (teamData ["teamName"] != null) {
					teamName.Text = teamData ["teamName"].ToString();
				} else {} 
			} catch {
				teamName.Text = "<No Team Name>";
			}
			teamName.FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label));

			data = teamData;

			listedItem("Drive Type:", "driveType");
			listedItem("Can use the low bar?:", "lowBarAccess");
			listedItem("Auto Strategy:", "autoStrategy");
			listedItem("TeleOp Strategy:", "teleOpStrategy");
			listedItem("Additional Notes:", "notes");
			Z2 = Z;
			listedItem("Highest Score:", "highScore1");
			listedItem("Second Highest Score:", "highScore2");
			listedItem("Third Highest Score:", "highScore3");
			listedItem("Thrid Lowest Score:", "lowScore3");
			listedItem("Second Lowest Score:", "lowScore2");
			listedItem("Lowest Score:", "lowScore1");
			listedItem("Portcullis Successes:", "totalA1");
			listedItem("Cheval de Frise Sccesses:", "totalA2");
			listedItem("Moat Successes:", "totalB1");
			listedItem("Rampart Successes:", "totalB2");
			listedItem("Drawbridge Successes:", "totalC1");
			listedItem("Sally Port Successes:", "totalC2");
			listedItem("Rock Wall Successes:", "totalD1");
			listedItem("Rough Terrain Successes:", "totalD2");
			listedItem("Low Bar Successes:", "totalE");


			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new RobotInfoViewPage(teamData));
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

			StackLayout side = new StackLayout () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					teamNumber,
					teamName
				}
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

				Children = {
					pitInfo,
					stats
				}
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,

				Children = {
					backBtn,
					refreshBtn
				}
			};

			grid.Children.Add (robotImage, 0, 0);
			grid.Children.Add (side, 1, 0);
			grid.Children.Add (allInfo, 0, 2, 1, 2);
			grid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content=grid
			};
		}

		void listedItem(string description, string parseString){
			descriptionLabel[Z] = new Label {
				Text = description,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				TextColor = Color.Green
			};

			dataLabel[Z] = new Label();
			try{
				if(data[parseString] != null)
					dataLabel[Z].Text = data [parseString].ToString();
			} catch {
				dataLabel[Z].Text = "<No Data Recorded>";
			}
			dataLabel [Z].FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label));

			Z++;
		}

		void addRobotImage(){
			try {
				if (data ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)data ["robotImage"];
					//Gets the image from parse and converts it to ParseFile
					//robotImage.Source = "I"+teamData["teamNumber"]+".jpg"; //Must scale down images manually before upload, & all images must be .jpg
					//How to write this so caching actually works?

					robotImage.Source = new UriImageSource{
						Uri = robotImageURL.Url,
						CachingEnabled = true,
						CacheValidity = new TimeSpan(7,0,0,0) //Caches Images onto your device for a week
					};
				} else {}
			}
			catch {
				robotImage.Source = "Placeholder_image_placeholder.png";
			}
			robotImage.Aspect = Aspect.AspectFit; //Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
			//robotImage.GestureRecognizers.Add (imageTap);
		}
	}
}

