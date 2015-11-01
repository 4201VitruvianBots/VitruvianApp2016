using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class ViewTeamPage:ContentPage
	{
		ParseObject data;
		Image robotImage = new Image();
		int Z = 0;

		Label[] descriptionLabel = new Label[999];
		Label[] dataLabel = new Label[999];

		public ViewTeamPage (ParseObject teamData)
		{
			string teamNo = teamData ["teamNo"].ToString ();
			Label title = new Label {
				Text = teamNo + "'s Stats"
			};

			Title = title.Text;

			Grid grid = new Grid () {
				//Padding = new Thickness(0,20,0,0), 
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = new GridLength(160, GridUnitType.Absolute) },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = new GridLength(160, GridUnitType.Absolute) },
					new ColumnDefinition{ Width = GridLength.Auto }
				}
			};

			Image robotImage = new Image();
			try {
				if (teamData ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)teamData ["robotImage"];
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

			listedItem("Robot Weight:", "robotWeight");
			listedItem("Requires Ramp:", "ramp");
			listedItem("Drive Type:", "driveType");
			listedItem("Tote Pickup Orientation:", "toteOrientation");
			listedItem("Can Pickup Orientaiton:", "canOrientation");
			listedItem("Auto Strategy:", "autoStrategy");
			listedItem("Can push tote in Auto?:", "autoTote");
			listedItem("TeleOp Strategy:", "teleOpStrategy");
			listedItem("# of Co-Op Totes they can stack:", "coopertitionTotes");
			listedItem("Additional Notes:", "notes");

			data = teamData;

			/*
			Button viewStatsBtn = new Button {
				Text = "View Stats",
				TextColor = Color.Green,
				BackgroundColor= Color.Black
			};
			viewStatsBtn.Clicked += (object sender, EventArgs e) => {
				//Navigation.PushModalAsync(new TeamStatGenerator(Convert.ToInt32(teamData["teamNumber"].ToString())));
			};
			*/

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new ViewTeamPage(teamData));
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


			StackLayout info = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			for(int i=0; i<Z; i++){
				info.Children.Add(descriptionLabel[i]);
				info.Children.Add (dataLabel [i]);
			}

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
			grid.Children.Add (info, 0, 2, 1, 2);
			//grid.Children.Add (viewStatsBtn, 0, 2, 2, 3);
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
	}
}

